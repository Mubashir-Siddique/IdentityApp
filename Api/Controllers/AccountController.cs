using Api.Models;
using Api.Services;
using Api.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        #endregion
    }
}
