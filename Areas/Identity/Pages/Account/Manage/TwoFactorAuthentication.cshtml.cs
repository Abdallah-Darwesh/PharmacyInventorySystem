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
    public class TwoFactorAuthenticationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<TwoFactorAuthenticationModel> _logger;

        public TwoFactorAuthenticationModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<TwoFactorAuthenticationModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public bool Is2faEnabled { get; set; }
        public int RecoveryCodesLeft { get; set; }
        public string SharedKey { get; set; }
        public string AuthenticatorUri { get; set; }
        public string[] RecoveryCodes { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Verification Code")]
            public string Code { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostEnable2faAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            StatusMessage = "2FA has been enabled.";
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
            RecoveryCodes = recoveryCodes.ToArray();
            StatusMessage = "You have generated new recovery codes.";
            return Page();
        }

        public async Task<IActionResult> OnPostResetAuthenticatorAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            StatusMessage = "Your authenticator app key has been reset. You will need to configure your authenticator app using the new key.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetEnableAuthenticatorAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }
            SharedKey = FormatKey(unformattedKey);
            AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
            return Page();
        }

        public async Task<IActionResult> OnPostEnableAuthenticatorAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (!ModelState.IsValid)
            {
                await OnGetEnableAuthenticatorAsync();
                return Page();
            }
            var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);
            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Input.Code", "Verification code is invalid.");
                await OnGetEnableAuthenticatorAsync();
                return Page();
            }
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            StatusMessage = "Your authenticator app has been verified.";
            return RedirectToPage();
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
    }
} 