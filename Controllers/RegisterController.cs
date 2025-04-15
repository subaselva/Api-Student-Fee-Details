using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;

    public RegisterController(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var newUser = new IdentityUser { UserName = model.Email, Email = model.Email };

        var result = await userManager.CreateAsync(newUser, model.Password!);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description);

            return Ok(new RegisterResult { Successful = false, Errors = errors });

        }



        return Ok(new RegisterResult { Successful = true });
    }
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
    public class RegisterResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }

}