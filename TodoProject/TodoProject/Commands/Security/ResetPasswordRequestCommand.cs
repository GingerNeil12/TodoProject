using System.Threading.Tasks;
using TodoProject.Interfaces.General;
using TodoProject.Interfaces.Security;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Security;

namespace TodoProject.Commands.Security
{
    public class ResetPasswordRequestCommand : BaseCommand, ICommand<ResetPasswordRequestModel>
    {
        private readonly IPasswordService _passwordService;

        public ResetPasswordRequestCommand(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public async Task<ResponseMessage> RunAsync(ResetPasswordRequestModel model)
        {
            await _passwordService.ResetPasswordRequestAsync(model);
            return OkResponse("An email has ben sent to the email address provided.");
        }
    }
}
