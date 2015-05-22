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
                        { KoreCmsLocalizableStrings.Blog.ManageBlog, "Manage Blog" },
                        { KoreCmsLocalizableStrings.Blog.Title, "Blog" },
                        { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.EmailAddress, "Email Address" },
                        { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.HtmlTemplate, "HTML Template" },
                        { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.PleaseEnterCaptcha, "Please enter captcha validation field." },
                        { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.PleaseEnterCorrectCaptcha, "Please enter correct captcha validation field." },
                        { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.RedirectUrl, "Redirect URL (After Submit)" },
                        { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.ThankYouMessage, "'Thank You' Message" },
                        { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.UseAjax, "Use Ajax" },
                        { KoreCmsLocalizableStrings.ContentBlocks.HtmlBlock.BodyContent, "Body Content" },
                        { KoreCmsLocalizableStrings.ContentBlocks.ManageContentBlocks, "Manage Content Blocks" },
                        { KoreCmsLocalizableStrings.ContentBlocks.ManageZones, "Manage Zones" },
                        { KoreCmsLocalizableStrings.ContentBlocks.Title, "Content Blocks" },
                        { KoreCmsLocalizableStrings.ContentBlocks.Zones, "Zones" },
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
                        { KoreCmsLocalizableStrings.Media.ManageMedia, "Manage Media" },
                        { KoreCmsLocalizableStrings.Media.Title, "Media" },
                        { KoreCmsLocalizableStrings.Membership.ChangePassword, "Change Password" },
                        { KoreCmsLocalizableStrings.Membership.EditRolePermissions, "Edit Role Permissions" },
                        { KoreCmsLocalizableStrings.Membership.InvalidEmailAddress, "That is not a valid e-mail address. Please check your input and try again." },
                        { KoreCmsLocalizableStrings.Membership.IsLockedOut, "Is Locked Out" },
                        { KoreCmsLocalizableStrings.Membership.Permissions, "Permissions" },
                        { KoreCmsLocalizableStrings.Membership.Roles, "Roles" },
                        { KoreCmsLocalizableStrings.Membership.Title, "Membership" },
                        { KoreCmsLocalizableStrings.Membership.UpdateUserRoles, "Update User Roles" },
                        { KoreCmsLocalizableStrings.Membership.UserEmailAlreadyExists, "A user with that e-mail address already exists. If you are the owner of that e-mail address, please login and try again." },
                        { KoreCmsLocalizableStrings.Membership.Users, "Users" },
                        { KoreCmsLocalizableStrings.Menus.IsExternalUrl, "Is External Url" },
                        { KoreCmsLocalizableStrings.Menus.Items, "Items" },
                        { KoreCmsLocalizableStrings.Menus.ManageMenuItems, "Manage Menu Items" },
                        { KoreCmsLocalizableStrings.Menus.ManageMenus, "Manage Menus" },
                        { KoreCmsLocalizableStrings.Menus.MenuItems, "Menu Items" },
                        { KoreCmsLocalizableStrings.Menus.NewItem, "New Item" },
                        { KoreCmsLocalizableStrings.Menus.Title, "Menus" },
                        { KoreCmsLocalizableStrings.Messages.CircularRelationshipError, "That action would cause a circular relationship!" },
                        { KoreCmsLocalizableStrings.Messages.ConfirmClearLocalizableStrings, "Warning! This will remove all localized strings from the database. Are you sure you want to do this?" },
                        { KoreCmsLocalizableStrings.Messages.GetTranslationError, "There was an error when retrieving the translation." },
                        { KoreCmsLocalizableStrings.Messages.UpdateTranslationError, "There was an error when saving the translation." },
                        { KoreCmsLocalizableStrings.Messages.UpdateTranslationSuccess, "Successfully saved translation." },
                        { KoreCmsLocalizableStrings.Messaging.MessageTemplates, "Message Templates" },
                        { KoreCmsLocalizableStrings.Messaging.QueuedEmails, "Queued Emails" },
                        { KoreCmsLocalizableStrings.Messaging.Title, "Messaging" },
                        { KoreCmsLocalizableStrings.Navigation.CMS, "CMS" },
                        { KoreCmsLocalizableStrings.Newsletters.Subscribers, "Subscribers" },
                        { KoreCmsLocalizableStrings.Newsletters.SuccessfullySignedUp, "You have successfully signed up for newsletters." },
                        { KoreCmsLocalizableStrings.Newsletters.Title, "Newsletters" },
                        { KoreCmsLocalizableStrings.Pages.ConfirmRestoreVersion, "Are you sure you want to restore this version?" },
                        { KoreCmsLocalizableStrings.Pages.History, "History" },
                        { KoreCmsLocalizableStrings.Pages.ManagePages, "Manage Pages" },
                        { KoreCmsLocalizableStrings.Pages.PageHistory, "Page History" },
                        { KoreCmsLocalizableStrings.Pages.PageHistoryRestoreError, "There was an error when trying to restore the specified page version." },
                        { KoreCmsLocalizableStrings.Pages.PageHistoryRestoreSuccess, "Successfully restored specified page version." },
                        { KoreCmsLocalizableStrings.Pages.PageTypes, "Page Types" },
                        { KoreCmsLocalizableStrings.Pages.RestoreVersion, "Restore Version" },
                        { KoreCmsLocalizableStrings.Pages.Tags, "Tags" },
                        { KoreCmsLocalizableStrings.Pages.Title, "Pages" },
                        { KoreCmsLocalizableStrings.Pages.Translations, "Translations" },
                        { KoreCmsLocalizableStrings.Settings.Blog.AccessRestrictions, "Access Restrictions" },
                        { KoreCmsLocalizableStrings.Settings.Blog.DateFormat, "Date Format" },
                        { KoreCmsLocalizableStrings.Settings.Blog.ItemsPerPage, "# Items Per Page" },
                        { KoreCmsLocalizableStrings.Settings.Blog.MenuPosition, "Menu Position" },
                        { KoreCmsLocalizableStrings.Settings.Blog.PageTitle, "Page Title" },
                        { KoreCmsLocalizableStrings.Settings.Blog.ShowOnMenus, "Show on Menus" },
                        { KoreCmsLocalizableStrings.Settings.Blog.UseAjax, "Use Ajax" },
                        { KoreCmsLocalizableStrings.Settings.Pages.NumberOfPageVersionsToKeep, "# Page Versions to Keep" },
                        { KoreCmsLocalizableStrings.UserProfile.Newsletter.SubscribeToNewsletters, "Subscribe to Newsletters" },
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}