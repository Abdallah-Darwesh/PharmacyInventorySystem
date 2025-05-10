using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using PharmacyInventorySystem.Models;

namespace PharmacyInventorySystem.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
                return RedirectToPage("/Index");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Unable to load user with ID '{userId}'.");

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            if (result.Succeeded)
            {
                // إرسال إيميل للإدمن بعد تأكيد الإيميل
                var adminEmail = "abdallahemad080@gmail.com";
                var approveUrl = Url.Page(
                    "/Account/AdminApprove",
                    pageHandler: null,
                    values: new { userId = user.Id, actionType = "approve" },
                    protocol: Request.Scheme);

                var rejectUrl = Url.Page(
                    "/Account/AdminApprove",
                    pageHandler: null,
                    values: new { userId = user.Id, actionType = "reject" },
                    protocol: Request.Scheme);

                var message = $@"
                    <p>المستخدم <strong>{user.Email}</strong> قام بتأكيد بريده الإلكتروني.</p>
                    <p>هل تريد الموافقة على المستخدم؟</p>
                    <a href='{approveUrl}'>✅ الموافقة</a><br/>
                    <a href='{rejectUrl}'>❌ الرفض</a>";

                await _emailSender.SendEmailAsync(adminEmail, "موافقة على مستخدم جديد", message);
            }

            return Page();
        }
    }
}
