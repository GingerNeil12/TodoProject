using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TodoProject.General;

namespace TodoProject.Controllers
{
    [Authorize(AuthenticationSchemes = AuthScheme.BEARER)]
    public class ApplicationController : ControllerBase
    {
        protected string GetUserId()
        {
            var claims = GetUserClaims();
            var userIdClaim = claims
                .Where(x => x.Type.Equals(ClaimTypes.NameIdentifier))
                .FirstOrDefault();
            return userIdClaim.Value;
        }

        private IEnumerable<Claim> GetUserClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return identity.Claims;
        }
    }
}
