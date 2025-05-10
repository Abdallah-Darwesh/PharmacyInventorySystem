using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PharmacyInventorySystem.Models;
using System.Threading.Tasks;

namespace PharmacyInventorySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (await _userManager.IsInRoleAsync(user, "Manager"))
                {
                    return RedirectToAction("Index", "Suppliers", new { area = "Manager" });
                }
                else if (await _userManager.IsInRoleAsync(user, "Pharmacist"))
                {
                    return RedirectToAction("Dashboard", "Home", new { area = "Pharmacist" });
                }
            }
            return View();
        }

        public IActionResult AccountSettings()
        {
            return View("~/Views/Shared/AccountSettings.cshtml");
        }
    }
}
