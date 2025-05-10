using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PharmacyInventorySystem.Models;
using System.Threading.Tasks;

namespace PharmacyInventorySystem.Areas.Identity.Pages.Account
{
    public class AdminApproveModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminApproveModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string actionType)
        {
            if (userId == null || actionType == null)
                return RedirectToPage("/Index");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                Message = "User not found.";
                return Page();
            }

            if (actionType == "approve")
            {
                user.IsApproved = true;
                await _userManager.UpdateAsync(user);
                Message = $"✅ User {user.Email} has been approved.";
            }
            else if (actionType == "reject")
            {
                await _userManager.DeleteAsync(user);
                Message = $"❌ User {user.Email} has been rejected and deleted.";
            }
            else
            {
                Message = "Invalid action.";
            }

            return Page();
        }
    }
}
