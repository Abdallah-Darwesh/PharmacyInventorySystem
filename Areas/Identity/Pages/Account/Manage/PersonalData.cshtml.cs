using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PharmacyInventorySystem.Models;

namespace PharmacyInventorySystem.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PersonalDataModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnPostDownloadAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var personalData = new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.FullName,
                user.UserType
            };
            var json = JsonSerializer.Serialize(personalData);
            var bytes = Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "PersonalData.json");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to delete account.";
                return RedirectToPage();
            }
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
} 