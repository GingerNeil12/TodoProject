using System.Threading.Tasks;

namespace TodoProject.Interfaces.Security
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(string email);
    }
}
