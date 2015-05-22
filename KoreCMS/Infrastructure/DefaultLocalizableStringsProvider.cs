using System.Collections.Generic;
using Kore.Localization;

namespace KoreCMS.Infrastructure
{
    public class DefaultLocalizableStringsProvider : IDefaultLocalizableStringsProvider
    {
        #region IDefaultLocalizableStringsProvider Members

        public ICollection<Translation> GetTranslations()
        {
            return new[]
            {
                new Translation
                {
                    CultureCode = null,
                    LocalizedStrings = new Dictionary<string, string>
                    {
                        { LocalizableStrings.Account.ConfirmNewPassword, "Confirm New Password" },
                        { LocalizableStrings.Account.ConfirmPassword, "Confirm Password" },
                        { LocalizableStrings.Account.EditMyProfile, "Edit My Profile" },
                        { LocalizableStrings.Account.EditProfile, "Edit Profile" },
                        { LocalizableStrings.Account.Email, "Email" },
                        { LocalizableStrings.Account.LogOut, "Log out" },
                        { LocalizableStrings.Account.MyProfile, "My Profile" },
                        { LocalizableStrings.Account.NewPassword, "New Password" },
                        { LocalizableStrings.Account.OldPassword, "Current Password" },
                        { LocalizableStrings.Account.Password, "Password" },
                        { LocalizableStrings.Account.ProfileForUser, "Profile For '{0}'" },
                        { LocalizableStrings.Account.RememberMe, "Remember Me?" },
                        { LocalizableStrings.Account.Title, "Account" },
                        { LocalizableStrings.Dashboard.Frontend, "Frontend" },
                        { LocalizableStrings.Dashboard.Title, "Dashboard" },
                        { LocalizableStrings.Dashboard.ToggleNavigation, "Toggle Navigation" },
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}