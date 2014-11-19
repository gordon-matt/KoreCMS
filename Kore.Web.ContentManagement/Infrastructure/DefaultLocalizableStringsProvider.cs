using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Web.ContentManagement.Infrastructure
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
                        //{ LocalizableStrings.Lists.Title, "Lists" },
                        { KoreCmsLocalizableStrings.Localization.IsRTL, "Is Right-to-Left" },
                        { KoreCmsLocalizableStrings.Localization.Languages, "Languages" },
                        { KoreCmsLocalizableStrings.Localization.LocalizableStrings, "Localizable Strings" },
                        { KoreCmsLocalizableStrings.Localization.Localize, "Localize" },
                        { KoreCmsLocalizableStrings.Localization.ManageLanguages, "Manage Languages" },
                        { KoreCmsLocalizableStrings.Localization.ManageLocalizableStrings, "Manage Localizable Strings" },
                        { KoreCmsLocalizableStrings.Localization.SelectLanguage, "Select Language" },
                        { KoreCmsLocalizableStrings.Localization.Title, "Localization" },
                        { KoreCmsLocalizableStrings.Localization.Translate, "Translate" },
                        { KoreCmsLocalizableStrings.Localization.Translations, "Translations" },
                        { KoreCmsLocalizableStrings.Media.Title, "Media" },
                        { KoreCmsLocalizableStrings.Menus.IsExternalUrl, "Is External Url" },
                        { KoreCmsLocalizableStrings.Menus.Items, "Items" },
                        { KoreCmsLocalizableStrings.Menus.ManageMenuItems, "Manage Menu Items" },
                        { KoreCmsLocalizableStrings.Menus.ManageMenus, "Manage Menus" },
                        { KoreCmsLocalizableStrings.Menus.MenuItems, "Menu Items" },
                        { KoreCmsLocalizableStrings.Menus.NewItem, "New Item" },
                        { KoreCmsLocalizableStrings.Menus.Title, "Menus" },
                        { KoreCmsLocalizableStrings.Membership.ChangePassword, "Change Password" },
                        { KoreCmsLocalizableStrings.Membership.EditRolePermissions, "Edit Role Permissions" },
                        { KoreCmsLocalizableStrings.Membership.IsLockedOut, "Is Locked Out" },
                        { KoreCmsLocalizableStrings.Membership.Permissions, "Permissions" },
                        { KoreCmsLocalizableStrings.Membership.Roles, "Roles" },
                        { KoreCmsLocalizableStrings.Membership.Title, "Membership" },
                        { KoreCmsLocalizableStrings.Membership.UpdateUserRoles, "Update User Roles" },
                        { KoreCmsLocalizableStrings.Membership.Users, "Users" },
                        { KoreCmsLocalizableStrings.Messages.ConfirmClearLocalizableStrings, "Warning! This will remove all localized strings from the database. Are you sure you want to do this?" },
                        { KoreCmsLocalizableStrings.Messages.GetTranslationError, "There was an error when retrieving the translation." },
                        { KoreCmsLocalizableStrings.Messages.UpdateTranslationError, "There was an error when saving the translation." },
                        { KoreCmsLocalizableStrings.Messages.UpdateTranslationSuccess, "Successfully saved translation." },
                        { KoreCmsLocalizableStrings.Messaging.MessageTemplates, "Message Templates" },
                        { KoreCmsLocalizableStrings.Messaging.QueuedEmails, "Queued Emails" },
                        { KoreCmsLocalizableStrings.Messaging.Title, "Messaging" },
                        { KoreCmsLocalizableStrings.Navigation.CMS, "CMS" },
                        { KoreCmsLocalizableStrings.Navigation.Home, "Home" },
                        { KoreCmsLocalizableStrings.Pages.ConfirmRestoreVersion, "Are you sure you want to restore this version?" },
                        { KoreCmsLocalizableStrings.Pages.History, "History" },
                        { KoreCmsLocalizableStrings.Pages.ManagePages, "Manage Pages" },
                        { KoreCmsLocalizableStrings.Pages.PageHistory, "Page History" },
                        { KoreCmsLocalizableStrings.Pages.RestoreVersion, "Restore Version" },
                        { KoreCmsLocalizableStrings.Pages.PageHistoryRestoreError, "There was an error when trying to restore the specified page version." },
                        { KoreCmsLocalizableStrings.Pages.PageHistoryRestoreSuccess, "Successfully restored specified page version." },
                        { KoreCmsLocalizableStrings.Pages.Tags, "Tags" },
                        { KoreCmsLocalizableStrings.Pages.Title, "Pages" },
                        { KoreCmsLocalizableStrings.Pages.Translations, "Translations" },
                        { KoreCmsLocalizableStrings.Widgets.ManageWidgets, "Manage Widgets" },
                        { KoreCmsLocalizableStrings.Widgets.ManageZones, "Manage Zones" },
                        { KoreCmsLocalizableStrings.Widgets.Title, "Widgets" },
                        { KoreCmsLocalizableStrings.Widgets.Zones, "Zones" },
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}