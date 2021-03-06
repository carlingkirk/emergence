using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Emergence.Data.Identity;
using Emergence.Data.Shared;
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
            public int Id { get; set; }
            [Display(Name = "Display Name")]
            public string DisplayName { get; set; }
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Bio")]
            public string Bio { get; set; }
            [Display(Name = "Profile photo")]
            public IFormFile ProfilePhotoFile { get; set; }
            [Display(Name = "Address Line 1")]
            public string AddressLine1 { get; set; }
            [Display(Name = "Address Line 2")]
            public string AddressLine2 { get; set; }
            [Display(Name = "City")]
            public string City { get; set; }
            [Display(Name = "State/Province")]
            public string StateOrProvince { get; set; }
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }
            [Display(Name = "Country")]
            public string Country { get; set; }
            [Display(Name = "Send me occasional email updates")]
            public bool EmailUpdates { get; set; }
            [Display(Name = "Send me social notifications")]
            public bool SocialUpdates { get; set; }
            [Display(Name = "Profile visibility")]
            public Visibility ProfileVisibility { get; set; }
            [Display(Name = "Specimen visibility")]
            public Visibility InventoryItemVisibility { get; set; }
            [Display(Name = "Plant profile visibility")]
            public Visibility PlantInfoVisibility { get; set; }
            [Display(Name = "Origin visibility")]
            public Visibility OriginVisibility { get; set; }
            [Display(Name = "Activity visibility")]
            public Visibility ActivityVisibility { get; set; }

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var userProfile = await _userService.GetUserAsync(user.Id);

            if (userProfile == null)
            {
                userProfile = new User
                {
                    UserId = user.Id,
                    EmailUpdates = true,
                    SocialUpdates = true,
                    DateCreated = DateTime.UtcNow
                };
                userProfile = await _userService.UpdateUserAsync(userProfile);
            }
            else
            {
                ProfilePhoto = userProfile.Photo;
            }

            Username = userName;

            Input = new InputModel
            {
                Id = userProfile.Id,
                DisplayName = userProfile.DisplayName,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Bio = userProfile.Bio,
                PhoneNumber = phoneNumber,
                AddressLine1 = userProfile.Location?.AddressLine1,
                AddressLine2 = userProfile.Location?.AddressLine2,
                City = userProfile.Location?.City,
                StateOrProvince = userProfile.Location?.StateOrProvince,
                Country = userProfile.Location?.Country,
                PostalCode = userProfile.Location?.PostalCode,
                EmailUpdates = userProfile.EmailUpdates,
                SocialUpdates = userProfile.SocialUpdates,
                ProfileVisibility = userProfile.ProfileVisibility,
                InventoryItemVisibility = userProfile.InventoryItemVisibility,
                PlantInfoVisibility = userProfile.PlantInfoVisibility,
                OriginVisibility = userProfile.OriginVisibility,
                ActivityVisibility = userProfile.ActivityVisibility
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

            // Validate uniqueness on display name
            if (userProfile.DisplayName != Input.DisplayName)
            {
                var existingUser = await _userService.GetUserByNameAsync(Input.DisplayName, userProfile);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Input.DisplayName", "Sorry, that display name is taken. Please choose another display name.");
                    return Page();
                }
            }

            if (userProfile.Location == null)
            {
                userProfile.Location = new Location
                {
                    DateCreated = DateTime.UtcNow
                };
            }

            userProfile.FirstName = Input.FirstName;
            userProfile.LastName = Input.LastName;
            userProfile.Bio = Input.Bio;
            userProfile.DisplayName = Input.DisplayName;
            userProfile.Location.AddressLine1 = Input.AddressLine1;
            userProfile.Location.AddressLine2 = Input.AddressLine2;
            userProfile.Location.City = Input.City;
            userProfile.Location.StateOrProvince = Input.StateOrProvince;
            userProfile.Location.PostalCode = Input.PostalCode;
            userProfile.Location.Country = Input.Country;
            userProfile.EmailUpdates = Input.EmailUpdates;
            userProfile.SocialUpdates = Input.SocialUpdates;
            userProfile.ProfileVisibility = Input.ProfileVisibility;
            userProfile.InventoryItemVisibility = Input.InventoryItemVisibility;
            userProfile.PlantInfoVisibility = Input.PlantInfoVisibility;
            userProfile.OriginVisibility = Input.OriginVisibility;
            userProfile.ActivityVisibility = Input.ActivityVisibility;
            userProfile.DateModified = DateTime.UtcNow;
            userProfile.Location.DateModified = DateTime.UtcNow;

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
                var photoResult = await _photoService.UploadOriginalAsync(Input.ProfilePhotoFile, Data.Shared.PhotoType.User, user.Id, storeLocation: false);
                photoResult.TypeId = userProfile.Id;
                photoResult = await _photoService.AddOrUpdatePhotoAsync(photoResult);

                userProfile.Photo = photoResult;
                userProfile = await _userService.UpdateUserAsync(userProfile);

                ProfilePhoto = userProfile.Photo;
            }
            else
            {
                await _userService.UpdateUserAsync(userProfile);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
