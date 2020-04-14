using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Web;
using TodoProject.Interfaces.General;
using TodoProject.Interfaces.Security;
using TodoProject.Models;
using TodoProject.Models.Email;
using TodoProject.ViewModels.Security;

namespace TodoProject.Services.Security
{
    public class PasswordService : IPasswordService
    {
        private const string USERID_PROPERTY = nameof(ChangePasswordModel.UserId);
        private const string CURRENT_PASSWORD_PROPERTY = nameof(ChangePasswordModel.CurrentPassword);
        private const string NEW_PASSWORD_PROPERTY = nameof(ChangePasswordModel.NewPassword);
        private const string CONFIRM_PASSWORD_PROPERTY = nameof(ChangePasswordModel.ConfirmPassword);
        
        private const string EMAIL_PROPERTY = nameof(ResetPasswordModel.Email);
        private const string RESET_TOKEN_PROPERTY = nameof(ResetPasswordModel.ResetToken);

        private const string USER_NOT_FOUND = "User not found";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailGateway _emailGateway;
        private readonly ILogger<PasswordService> _logger;

        public PasswordService(UserManager<ApplicationUser> userManager,
            IEmailGateway emailGateway,
            ILogger<PasswordService> logger)
        {
            _userManager = userManager;
            _emailGateway = emailGateway;
            _logger = logger;
        }

        public async Task<ModelStateDictionary> ChangePasswordAsync(ChangePasswordModel model)
        {
            _logger.LogInformation($"Changing password for: {model.UserId}");

            var modelState = new ModelStateDictionary();

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                _logger.LogError($"User not found: {model.UserId}");
                modelState.AddModelError(USERID_PROPERTY, USER_NOT_FOUND);
                return modelState;
            }

            var passwordChanged = await _userManager.ChangePasswordAsync(
                user, model.CurrentPassword, model.NewPassword);

            if (!passwordChanged.Succeeded)
            {
                foreach (var error in passwordChanged.Errors)
                {
                    switch (error.Code)
                    {
                        case "PasswordMismatch":
                            modelState.AddModelError(CURRENT_PASSWORD_PROPERTY, error.Description);
                            break;
                        case "PasswordTooShort":
                        case "PasswordRequiresNonAlphanumeric":
                        case "PasswordRequiresDigit":
                        case "PasswordRequiresLower":
                        case "PasswordRequiresUpper":
                        default:
                            modelState.AddModelError(NEW_PASSWORD_PROPERTY, error.Description);
                            modelState.AddModelError(CONFIRM_PASSWORD_PROPERTY, error.Description);
                            break;
                    }
                }
                _logger.LogError($"Errors changing password for: {model.UserId}");
                return modelState;
            }

            var sendEmailResult = await _emailGateway.SendEmailAsync(new ChangePasswordMessage(user.Email));

            _logger.LogInformation($"Password changed for: {model.UserId}");
            return modelState;
        }

        public async Task<ModelStateDictionary> ResetPasswordAsync(ResetPasswordModel model)
        {
            _logger.LogInformation($"Resetting Password for: {model.Email}");

            var modelState = new ModelStateDictionary();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogError($"User not found: {model.Email}");
                modelState.AddModelError(EMAIL_PROPERTY, USER_NOT_FOUND);
                return modelState;
            }

            var passwordResetResult = await _userManager.ResetPasswordAsync(
                user, model.ResetToken, model.NewPassword);

            if(!passwordResetResult.Succeeded)
            {
                foreach (var error in passwordResetResult.Errors)
                {
                    switch(error.Code)
                    {
                        case "InvalidToken":
                            modelState.AddModelError(RESET_TOKEN_PROPERTY, error.Description);
                            break;
                        case "PasswordTooShort":
                        case "PasswordRequiresNonAlphanumeric":
                        case "PasswordRequiresDigit":
                        case "PasswordRequiresLower":
                        case "PasswordRequiresUpper":
                        default:
                            modelState.AddModelError(NEW_PASSWORD_PROPERTY, error.Description);
                            modelState.AddModelError(CONFIRM_PASSWORD_PROPERTY, error.Description);
                            break;
                    }
                }
                _logger.LogError($"Errors reseting password for: {model.Email}");
                return modelState;
            }

            var sendEmailResult = await _emailGateway.SendEmailAsync(new ResetPasswordMessage(user.Email));

            _logger.LogInformation($"Password has been reset for: {model.Email}");
            return modelState;
        }

        public async Task ResetPasswordRequestAsync(ResetPasswordRequestModel model)
        {
            _logger.LogInformation($"Reset Password Request for: {model.Email}");
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                resetToken = HttpUtility.UrlEncode(resetToken);
                var resetUrl = $"{model.ResetPasswordURL}?email={user.Email}&resetToken={resetToken}";
                var resetPasswordRequestMessage = new ResetPasswordRequestMessage(user.Email, resetUrl);
                var sendEmailResult = await _emailGateway.SendEmailAsync(resetPasswordRequestMessage);
            }
        }
    }
}
