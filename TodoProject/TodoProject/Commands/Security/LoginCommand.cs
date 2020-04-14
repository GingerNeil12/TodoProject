using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using TodoProject.Interfaces.General;
using TodoProject.Interfaces.Security;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Security;

namespace TodoProject.Commands.Security
{
    public class LoginCommand : BaseCommand, ICommand<LoginModel>
    {
        private const string EMAIL_PROPERTY = nameof(LoginModel.Email);
        private const string PASSWORD_PROPERTY = nameof(LoginModel.Password);

        private const string EMAIL_PASSWORD_INCORRECT = "Email or Password is incorrect.";
        private const string ACCOUNT_LOCKED = "Account locked. Contact Administrator";

        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;

        public LoginCommand(IAuthenticationService authenticationService,
            ITokenService tokenService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
        }

        public async Task<ResponseMessage> RunAsync(LoginModel model)
        {
            var authenticationResult = await _authenticationService.AuthenticateAsync(model);
            switch (authenticationResult)
            {
                case AuthenticationResult.OK:
                    var token = await _tokenService.GenerateTokenAsync(model.Email);
                    return OkResponse(token);

                case AuthenticationResult.USER_NOT_FOUND:
                case AuthenticationResult.PASSWORD_INCORRECT:
                    var errors = CreateModelErrors(EMAIL_PASSWORD_INCORRECT);
                    return BadRequestResponse(errors);

                case AuthenticationResult.ACCOUNT_LOCKED:
                    return UnauthorizedResponse(ACCOUNT_LOCKED);

                default:
                    return InternalErrorResponse();
            }
        }

        private ModelStateDictionary CreateModelErrors(string errorMessage)
        {
            var result = new ModelStateDictionary();
            result.AddModelError(EMAIL_PROPERTY, errorMessage);
            result.AddModelError(PASSWORD_PROPERTY, errorMessage);
            return result;
        }
    }
}
