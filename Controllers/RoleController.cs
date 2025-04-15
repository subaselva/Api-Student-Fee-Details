using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ILogger<RoleController> _logger;

        public RoleController(AppDbContext context,RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ILogger<RoleController> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        // 🔹 Log role changes for CEO monitoring
        [HttpGet]
        
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost]
       
        public async Task<IActionResult> CreateRole(string name)
        {
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation($"The Role{name} has been added successfully");
                    return Ok(new
                    {
                        result = $"The Role {name} has been added successfully"

                    });
                }
                else
                {
                    _logger.LogInformation($"Failed to create role {name}");
                    return Ok(new
                    {
                        error = $"Failed to create role {name}"

                    });

                }

            }
            return BadRequest(new { error = "Role already exist" });
        }
        [HttpGet]
        [Route("GetAllUsers")]
        
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);

        }

        [HttpPost]
        [Route("AddUserToRole")]
        
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation($"User does not exist {email}");
                return BadRequest(new
                {
                    error = "User does not exist"
                });
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation($"Failed to add role {email}");
                return BadRequest(new
                {
                    error = "Failed to add role"
                });

            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = "Succeesss",

                });


            }
            else
            {
                _logger.LogInformation($"the user does not able to added");
                return BadRequest(new
                {
                    error = "the user does not able to added"
                });

            }
        }

        [HttpGet]
        [Route("GetUserRoles")]
       
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation($"the user will the {email} does not exsit ");
                return BadRequest(new
                {
                    error = "the user does not able to added"
                });

            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost]
        [Route("RemoveUserFromRole")]
       
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation($"User does not exist {email}");
                return BadRequest(new
                {
                    error = "User does not exist"
                });
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation($"Failed to add role {email}");
                return BadRequest(new
                {
                    error = "Failed to add role"
                });

            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"User{email} has been removed from role {roleName}"
                });
            }

            return BadRequest(new
            {
                error = $"Unable to user{email} remove the role{roleName}"
            });

        }
    }
}
