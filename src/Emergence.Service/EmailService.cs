using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Emergence.Service
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendVerificationEmail(string email, string callbackUrl, string contentPath)
        {
            var buttonStyle = "color:#fff!important;padding:12px 20px 12px 20px;height:40px;width:160px;background-color:#579B0C;" +
                              "font-size:16px;text-decoration:none;border-radius:4px;";
            var logoUrl = contentPath + "icon-512.png";
            var htmlMessage = $"<div style=\"height:150px;\"><p><a href=\"https://www.emergence.app\"><img src=\"{logoUrl}\" alt=\"Emergence.app\" style=\"width:50px;vertical-align:middle;\"></a>" +
                               "<span style=\"font-size:1.5em;padding:15px;\">Emergence.app</span></p>" +
                              $"Hello,<br><br>Please confirm your Emergence.app account ({email}). " +
                              $"<a style=\"{buttonStyle}\" href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Verify</a>.</div>";
            await _emailSender.SendEmailAsync(
                    email,
                    "Verify your account for Emergence.app",
                    htmlMessage);
        }
    }
}
