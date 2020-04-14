using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TodoProject.Interfaces.Security;
using TodoProject.Models;

namespace TodoProject.Services.Security
{
    public class TokenService : ITokenService, IDeserializeToken
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;

        public TokenService(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<TokenService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public IEnumerable<Claim> DeserializeToken(string bearerToken)
        {
            var result = new JwtSecurityToken(bearerToken);
            return result.Claims;
        }

        public async Task<string> GenerateTokenAsync(string email)
        {
            _logger.LogInformation($"Generating token for: {email}");

            var user = await _userManager.FindByEmailAsync(email);
            AddRefreshTokenToUser(user);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("RefreshToken", user.RefreshToken)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var jwtHeader = new JwtHeader(credentials);
            var jwtPayload = new JwtPayload(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(30),
                issuedAt: DateTime.Now);

            var token = new JwtSecurityToken(jwtHeader, jwtPayload);

            _logger.LogInformation($"Token Generated for: {email}");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void AddRefreshTokenToUser(ApplicationUser user)
        {
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.Now.AddHours(1);
            _userManager.UpdateAsync(user).Wait();
        }

        private string GenerateRefreshToken()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var result = new byte[32];
                rng.GetBytes(result);
                return Convert.ToBase64String(result);
            }
        }
    }
}
