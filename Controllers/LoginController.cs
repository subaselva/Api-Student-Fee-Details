using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StudentFeeManagement.Model;
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly AuditService _auditService;  // Inject AuditService

    public LoginController(IConfiguration configuration, SignInManager<IdentityUser> signInManager, AuditService auditService)
    {
        _configuration = configuration;
        _signInManager = signInManager;
        _auditService = auditService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var result = await _signInManager.PasswordSignInAsync(login.Email!, login.Password!, false, false);

        if (!result.Succeeded)
        {
            await _auditService.LogLoginAttempt(login.Email!, false, "Invalid credentials");
            return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });
        }

        var user = await _signInManager.UserManager.FindByEmailAsync(login.Email!);
        var roles = await _signInManager.UserManager.GetRolesAsync(user!);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, login.Email!)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["Jwt:ExpiresInHours"]));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiry,
            signingCredentials: creds
        );

        // ✅ Log successful login
        await _auditService.LogLoginAttempt(login.Email!, true);

        return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}


public class LoginModel
{
    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }
}
public class LoginResult
{
    public bool Successful { get; set; }
    public string? Error { get; set; }
    public string? Token { get; set; }
}
