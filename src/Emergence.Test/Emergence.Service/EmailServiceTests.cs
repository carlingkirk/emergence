using System.Threading.Tasks;
using Emergence.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using Xunit;

namespace Emergence.Test.Emergence.Service
{
    public class EmailServiceTests
    {
        private readonly Mock<IEmailSender> _mockEmailSender;
        public EmailServiceTests()
        {
            _mockEmailSender = new Mock<IEmailSender>();
            _mockEmailSender.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task TestSendVerificationEmail()
        {
            var emailService = new EmailService(_mockEmailSender.Object);

            await emailService.SendVerificationEmail("me@gmail.com", "https://www.emergence.app/", "https://www.emergence.app/");

            _mockEmailSender.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task TestSendResetPasswordEmail()
        {
            var emailService = new EmailService(_mockEmailSender.Object);

            await emailService.SendResetPasswordEmail("me@gmail.com", "https://www.emergence.app/", "https://www.emergence.app/");

            _mockEmailSender.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
