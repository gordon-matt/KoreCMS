using System.Collections.Generic;
using Kore.Localization;

namespace KoreCMS.Infrastructure
{
    public class LanguagePackInvariant : ILanguagePack
    {
        #region ILanguagePack Members

        public string CultureCode
        {
            get { return null; }
        }

        public IDictionary<string, string> LocalizedStrings
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { LocalizableStrings.Account.ConfirmNewPassword, "Confirm New Password" },
                    { LocalizableStrings.Account.ConfirmPassword, "Confirm Password" },
                    { LocalizableStrings.Account.EditMyProfile, "Edit My Profile" },
                    { LocalizableStrings.Account.EditProfile, "Edit Profile" },
                    { LocalizableStrings.Account.Email, "Email" },
                    { LocalizableStrings.Account.InvalidUserNameOrPassword, "Invalid username or password." },
                    { LocalizableStrings.Account.LogOut, "Log out" },
                    { LocalizableStrings.Account.ManageMessages.ChangePasswordSuccess, "Your password has been changed." },
                    { LocalizableStrings.Account.ManageMessages.Error, "An error has occurred." },
                    { LocalizableStrings.Account.ManageMessages.RemoveLoginSuccess, "The external login was removed." },
                    { LocalizableStrings.Account.ManageMessages.SetPasswordSuccess, "Your password has been set." },
                    { LocalizableStrings.Account.MyProfile, "My Profile" },
                    { LocalizableStrings.Account.NewPassword, "New Password" },
                    { LocalizableStrings.Account.NoUserFound, "No user found." },
                    { LocalizableStrings.Account.OldPassword, "Current Password" },
                    { LocalizableStrings.Account.Password, "Password" },
                    { LocalizableStrings.Account.ProfileForUser, "Profile For '{0}'" },
                    { LocalizableStrings.Account.RememberMe, "Remember Me?" },
                    { LocalizableStrings.Account.Title, "Account" },
                    { LocalizableStrings.Account.UserDoesNotExistOrIsNotConfirmed, "The user either does not exist or is not confirmed." },
                    { LocalizableStrings.Dashboard.Frontend, "Frontend" },
                    { LocalizableStrings.Dashboard.Title, "Dashboard" },
                    { LocalizableStrings.Dashboard.ToggleNavigation, "Toggle Navigation" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}