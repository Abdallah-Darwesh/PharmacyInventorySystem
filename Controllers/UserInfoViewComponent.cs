using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmacyInventorySystem.Models;
using System.Threading.Tasks;

public class UserInfoViewComponent : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UserInfoViewComponent(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (!User.Identity.IsAuthenticated)
            return View(("", ""));
        var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
        var roles = await _userManager.GetRolesAsync(user);
        string role = roles.Count > 0 ? roles[0] : "";
        string fullName = user?.FullName ?? user?.UserName ?? "";
        return View((fullName, role));
    }
} 