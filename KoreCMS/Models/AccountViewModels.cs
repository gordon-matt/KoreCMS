using System.ComponentModel.DataAnnotations;
using Kore.ComponentModel;

namespace KoreCMS.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [LocalizedDisplayName(LocalizableStrings.Account.Email)]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [LocalizedDisplayName(LocalizableStrings.Account.OldPassword)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [LocalizedDisplayName(LocalizableStrings.Account.NewPassword)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [LocalizedDisplayName(LocalizableStrings.Account.ConfirmNewPassword)]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [LocalizedDisplayName(LocalizableStrings.Account.Email)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [LocalizedDisplayName(LocalizableStrings.Account.Password)]
        public string Password { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Account.RememberMe)]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [LocalizedDisplayName(LocalizableStrings.Account.Email)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [LocalizedDisplayName(LocalizableStrings.Account.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [LocalizedDisplayName(LocalizableStrings.Account.ConfirmPassword)]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [LocalizedDisplayName(LocalizableStrings.Account.Email)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [LocalizedDisplayName(LocalizableStrings.Account.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [LocalizedDisplayName(LocalizableStrings.Account.ConfirmPassword)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [LocalizedDisplayName(LocalizableStrings.Account.Email)]
        public string Email { get; set; }
    }
}