using Api.Models;
using Api.Services;
using Api.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService _jwtService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(JWTService jwtService,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpGet("Refresh-Token")]
        [Authorize]
        public async Task<ActionResult<UserView>> RefreshUserToken()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return CreateApplicationUserDto(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserView>> Login(LoginView model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null) return Unauthorized("Invalid Username or Password");

            if (user.EmailConfirmed == false) return Unauthorized("Please confirm your Email!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid Username or Password");

            return CreateApplicationUserDto(user);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterView model)
        {
            if (await CheckEmailExistsAsync(model.Email))
            {
                return BadRequest($"An Existing account is using {model.Email} email address. Please try with another email");
            }

            var userToAdd = new User
            {
                FirstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                Email = model.Email.ToLower(),
                UserName = model.Email.ToLower(),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(userToAdd, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Your account has been created, Please Login");

        }

        #region Private Helper Methods

        private UserView CreateApplicationUserDto(User user)
        {
            return new UserView
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = _jwtService.CreateJWT(user),
            };
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x=> x.Email == email.ToLower()); 
        }
        #endregion
    }
}
