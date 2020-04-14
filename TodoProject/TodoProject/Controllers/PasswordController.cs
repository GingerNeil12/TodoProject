using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoProject.General;
using TodoProject.Interfaces.General;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Security;

namespace TodoProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.USER)]
    public class PasswordController : ApplicationController
    {
        private readonly ICommand<ChangePasswordModel> _changePasswordCommand;
        private readonly ICommand<ResetPasswordRequestModel> _resetPasswordRequestCommand;
        private readonly ICommand<ResetPasswordModel> _resetPasswordCommand;

        public PasswordController(ICommand<ChangePasswordModel> changePasswordCommand,
            ICommand<ResetPasswordRequestModel> resetPasswordRequestCommand,
            ICommand<ResetPasswordModel> resetPasswordCommand)
        {
            _changePasswordCommand = changePasswordCommand;
            _resetPasswordRequestCommand = resetPasswordRequestCommand;
            _resetPasswordCommand = resetPasswordCommand;
        }

        [HttpPut]
        [Route("change/{id}")]
        public async Task<IActionResult> ChangePasswordAsync(string id, [FromBody]ChangePasswordModel model)
        {
            if (GetUserId() != model.UserId || GetUserId() != id)
            {
                var forbiddenResult = new ForbiddenResponse();
                return StatusCode(forbiddenResult.Status, forbiddenResult);
            }

            if (ModelState.IsValid)
            {
                var result = await _changePasswordCommand.RunAsync(model);
                return StatusCode(result.Status, result.Payload);
            }
            return new BadRequestObjectResult(new BadRequestResponse(ModelState));
        }

        [HttpPost]
        [Route("reset/request")]
        public async Task<IActionResult> ResetPasswordRequestAsync(ResetPasswordRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _resetPasswordRequestCommand.RunAsync(model);
                return StatusCode(result.Status, result.Payload);
            }
            return new BadRequestObjectResult(new BadRequestResponse(ModelState));
        }

        [HttpPost]
        [Route("reset")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _resetPasswordCommand.RunAsync(model);
                return StatusCode(result.Status, result.Payload);
            }
            return new BadRequestObjectResult(new BadRequestResponse(ModelState));
        }
    }
}
