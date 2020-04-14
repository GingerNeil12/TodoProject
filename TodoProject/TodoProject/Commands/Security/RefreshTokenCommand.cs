using System.Threading.Tasks;
using TodoProject.Interfaces.General;
using TodoProject.Interfaces.Security;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Security;

namespace TodoProject.Commands.Security
{
    public class RefreshTokenCommand : BaseCommand, ICommand<RefreshTokenModel>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommand(IAuthenticationService authenticationService,
            ITokenService tokenService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
        }

        public async Task<ResponseMessage> RunAsync(RefreshTokenModel model)
        {
            var authenticationResult = await _authenticationService.AuthenticateAsync(model);
            switch (authenticationResult)
            {
                case AuthenticationResult.OK:
                    var token = await _tokenService.GenerateTokenAsync(model.Email);
                    return OkResponse(token);

                case AuthenticationResult.ACCOUNT_LOCKED:
                    return UnauthorizedResponse("Account locked. Contact Administrator.");

                case AuthenticationResult.CREDENTIALS_MISMATCH:
                case AuthenticationResult.EXPIRED_TOKEN:
                    return ForbiddenResponse();

                case AuthenticationResult.USER_NOT_FOUND:
                    return NotFoundResponse(model.Email);

                default:
                    return InternalErrorResponse();
            }
        }
    }
}
