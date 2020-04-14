using System.Collections.Generic;
using System.Security.Claims;

namespace TodoProject.Interfaces.Security
{
    public interface IDeserializeToken
    {
        IEnumerable<Claim> DeserializeToken(string bearerToken);
    }
}
