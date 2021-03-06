﻿using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Web.ContentManagement.Infrastructure
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
                    { KoreCmsLocalizableStrings.Blog.Categories, "Categories" },
                    { KoreCmsLocalizableStrings.Blog.ManageBlog, "Manage Blog" },
                    { KoreCmsLocalizableStrings.Blog.CategoryModel.Name, "Name" },
                    { KoreCmsLocalizableStrings.Blog.CategoryModel.UrlSlug, "URL Slug" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.CategoryId, "Category" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.DateCreatedUtc, "Date Created (UTC)" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.ExternalLink, "External Link" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.FullDescription, "Full Description" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.Headline, "Headline" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.MetaDescription, "Meta Description" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.MetaKeywords, "Meta Keywords" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.ShortDescription, "Short Description" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.Slug, "URL Slug" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.TeaserImageUrl, "Teaser Image URL" },
                    { KoreCmsLocalizableStrings.Blog.PostModel.UseExternalLink, "Use External Link" },
                    { KoreCmsLocalizableStrings.Blog.TagModel.Name, "Name" },
                    { KoreCmsLocalizableStrings.Blog.TagModel.UrlSlug, "URL Slug" },
                    { KoreCmsLocalizableStrings.Blog.PostedByXOnX, "Posted by {0} on {1}." },
                    { KoreCmsLocalizableStrings.Blog.Posts, "Posts" },
                    { KoreCmsLocalizableStrings.Blog.Tags, "Tags" },
                    { KoreCmsLocalizableStrings.Blog.Title, "Blog" },
                    { KoreCmsLocalizableStrings.ContentBlocks.AllPostsBlock.CategoryId, "Category" },
                    { KoreCmsLocalizableStrings.ContentBlocks.AllPostsBlock.FilterType, "Filter Type" },
                    { KoreCmsLocalizableStrings.ContentBlocks.AllPostsBlock.TagId, "Tag" },
                    { KoreCmsLocalizableStrings.ContentBlocks.CategoriesBlock.NumberOfCategories, "Number of Categories" },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.EmailAddress, "Email Address" },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.FormUrl, "Form URL" },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.HtmlTemplate, "HTML Template" },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.PleaseEnterCaptcha, "Please enter captcha validation field." },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.PleaseEnterCorrectCaptcha, "Please enter correct captcha validation field." },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.RedirectUrl, "Redirect URL (After Submit)" },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.SaveResultIfNotRedirectPleaseClick, "If this page does not automatically redirect, please click the following link to continue:" },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.SaveResultRedirect, "Redirect" },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.ThankYouMessage, "'Thank You' Message" },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.UseAjax, "Use Ajax" },
                    { KoreCmsLocalizableStrings.ContentBlocks.HtmlBlock.BodyContent, "Body Content" },
                    { KoreCmsLocalizableStrings.ContentBlocks.HtmlBlock.HelpText.BodyContent, "The HTML content to display." },
                    { KoreCmsLocalizableStrings.ContentBlocks.HtmlBlock.HelpText.Script, "Optional JavaScript to add to the page." },
                    { KoreCmsLocalizableStrings.ContentBlocks.HtmlBlock.Script, "Script" },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.HelpText.EmailAddress, "The email address to send the form values to." },
                    { KoreCmsLocalizableStrings.ContentBlocks.FormBlock.HelpText.FormUrl, "Specify URL to send the form values to or leave blank for default behavior (form values are emailed to the specified address)." },
                    { KoreCmsLocalizableStrings.ContentBlocks.LanguageSwitchBlock.CustomTemplatePath, "Custom Template Path" },
                    { KoreCmsLocalizableStrings.ContentBlocks.LanguageSwitchBlock.IncludeInvariant, "Include Invariant" },
                    { KoreCmsLocalizableStrings.ContentBlocks.LanguageSwitchBlock.InvariantText, "Invariant Text" },
                    { KoreCmsLocalizableStrings.ContentBlocks.LanguageSwitchBlock.Style, "Style" },
                    { KoreCmsLocalizableStrings.ContentBlocks.LanguageSwitchBlock.UseUrlPrefix, "Use URL Prefix" },
                    { KoreCmsLocalizableStrings.ContentBlocks.LastNPostsBlock.NumberOfEntries, "# of Entries" },
                    { KoreCmsLocalizableStrings.ContentBlocks.ManageContentBlocks, "Manage Content Blocks" },
                    { KoreCmsLocalizableStrings.ContentBlocks.ManageZones, "Manage Zones" },
                    { KoreCmsLocalizableStrings.ContentBlocks.Model.BlockType, "Block Type" },
                    { KoreCmsLocalizableStrings.ContentBlocks.Model.CustomTemplatePath, "Custom Template Path" },
                    { KoreCmsLocalizableStrings.ContentBlocks.Model.IsEnabled, "Is Enabled" },
                    { KoreCmsLocalizableStrings.ContentBlocks.Model.Order, "Order" },
                    { KoreCmsLocalizableStrings.ContentBlocks.Model.Title, "Title" },
                    { KoreCmsLocalizableStrings.ContentBlocks.Model.ZoneId, "Zone" },
                    { KoreCmsLocalizableStrings.ContentBlocks.NewsletterSubscriptionBlock.Email, "Email" },
                    { KoreCmsLocalizableStrings.ContentBlocks.NewsletterSubscriptionBlock.EmailPlaceholder, "Your E-Mail Address" },
                    { KoreCmsLocalizableStrings.ContentBlocks.NewsletterSubscriptionBlock.Name, "Name" },
                    { KoreCmsLocalizableStrings.ContentBlocks.NewsletterSubscriptionBlock.NamePlaceholder, "Your Name" },
                    { KoreCmsLocalizableStrings.ContentBlocks.NewsletterSubscriptionBlock.SignUpLabel, "Sign up for newsletters" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.AfterCloudRender, "After Cloud Render" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.AutoResize, "Auto Resize" },
                    //{ KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.CenterX, "Center X" },
                    //{ KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.CenterY, "Center Y" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.ClassPattern, "Class Pattern" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.Colors, "Colors" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.Delay, "Delay" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.EncodeURI, "Encode URI" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.FontSizeFrom, "Font Size From" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.FontSizeTo, "Font Size To" },
                    //{ KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.Height, "Height" },
                    //{ KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.HeightUnit, "Height Unit" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.RemoveOverflowing, "Remove Overflowing" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.Shape, "Shape" },
                    { KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.Steps, "Steps" },
                    //{ KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.Width, "Width" },
                    //{ KoreCmsLocalizableStrings.ContentBlocks.TagCloudBlock.WidthUnit, "Width Unit" },
                    { KoreCmsLocalizableStrings.ContentBlocks.Title, "Content Blocks" },
                    { KoreCmsLocalizableStrings.ContentBlocks.VideoBlock.AutoPlay, "Auto Play" },
                    { KoreCmsLocalizableStrings.ContentBlocks.VideoBlock.ControlId, "Control ID" },
                    { KoreCmsLocalizableStrings.ContentBlocks.VideoBlock.Loop, "Loop" },
                    { KoreCmsLocalizableStrings.ContentBlocks.VideoBlock.ShowControls, "Show Controls" },
                    { KoreCmsLocalizableStrings.ContentBlocks.VideoBlock.Source, "Source" },
                    { KoreCmsLocalizableStrings.ContentBlocks.VideoBlock.Type, "Type" },
                    { KoreCmsLocalizableStrings.ContentBlocks.VideoBlock.VideoTagNotSupported, "Your browser is too old to support HTML5 Video. Please update your browser." },
                    { KoreCmsLocalizableStrings.ContentBlocks.ZoneModel.Name, "Name" },
                    { KoreCmsLocalizableStrings.ContentBlocks.Zones, "Zones" },
                    { KoreCmsLocalizableStrings.Media.FileBrowserTitle, "File Browser" },
                    { KoreCmsLocalizableStrings.Media.ManageMedia, "Manage Media" },
                    { KoreCmsLocalizableStrings.Media.Title, "Media" },
                    { KoreCmsLocalizableStrings.Menus.IsExternalUrl, "Is External Url" },
                    { KoreCmsLocalizableStrings.Menus.Items, "Items" },
                    { KoreCmsLocalizableStrings.Menus.ManageMenuItems, "Manage Menu Items" },
                    { KoreCmsLocalizableStrings.Menus.ManageMenus, "Manage Menus" },
                    { KoreCmsLocalizableStrings.Menus.MenuItemModel.CssClass, "CSS Class" },
                    { KoreCmsLocalizableStrings.Menus.MenuItemModel.Description, "Description" },
                    { KoreCmsLocalizableStrings.Menus.MenuItemModel.Enabled, "Enabled" },
                    { KoreCmsLocalizableStrings.Menus.MenuItemModel.Position, "Position" },
                    { KoreCmsLocalizableStrings.Menus.MenuItemModel.Text, "Text" },
                    { KoreCmsLocalizableStrings.Menus.MenuItemModel.Url, "Url" },
                    { KoreCmsLocalizableStrings.Menus.MenuItems, "Menu Items" },
                    { KoreCmsLocalizableStrings.Menus.MenuModel.Name, "Name" },
                    { KoreCmsLocalizableStrings.Menus.MenuModel.UrlFilter, "URL Filter" },
                    { KoreCmsLocalizableStrings.Menus.NewItem, "New Item" },
                    { KoreCmsLocalizableStrings.Menus.Title, "Menus" },
                    { KoreCmsLocalizableStrings.Messages.CircularRelationshipError, "That action would cause a circular relationship!" },
                    { KoreCmsLocalizableStrings.Messages.ConfirmClearLocalizableStrings, "Warning! This will remove all localized strings from the database. Are you sure you want to do this?" },
                    { KoreCmsLocalizableStrings.Messages.GetTranslationError, "There was an error when retrieving the translation." },
                    { KoreCmsLocalizableStrings.Messages.UpdateTranslationError, "There was an error when saving the translation." },
                    { KoreCmsLocalizableStrings.Messages.UpdateTranslationSuccess, "Successfully saved translation." },
                    { KoreCmsLocalizableStrings.Messaging.GetTokensError, "Could not get tokens." },
                    { KoreCmsLocalizableStrings.Messaging.MessageTemplateModel.Body, "Body" },
                    { KoreCmsLocalizableStrings.Messaging.MessageTemplateModel.Enabled, "Enabled" },
                    { KoreCmsLocalizableStrings.Messaging.MessageTemplateModel.Name, "Name" },
                    { KoreCmsLocalizableStrings.Messaging.MessageTemplateModel.Subject, "Subject" },
                    { KoreCmsLocalizableStrings.Messaging.MessageTemplateModel.Tokens, "Tokens" },
                    { KoreCmsLocalizableStrings.Messaging.MessageTemplates, "Message Templates" },
                    { KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.CreatedOnUtc, "Created On (UTC)" },
                    { KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.SentOnUtc, "Sent On (UTC)" },
                    { KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.SentTries, "Sent Tries" },
                    { KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.Subject, "Subject" },
                    { KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.ToAddress, "To Address" },
                    { KoreCmsLocalizableStrings.Messaging.QueuedEmails, "Queued Emails" },
                    { KoreCmsLocalizableStrings.Messaging.Title, "Messaging" },
                    { KoreCmsLocalizableStrings.Navigation.CMS, "CMS" },
                    { KoreCmsLocalizableStrings.Newsletters.ExportToCSV, "Export To CSV" },
                    { KoreCmsLocalizableStrings.Newsletters.Subscribers, "Subscribers" },
                    { KoreCmsLocalizableStrings.Newsletters.SuccessfullySignedUp, "You have successfully signed up for newsletters." },
                    { KoreCmsLocalizableStrings.Newsletters.Title, "Newsletters" },
                    { KoreCmsLocalizableStrings.Pages.CannotDeleteOnlyVersion, "Cannot delete the only version of a page. Create a new version first." },
                    { KoreCmsLocalizableStrings.Pages.History, "History" },
                    { KoreCmsLocalizableStrings.Pages.ManagePages, "Manage Pages" },
                    { KoreCmsLocalizableStrings.Pages.Page, "Page" },
                    { KoreCmsLocalizableStrings.Pages.PageHistory, "Page History" },
                    { KoreCmsLocalizableStrings.Pages.PageHistoryRestoreConfirm, "Are you sure you want to restore this version?" },
                    { KoreCmsLocalizableStrings.Pages.PageHistoryRestoreError, "There was an error when trying to restore the specified page version." },
                    { KoreCmsLocalizableStrings.Pages.PageHistoryRestoreSuccess, "Successfully restored specified page version." },
                    { KoreCmsLocalizableStrings.Pages.PageModel.IsEnabled, "Is Enabled" },
                    { KoreCmsLocalizableStrings.Pages.PageModel.Name, "Name" },
                    { KoreCmsLocalizableStrings.Pages.PageModel.Order, "Order" },
                    { KoreCmsLocalizableStrings.Pages.PageModel.PageTypeId, "Page Type" },
                    { KoreCmsLocalizableStrings.Pages.PageModel.Roles, "Roles" },
                    { KoreCmsLocalizableStrings.Pages.PageVersionModel.CultureCode, "Culture Code" },
                    { KoreCmsLocalizableStrings.Pages.PageVersionModel.DateCreated, "Date Created (UTC)" },
                    { KoreCmsLocalizableStrings.Pages.PageVersionModel.DateModified, "Date Modified (UTC)" },
                    { KoreCmsLocalizableStrings.Pages.PageVersionModel.IsDraft, "Is Draft" },
                    { KoreCmsLocalizableStrings.Pages.PageVersionModel.ShowOnMenus, "Show on Menus" },
                    { KoreCmsLocalizableStrings.Pages.PageVersionModel.Slug, "URL Slug" },
                    { KoreCmsLocalizableStrings.Pages.PageVersionModel.Status, "Status" },
                    { KoreCmsLocalizableStrings.Pages.PageVersionModel.Title, "Title" },
                    { KoreCmsLocalizableStrings.Pages.PageTypeModel.DisplayTemplatePath, "Display Template Path" },
                    { KoreCmsLocalizableStrings.Pages.PageTypeModel.EditorTemplatePath, "Editor Template Path" },
                    { KoreCmsLocalizableStrings.Pages.PageTypeModel.LayoutPath, "Layout Path" },
                    { KoreCmsLocalizableStrings.Pages.PageTypeModel.Name, "Name" },
                    { KoreCmsLocalizableStrings.Pages.PageTypes.StandardPage.BodyContent, "Body Content" },
                    { KoreCmsLocalizableStrings.Pages.PageTypes.StandardPage.MetaDescription, "Meta Description" },
                    { KoreCmsLocalizableStrings.Pages.PageTypes.StandardPage.MetaKeywords, "Meta Keywords" },
                    { KoreCmsLocalizableStrings.Pages.PageTypes.StandardPage.MetaTitle, "Meta Title" },
                    { KoreCmsLocalizableStrings.Pages.PageTypes.Title, "Page Types" },
                    { KoreCmsLocalizableStrings.Pages.Restore, "Restore" },
                    { KoreCmsLocalizableStrings.Pages.RestoreVersion, "Restore Version" },
                    { KoreCmsLocalizableStrings.Pages.SelectPageToBeginEdit, "Select a page to begin editing." },
                    { KoreCmsLocalizableStrings.Pages.Tags, "Tags" },
                    { KoreCmsLocalizableStrings.Pages.Title, "Pages" },
                    { KoreCmsLocalizableStrings.Pages.Translations, "Translations" },
                    { KoreCmsLocalizableStrings.Pages.Versions, "Versions" },
                    { KoreCmsLocalizableStrings.Settings.Blog.AccessRestrictions, "Access Restrictions" },
                    { KoreCmsLocalizableStrings.Settings.Blog.DateFormat, "Date Format" },
                    { KoreCmsLocalizableStrings.Settings.Blog.ItemsPerPage, "# Items Per Page" },
                    { KoreCmsLocalizableStrings.Settings.Blog.MenuPosition, "Menu Position" },
                    { KoreCmsLocalizableStrings.Settings.Blog.PageTitle, "Page Title" },
                    { KoreCmsLocalizableStrings.Settings.Blog.ShowOnMenus, "Show on Menus" },
                    { KoreCmsLocalizableStrings.Settings.Blog.LayoutPathOverride, "Layout Path (Override)" },
                    { KoreCmsLocalizableStrings.Settings.Pages.NumberOfPageVersionsToKeep, "# Page Versions to Keep" },
                    { KoreCmsLocalizableStrings.Settings.Pages.ShowInvariantVersionIfLocalizedUnavailable, "Show Invariant Version if Localized Unavailable" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequency, "Change Frequency" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.Id, "ID" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.Location, "Location" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.Priority, "Priority" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Always, "Always" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Daily, "Daily" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Hourly, "Hourly" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Monthly, "Monthly" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Never, "Never" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Weekly, "Weekly" },
                    { KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Yearly, "Yearly" },
                    { KoreCmsLocalizableStrings.Sitemap.ConfirmGenerateFile, "Are you sure you want to generate a new XML sitemap file? Warning: This will replace the existing one." },
                    { KoreCmsLocalizableStrings.Sitemap.GenerateFile, "Generate File" },
                    { KoreCmsLocalizableStrings.Sitemap.GenerateFileError, "Error when generating XML sitemap file." },
                    { KoreCmsLocalizableStrings.Sitemap.GenerateFileSuccess, "Successfully generated XML sitemap file." },
                    { KoreCmsLocalizableStrings.Sitemap.Title, "Sitemap" },
                    { KoreCmsLocalizableStrings.Sitemap.XMLSitemap, "XML Sitemap" },
                    { KoreCmsLocalizableStrings.UserProfile.Newsletter.SubscribeToNewsletters, "Subscribe to Newsletters" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}