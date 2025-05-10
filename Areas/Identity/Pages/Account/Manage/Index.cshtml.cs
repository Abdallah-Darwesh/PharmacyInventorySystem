using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using PharmacyInventorySystem.Models;

namespace PharmacyInventorySystem.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public bool Is2faSetupInProgress { get; set; }

        // Profile
        [BindProperty]
        public ProfileInputModel ProfileInput { get; set; }
        public class ProfileInputModel
        {
            [Display(Name = "Phone number")]
            [Phone]
            public string PhoneNumber { get; set; }
        }

        // Change Password
        [BindProperty]
        public ChangePasswordInputModel ChangePasswordInput { get; set; }
        public class ChangePasswordInputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        // Email
        [BindProperty]
        public EmailInputModel EmailInput { get; set; }
        public class EmailInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        // 2FA
        public bool Is2faEnabled { get; set; }
        public bool IsMachineRemembered { get; set; }
        public int RecoveryCodesLeft { get; set; }
        public string SharedKey { get; set; }
        public string AuthenticatorUri { get; set; }
        [BindProperty]
        public string TwoFactorCode { get; set; }
        [BindProperty]
        public string RecoveryCode { get; set; }

        // Delete Account
        [BindProperty]
        public bool ConfirmDelete { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            ProfileInput = new ProfileInputModel
            {
                PhoneNumber = user.PhoneNumber
            };
            EmailInput = new EmailInputModel
            {
                Email = user.Email
            };
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);

            // 2FA: If setup in progress, generate and expose key/QR
            if (Is2faSetupInProgress)
            {
                var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                if (string.IsNullOrEmpty(unformattedKey))
                {
                    await _userManager.ResetAuthenticatorKeyAsync(user);
                    unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                }
                SharedKey = FormatKey(unformattedKey);
                AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
            }
            return Page();
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }
            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6",
                UrlEncoder.Default.Encode("PharmacyInventorySystem"),
                UrlEncoder.Default.Encode(email),
                unformattedKey);
        }

        public async Task<IActionResult> OnPostProfileAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var phone = await _userManager.GetPhoneNumberAsync(user);
            if (ProfileInput.PhoneNumber != phone)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, ProfileInput.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, ChangePasswordInput.OldPassword, ChangePasswordInput.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your password has been changed.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var email = await _userManager.GetEmailAsync(user);
            if (EmailInput.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, EmailInput.Email);
                if (!setEmailResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set email.";
                    return RedirectToPage();
                }
            }
            StatusMessage = "Your email has been updated.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAccountAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (!ConfirmDelete)
            {
                StatusMessage = "You must confirm account deletion.";
                return RedirectToPage();
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

        // 2FA Management
        public async Task<IActionResult> OnPostEnable2faAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            // Set setup in progress flag
            Is2faSetupInProgress = true;
            StatusMessage = "Scan the QR code and enter the code from your authenticator app to complete setup.";
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDisable2faAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            StatusMessage = "2FA has been disabled.";
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostGenerateRecoveryCodesAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            {
                StatusMessage = "Cannot generate recovery codes because 2FA is not enabled.";
                return RedirectToPage();
            }
            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            StatusMessage = $"You have generated new recovery codes.";
            // You may want to display these codes to the user
            return RedirectToPage();
        }
    }
} 