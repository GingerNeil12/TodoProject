using System.Threading.Tasks;
using TodoProject.Interfaces.General;
using TodoProject.Interfaces.Security;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Security;

namespace TodoProject.Commands.Security
{
    public class ResetPasswordCommand : BaseCommand, ICommand<ResetPasswordModel>
    {
        private const string EMAIL_PROPERTY = nameof(ResetPasswordModel.Email);
        private const string RESET_TOKEN_PROPERTY = nameof(ResetPasswordModel.ResetToken);

        private readonly IPasswordService _passwordService;

        public ResetPasswordCommand(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public async Task<ResponseMessage> RunAsync(ResetPasswordModel model)
        {
            var resetResult = await _passwordService.ResetPasswordAsync(model);

            if (!resetResult.IsValid)
            {
                if (resetResult.ContainsKey(EMAIL_PROPERTY))
                {
                    return NotFoundResponse(model.Email);
                }

                if (resetResult.ContainsKey(RESET_TOKEN_PROPERTY))
                {
                    return ForbiddenResponse();
                }

                return BadRequestResponse(resetResult);
            }

            return OkResponse("Password has been reset.");
        }
    }
}
