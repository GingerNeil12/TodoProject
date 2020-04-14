using System.Threading.Tasks;
using TodoProject.Interfaces.Account;
using TodoProject.Interfaces.General;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Account;

namespace TodoProject.Commands.Account
{
    public class RegisterCommand : BaseCommand, ICommand<RegisterModel>
    {
        private readonly IUserService _userService;

        public RegisterCommand(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResponseMessage> RunAsync(RegisterModel model)
        {
            var registerResult = await _userService.RegisterAccountAsync(model);

            if (!registerResult.IsValid)
            {
                return BadRequestResponse(registerResult);
            }

            return CreatedResponse(model.Email);
        }
    }
}
