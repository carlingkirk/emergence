using System.Text;
using System.Threading.Tasks;
using Emergence.Client.Server.Extensions;
using Emergence.Data.Identity;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Emergence.Client.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public string Email { get; set; }
        public bool DisplayConfirmAccountLink { get; set; }
        public string EmailConfirmationUrl { get; set; }


        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("./Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string email, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return RedirectToPage("./Login");
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code },
                    protocol: Request.Scheme);

                var contentPath = Request.GetContentUrl(Url.Content("~/"));
                await _emailService.SendVerificationEmail(user.Email, callbackUrl, contentPath);
                Message = "We've resent your confirmation email. Please confirm your email and login.";

                return RedirectToPage("./RegisterConfirmation", new { email = user.Email, returnUrl });

            }
            return RedirectToPage("./Login");
        }
    }
}
