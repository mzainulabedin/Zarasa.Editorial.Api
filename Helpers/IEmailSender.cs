using System.Threading.Tasks;

namespace Zarasa.Editorial.Api.Helper
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}