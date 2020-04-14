using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TodoProject.General;
using TodoProject.Interfaces.Account;
using TodoProject.Interfaces.General;
using TodoProject.Models;
using TodoProject.Models.Email;
using TodoProject.ViewModels.Account;

namespace TodoProject.Services.Account
{
    public class UserService : IUserService
    {
        private const string EMAIL_PROPERTY = nameof(RegisterModel.Email);
        private const string PASSWORD_PROPERTY = nameof(RegisterModel.Password);
        private const string CONFIRM_PASSWORD_PROPERTY = nameof(RegisterModel.ConfirmPassword);
        private const string USERID_PROPERTY = nameof(UpdateAccountModel.UserId);
        private const string FIRSTNAME_PROPERTY = nameof(UpdateAccountModel.FirstName);
        private const string SURNAME_PROPERTY = nameof(UpdateAccountModel.Surname);

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailGateway _emailGateway;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<ApplicationUser> userManager,
            IEmailGateway emailGateway,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _emailGateway = emailGateway;
            _logger = logger;
        }

        public async Task<ModelStateDictionary> RegisterAccountAsync(RegisterModel model)
        {
            _logger.LogInformation($"Registering account for: {model.Email}");

            var modelState = new ModelStateDictionary();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                _logger.LogError($"Error creating account for: {model.Email}");
                modelState.AddModelError(EMAIL_PROPERTY, "Account with that Email already exists.");
                return modelState;
            }

            model.FirstName = CapitaliseFirstLetter(model.FirstName);
            model.Surname = CapitaliseFirstLetter(model.Surname);

            user = new ApplicationUser()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                Surname = model.Surname,
                UserName = $"{model.FirstName[0]}{model.Surname}"
            };

            var accountCreated = await _userManager.CreateAsync(user, model.Password);

            if (!accountCreated.Succeeded)
            {
                foreach (var error in accountCreated.Errors)
                {
                    switch (error.Code)
                    {
                        case "InvalidEmail":
                        case "DuplicateEmail":
                            modelState.AddModelError(EMAIL_PROPERTY, error.Description);
                            break;
                        case "PasswordTooShort":
                        case "PasswordRequiresNonAlphanumeric":
                        case "PasswordRequiresDigit":
                        case "PasswordRequiresLower":
                        case "PasswordRequiresUpper":
                        default:
                            modelState.AddModelError(PASSWORD_PROPERTY, error.Description);
                            modelState.AddModelError(CONFIRM_PASSWORD_PROPERTY, error.Description);
                            break;
                    }
                }

                _logger.LogError($"Error creating account for: {model.Email}");
                return modelState;
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, RoleNames.USER);
            if(!addToRoleResult.Succeeded)
            {
                _logger.LogError($"Error adding user to role: {model.Email}");
                _userManager.DeleteAsync(user).Wait();
                modelState.AddModelError(EMAIL_PROPERTY, "Error creating account. Try again later.");
                return modelState;
            }

            var sendEmailResult = await _emailGateway.SendEmailAsync(
                new RegisterAccountMessage(user.Email));

            _logger.LogInformation($"Account created for: {model.Email}");
            return modelState;
        }

        public async Task<ModelStateDictionary> UpdateAccountAsync(UpdateAccountModel model)
        {
            _logger.LogInformation($"Updating account for: {model.UserId}");

            var modelState = new ModelStateDictionary();

            var user = await _userManager.FindByIdAsync(model.UserId);
            if(user == null)
            {
                _logger.LogError($"User not found: {model.UserId}");
                modelState.AddModelError(USERID_PROPERTY, "User not found.");
                return modelState;
            }

            model.FirstName = CapitaliseFirstLetter(model.FirstName);
            model.Surname = CapitaliseFirstLetter(model.Surname);

            user.FirstName = model.FirstName;
            user.Surname = model.Surname;

            var accountUpdateResult = await _userManager.UpdateAsync(user);

            if(!accountUpdateResult.Succeeded)
            {
                foreach (var error in accountUpdateResult.Errors)
                {
                    switch(error.Code)
                    {
                        default:
                            modelState.AddModelError(FIRSTNAME_PROPERTY, error.Description);
                            modelState.AddModelError(SURNAME_PROPERTY, error.Description);
                            break;
                    }
                }

                _logger.LogError($"Error updating account for: {model.UserId}");
                return modelState;
            }

            var sendEmailResult = await _emailGateway.SendEmailAsync(
                new UpdateAccountMessage(user.Email));

            _logger.LogInformation($"Account Updated: {model.UserId}");
            return modelState;
        }

        private string CapitaliseFirstLetter(string name)
        {
            return char.ToUpper(name[0]) + name.Substring(1);
        }
    }
}
