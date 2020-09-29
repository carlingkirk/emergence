using System.Threading.Tasks;

namespace Emergence.Service.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationEmail(string email, string callbackUrl, string contentPath);
        Task SendResetPasswordEmail(string email, string callbackUrl, string contentPath);
    }
}
