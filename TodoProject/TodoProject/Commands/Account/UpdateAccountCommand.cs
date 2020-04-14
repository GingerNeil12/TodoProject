using System.Threading.Tasks;
using TodoProject.Interfaces.Account;
using TodoProject.Interfaces.General;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Account;

namespace TodoProject.Commands.Account
{
    public class UpdateAccountCommand : BaseCommand, ICommand<UpdateAccountModel>
    {
        private readonly IUserService _userService;

        public UpdateAccountCommand(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResponseMessage> RunAsync(UpdateAccountModel model)
        {
            var accountUpdatedResult = await _userService.UpdateAccountAsync(model);
            if (!accountUpdatedResult.IsValid)
            {
                return BadRequestResponse(accountUpdatedResult);
            }
            return OkResponse(model.UserId);
        }
    }
}
