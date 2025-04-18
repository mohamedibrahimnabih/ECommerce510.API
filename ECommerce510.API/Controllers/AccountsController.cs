using ECommerce510.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace ECommerce510.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Company"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            ApplicationUser applicationUser = registerDTO.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);

            if (result.Succeeded)
            {
                // Success Register
                await _signInManager.SignInAsync(applicationUser, false);

                await _userManager.AddToRoleAsync(applicationUser, "Customer");

                return NoContent();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var appUser = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (appUser != null)
            {
                var result = await _userManager.CheckPasswordAsync(appUser, loginDTO.Password);

                if (result)
                {
                    // Login
                    await _signInManager.SignInAsync(appUser, loginDTO.RememberMe);

                    return NoContent();
                }
                else
                {
                    ModelStateDictionary keyValuePairs = new();
                    keyValuePairs.AddModelError("Error", "Invalid user name or password");
                    return BadRequest(keyValuePairs);
                }
            }

            return NotFound();
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userManager.GetUserAsync(User);

            if(user is not null)
            {

                var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);

                if (result.Succeeded)
                {
                    // Success Register
                    await _signInManager.SignInAsync(user, false);


                    return NoContent();
                }

                return BadRequest(result.Errors);

            }

            return NotFound();
        }
    }
}
