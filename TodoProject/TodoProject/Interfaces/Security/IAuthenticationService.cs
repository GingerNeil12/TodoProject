using System.Threading.Tasks;
using TodoProject.ViewModels.Security;

namespace TodoProject.Interfaces.Security
{
    public enum AuthenticationResult
    {
        OK,
        USER_NOT_FOUND,
        PASSWORD_INCORRECT,
        ACCOUNT_LOCKED,
        CREDENTIALS_MISMATCH,
        EXPIRED_TOKEN
    }

    public interface IAuthenticationService
    {
        Task<AuthenticationResult> AuthenticateAsync(LoginModel model);
        Task<AuthenticationResult> AuthenticateAsync(RefreshTokenModel model);
    }
}
