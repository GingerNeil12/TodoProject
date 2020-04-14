using System.Threading.Tasks;
using TodoProject.Interfaces.General;
using TodoProject.Interfaces.Security;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Security;

namespace TodoProject.Commands.Security
{
    public class ChangePasswordCommand : BaseCommand, ICommand<ChangePasswordModel>
    {
        private const string USERID_PROPERTY = nameof(ChangePasswordModel.UserId);

        private readonly IPasswordService _passwordService;

        public ChangePasswordCommand(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public async Task<ResponseMessage> RunAsync(ChangePasswordModel model)
        {
            var changePasswordResult = await _passwordService.ChangePasswordAsync(model);
            if (!changePasswordResult.IsValid)
            {
                if (changePasswordResult.ContainsKey(USERID_PROPERTY))
                {
                    return NotFoundResponse(model.UserId);
                }
                return BadRequestResponse(changePasswordResult);
            }

            return OkResponse("Password has been changed.");
        }
    }
}
