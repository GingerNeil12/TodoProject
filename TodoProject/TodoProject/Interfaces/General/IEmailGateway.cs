using System.Threading.Tasks;
using TodoProject.Models.Email;

namespace TodoProject.Interfaces.General
{
    public interface IEmailGateway
    {
        Task<bool> SendEmailAsync(EmailMessage message);
    }
}
