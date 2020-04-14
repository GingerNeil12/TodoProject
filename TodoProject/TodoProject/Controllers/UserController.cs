using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoProject.General;
using TodoProject.Interfaces.General;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Account;

namespace TodoProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ApplicationController
    {
        private readonly ICommand<RegisterModel> _registerCommand;
        private readonly ICommand<UpdateAccountModel> _updateAccountCommand;

        public UserController(ICommand<RegisterModel> registerCommand,
            ICommand<UpdateAccountModel> updateAccountCommand)
        {
            _registerCommand = registerCommand;
            _updateAccountCommand = updateAccountCommand;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _registerCommand.RunAsync(model);
                return StatusCode(result.Status, result.Payload);
            }
            return new BadRequestObjectResult(new BadRequestResponse(ModelState));
        }

        [HttpPut]
        [Route("update/{id}")]
        [Authorize(Roles = RoleNames.USER)]
        public async Task<IActionResult> UpdateAccountAsync(string id, [FromBody]UpdateAccountModel model)
        {
            if (GetUserId() != id || GetUserId() != model.UserId)
            {
                var forbidden = new ForbiddenResponse();
                return StatusCode(forbidden.Status, forbidden);
            }

            if (!ModelState.IsValid)
            {
                var result = await _updateAccountCommand.RunAsync(model);
                return StatusCode(result.Status, result.Payload);
            }
            return new BadRequestObjectResult(new BadRequestResponse(ModelState));
        }
    }
}
