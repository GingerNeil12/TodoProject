using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoProject.Interfaces.General;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Security;

namespace TodoProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ApplicationController
    {
        private readonly ICommand<LoginModel> _loginCommand;
        private readonly ICommand<RefreshTokenModel> _refreshTokenModel;

        public AuthenticationController(ICommand<LoginModel> loginCommand,
            ICommand<RefreshTokenModel> refreshTokenModel)
        {
            _loginCommand = loginCommand;
            _refreshTokenModel = refreshTokenModel;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _loginCommand.RunAsync(model);
                return StatusCode(result.Status, result.Payload);
            }
            return new BadRequestObjectResult(new BadRequestResponse(ModelState));
        }

        [HttpPost]
        [Route("token/refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _refreshTokenModel.RunAsync(model);
                return StatusCode(result.Status, result.Payload);
            }
            return new BadRequestObjectResult(new BadRequestResponse(ModelState));
        }
    }
}
