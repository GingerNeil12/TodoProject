using System.Threading.Tasks;
using TodoProject.Interfaces.General;
using TodoProject.Models.Email;

namespace TodoProject.Services.General
{
    public class SendGridEmailGateway : IEmailGateway
    {
        public async Task<bool> SendEmailAsync(EmailMessage message)
        {
            return true;
        }
    }
}
