using System.Threading.Tasks;
using Emergence.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using Xunit;

namespace Emergence.Test.Emergence.Service
{
    public class EmailServiceTests
    {
        [Fact]
        public async Task SendVerificationEmail()
        {
            var emailSender = new Mock<IEmailSender>();
            emailSender.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var emailService = new EmailService(emailSender.Object);

            await emailService.SendVerificationEmail("me@gmail.com", "https://www.emergence.app/", "https://www.emergence.app/");
        }
    }
}
