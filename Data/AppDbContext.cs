using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Model;



namespace StudentFeeManagement.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>  // ✅ Ensure it inherits from DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<StudentFee> StudentFees { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<StudentFeeEditRequest> StudentFeeEditRequests { get; set; }
        public DbSet<Student> Students { get; set; }
        public async Task<StudentFeeSummary> GetStudentFeeSummaryAsync()
        {
            return new StudentFeeSummary
            {
                TotalStudents = await StudentFees
                    .Where(f => !string.IsNullOrEmpty(f.StudentName))
                    .CountAsync(),

                // Ensure null values are handled properly
                TotalAdmissionFee = await StudentFees.SumAsync(f => (decimal?)f.AdmissionFee) ?? 0,
                TotalAdmissionAmountPaid = await StudentFees.SumAsync(f => (decimal?)f.AdmissionAmountPaid) ?? 0,
                TotalAdmissionDue = await StudentFees.SumAsync(f => (decimal?)(f.AdmissionFee - f.AdmissionAmountPaid)) ?? 0,

                TotalFirstTermFee = await StudentFees.SumAsync(f => (decimal?)f.FirstTermFee) ?? 0,
                TotalFirstTermAmountPaid = await StudentFees.SumAsync(f => (decimal?)f.FirstTermAmountPaid) ?? 0,
                TotalFirstTermDue = await StudentFees.SumAsync(f => (decimal?)(f.FirstTermFee - f.FirstTermAmountPaid)) ?? 0,

                TotalSecondTermFee = await StudentFees.SumAsync(f => (decimal?)f.SecondTermFee) ?? 0,
                TotalSecondTermAmountPaid = await StudentFees.SumAsync(f => (decimal?)f.SecondTermAmountPaid) ?? 0,
                TotalSecondTermDue = await StudentFees.SumAsync(f => (decimal?)(f.SecondTermFee - f.SecondTermAmountPaid)) ?? 0,

                TotalAnnualFees = await StudentFees.SumAsync(f => (decimal?)(f.AdmissionFee + f.FirstTermFee + f.SecondTermFee)) ?? 0,
                TotalDues = await StudentFees.SumAsync(f => (decimal?)(f.AdmissionFee - f.AdmissionAmountPaid +
                                                                      f.FirstTermFee - f.FirstTermAmountPaid +
                                                                      f.SecondTermFee - f.SecondTermAmountPaid)) ?? 0,

                TotalConcession = await StudentFees.SumAsync(f => (decimal?)f.Concession) ?? 0,

                TotalBusFirstTermFee = await StudentFees.SumAsync(f => (decimal?)f.BusFirstTermFee) ?? 0,
                TotalBusFirstTermAmountPaid = await StudentFees.SumAsync(f => (decimal?)f.BusFirstTermAmountPaid) ?? 0,
                TotalBusFirstTermDue = await StudentFees.SumAsync(f => (decimal?)(f.BusFirstTermFee - f.BusFirstTermAmountPaid)) ?? 0,

                TotalBusSecondTermFee = await StudentFees.SumAsync(f => (decimal?)f.BusSecondTermFee) ?? 0,
                TotalBusSecondTermAmountPaid = await StudentFees.SumAsync(f => (decimal?)f.BusSecondTermAmountPaid) ?? 0,
                TotalBusSecondTermDue = await StudentFees.SumAsync(f => (decimal?)(f.BusSecondTermFee - f.BusSecondTermAmountPaid)) ?? 0,
            };
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.AdmissionFee)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.AdmissionAmountPaid)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.FirstTermFee)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.FirstTermAmountPaid)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.SecondTermFee)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.SecondTermAmountPaid)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.BusFirstTermFee)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.BusFirstTermAmountPaid)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.BusSecondTermFee)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.BusSecondTermAmountPaid)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentFee>()
                .Property(f => f.Concession)
                .HasColumnType("decimal(18,2)");




        }
        
    }
}

