using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoProject.Interfaces.Security;
using TodoProject.Models;
using TodoProject.ViewModels.Security;

namespace TodoProject.Services.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDeserializeToken _deserializeToken;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IDeserializeToken deserializeToken,
            ILogger<AuthenticationService> logger)
        {
            _userManager = userManager;
            _deserializeToken = deserializeToken;
            _logger = logger;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(LoginModel model)
        {
            _logger.LogInformation($"Authenticating: {model.Email}");
            
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogError($"User not found: {model.Email}");
                return AuthenticationResult.USER_NOT_FOUND;
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogError($"User locked out: {model.Email}");
                return AuthenticationResult.ACCOUNT_LOCKED;
            }

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordCorrect)
            {
                _logger.LogError($"Password incorrect for: {model.Email}");
                return AuthenticationResult.PASSWORD_INCORRECT;
            }

            _logger.LogInformation($"User Authenticated: {model.Email}");
            return AuthenticationResult.OK;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(RefreshTokenModel model)
        {
            _logger.LogInformation($"Authenticating: {model.Email}");
            var tokenClaims = _deserializeToken.DeserializeToken(model.BearerToken);
            var tokenEmail = GetEmailFromClaims(tokenClaims);

            if(!model.Email.Equals(tokenEmail))
            {
                _logger.LogError($"Credentials do not match: {model.Email} || {tokenEmail}");
                return AuthenticationResult.CREDENTIALS_MISMATCH;
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogError($"User not found: {model.Email}");
                return AuthenticationResult.USER_NOT_FOUND;
            }

            if(await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogError($"User locked out: {model.Email}");
                return AuthenticationResult.ACCOUNT_LOCKED;
            }

            if(user.RefreshTokenExpiry > DateTime.Now)
            {
                _logger.LogError($"Refresh Token Expired for: {model.Email}");
                return AuthenticationResult.EXPIRED_TOKEN;
            }

            if(!user.RefreshToken.Equals(model.RefreshToken))
            {
                _logger.LogError($"Refresh Token mismatch: {model.RefreshToken} || {user.RefreshToken}");
                return AuthenticationResult.CREDENTIALS_MISMATCH;
            }

            _logger.LogInformation($"Authenticated: {model.Email}");
            return AuthenticationResult.OK;
        }

        private string GetEmailFromClaims(IEnumerable<Claim> claims)
        {
            var emailClaim = claims
                .Where(x => x.Type.Equals(ClaimTypes.Email))
                .FirstOrDefault();

            return emailClaim.Value;
        }
    }
}
