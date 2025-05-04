using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using StudentFeeManagement.Data;
using StudentFeeManagement.Service;
using System.Data.Common;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddSignalR();

// Add Hangfire with the correct connection string
builder.Services.AddHangfire(config =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    config
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(connectionString);
});

// Add Hangfire server to process background jobs
builder.Services.AddHangfireServer();

builder.Services.AddHangfireServer();
builder.Services.AddScoped<RestoreService>();  // Register RestoreService

builder.Services.AddScoped<DatabaseHealthCheckService>(); //
builder.Services.AddScoped<DataBackupService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auth Demo",
        Version = "v1"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Enter a Token ",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddHealthChecks();
// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Register Audit & CEO Notification Services
builder.Services.AddScoped<AuditService>();

// Authentication and JWT Setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        b => b.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
});
builder.Services.AddScoped<StudentService>();


var app = builder.Build();

// Configure middleware for the HTTP request pipeline
if (app.Environment.IsDevelopment())
{

    //app.UseCors(policy =>
    //{
    //    policy.WithOrigins("https://localhost:7263")
    //        .AllowAnyMethod()
    //        .AllowAnyHeader()
    //        .WithHeaders(HeaderNames.ContentType)
    //        .SetIsOriginAllowed(origin => true);
    //});
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseHealthChecks("/health");
// Step 2: Create CEO Role & Assign User
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Ensure CEO Role Exists
    string ceoRole = "CEO";
    if (!await roleManager.RoleExistsAsync(ceoRole))
    {
        await roleManager.CreateAsync(new IdentityRole(ceoRole));
    }

    // Assign CEO Role to a Specific User
    string ceoEmail = "buildmybusinessu1@gmail.com"; // Change to your CEO's email
    var ceoUser = await userManager.FindByEmailAsync(ceoEmail);

    if (ceoUser == null)
    {
        ceoUser = new IdentityUser
        {
            UserName = ceoEmail,
            Email = ceoEmail,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(ceoUser, "Unit1@1234"); // Set a strong password
    }

    if (!await userManager.IsInRoleAsync(ceoUser, ceoRole))
    {
        await userManager.AddToRoleAsync(ceoUser, ceoRole);
    }
}
app.UseHangfireDashboard("/hangfire");

RecurringJob.AddOrUpdate<DataBackupService>(
    "backup-all-data",                        // Unique job id
    service => service.BackupAllAsync(),       // Method to call
    Cron.HourInterval(8)                      // Cron expression for every 8 hours
);

RecurringJob.AddOrUpdate<DatabaseHealthCheckService>(
    "database-health-check",
    x => x.CheckAndRecoverAsync(),
    Cron.Hourly   // Check every 1 hour
);

app.Run();
