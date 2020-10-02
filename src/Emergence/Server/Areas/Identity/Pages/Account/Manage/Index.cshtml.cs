using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Emergence.Data.Identity;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Emergence.Client.Server.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;

        public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService, IPhotoService photoService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _photoService = photoService;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public Photo ProfilePhoto { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Profile photo")]
            public IFormFile ProfilePhotoFile { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var userProfile = await _userService.GetUserAsync(user.Id);

            if (userProfile == null)
            {
                userProfile = new User();
            }
            else
            {
                ProfilePhoto = userProfile.Photo;
            }

            Username = userName;

            Input = new InputModel
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userProfile = await _userService.GetUserAsync(user.Id);
            if (userProfile == null)
            {
                userProfile = new User
                {
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    UserId = new Guid(user.Id),
                    DateCreated = DateTime.UtcNow
                };
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.ProfilePhotoFile != null)
            {
                var photoResult = await _photoService.UploadOriginalAsync(Input.ProfilePhotoFile, Data.Shared.PhotoType.User, user.Id);
                photoResult = await _photoService.AddOrUpdatePhotoAsync(photoResult);

                userProfile.Photo = photoResult;
                userProfile = await _userService.UpdateUserAsync(userProfile);

                ProfilePhoto = userProfile.Photo;
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
