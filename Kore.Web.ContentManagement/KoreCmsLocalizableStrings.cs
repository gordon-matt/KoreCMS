﻿namespace Kore.Web.ContentManagement
{
    public static class KoreCmsLocalizableStrings
    {
        public static class Blog
        {
            public const string Categories = "Kore.Web.ContentManagement/Blog.Categories";
            public const string ManageBlog = "Kore.Web.ContentManagement/Blog.ManageBlog";
            public const string PostedByXOnX = "Kore.Web.ContentManagement/Blog.PostedByXOnX";
            public const string Posts = "Kore.Web.ContentManagement/Blog.Posts";
            public const string Tags = "Kore.Web.ContentManagement/Blog.Tags";
            public const string Title = "Kore.Web.ContentManagement/Blog.Title";

            public static class CategoryModel
            {
                public const string Name = "Kore.Web.ContentManagement/Blog.CategoryModel.Name";
                public const string UrlSlug = "Kore.Web.ContentManagement/Blog.CategoryModel.UrlSlug";
            }

            public static class PostModel
            {
                public const string CategoryId = "Kore.Web.ContentManagement/Blog.PostModel.CategoryId";
                public const string DateCreatedUtc = "Kore.Web.ContentManagement/Blog.PostModel.DateCreatedUtc";
                public const string ExternalLink = "Kore.Web.ContentManagement/Blog.PostModel.ExternalLink";
                public const string FullDescription = "Kore.Web.ContentManagement/Blog.PostModel.FullDescription";
                public const string Headline = "Kore.Web.ContentManagement/Blog.PostModel.Headline";
                public const string MetaDescription = "Kore.Web.ContentManagement/Blog.PostModel.MetaDescription";
                public const string MetaKeywords = "Kore.Web.ContentManagement/Blog.PostModel.MetaKeywords";
                public const string ShortDescription = "Kore.Web.ContentManagement/Blog.PostModel.ShortDescription";
                public const string Slug = "Kore.Web.ContentManagement/Blog.PostModel.Slug";
                public const string TeaserImageUrl = "Kore.Web.ContentManagement/Blog.PostModel.TeaserImageUrl";
                public const string UseExternalLink = "Kore.Web.ContentManagement/Blog.PostModel.UseExternalLink";
            }

            public static class TagModel
            {
                public const string Name = "Kore.Web.ContentManagement/Blog.TagModel.Name";
                public const string UrlSlug = "Kore.Web.ContentManagement/Blog.TagModel.UrlSlug";
            }
        }

        public static class ContentBlocks
        {
            public const string ManageContentBlocks = "Kore.Web.ContentManagement/ContentBlocks.ManageContentBlocks";
            public const string ManageZones = "Kore.Web.ContentManagement/ContentBlocks.ManageZones";
            public const string Title = "Kore.Web.ContentManagement/ContentBlocks.Title";
            public const string Zones = "Kore.Web.ContentManagement/ContentBlocks.Zones";

            #region Blog

            public static class AllPostsBlock
            {
                public const string CategoryId = "Kore.Web.ContentManagement/ContentBlocks.AllPostsBlock.CategoryId";
                public const string FilterType = "Kore.Web.ContentManagement/ContentBlocks.AllPostsBlock.FilterType";
                public const string TagId = "Kore.Web.ContentManagement/ContentBlocks.AllPostsBlock.TagId";
            }

            public static class CategoriesBlock
            {
                public const string NumberOfCategories = "Kore.Web.ContentManagement/ContentBlocks.CategoriesBlock.NumberOfCategories";
            }

            public static class LastNPostsBlock
            {
                public const string NumberOfEntries = "Kore.Web.ContentManagement/ContentBlocks.LastNPostsBlock.NumberOfEntries";
            }

            public static class TagCloudBlock
            {
                //public const string Width = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.Width";
                //public const string WidthUnit = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.WidthUnit";
                //public const string Height = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.Height";
                //public const string HeightUnit = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.HeightUnit";
                //public const string CenterX = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.CenterX";
                //public const string CenterY = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.CenterY";
                public const string AutoResize = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.AutoResize";

                public const string Steps = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.Steps";
                public const string ClassPattern = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.ClassPattern";
                public const string AfterCloudRender = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.AfterCloudRender";
                public const string Delay = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.Delay";
                public const string Shape = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.Shape";
                public const string RemoveOverflowing = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.RemoveOverflowing";
                public const string EncodeURI = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.EncodeURI";
                public const string Colors = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.Colors";
                public const string FontSizeFrom = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.FontSizeFrom";
                public const string FontSizeTo = "Kore.Web.ContentManagement/ContentBlocks.TagCloudBlock.FontSizeTo";
            }

            #endregion Blog

            public static class FormBlock
            {
                public static class HelpText
                {
                    public const string EmailAddress = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.HelpText.EmailAddress";
                    public const string FormUrl = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.HelpText.FormUrl";
                }

                public const string EmailAddress = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.EmailAddress";
                public const string FormUrl = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.FormUrl";
                public const string HtmlTemplate = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.HtmlTemplate";
                public const string PleaseEnterCaptcha = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.PleaseEnterCaptcha";
                public const string PleaseEnterCorrectCaptcha = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.PleaseEnterCorrectCaptcha";
                public const string RedirectUrl = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.RedirectUrl";
                public const string SaveResultIfNotRedirectPleaseClick = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.SaveResultIfNotRedirectPleaseClick";
                public const string SaveResultRedirect = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.SaveResultRedirect";
                public const string ThankYouMessage = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.ThankYouMessage";
                public const string UseAjax = "Kore.Web.ContentManagement/ContentBlocks.FormBlock.UseAjax";
            }

            public static class HtmlBlock
            {
                public static class HelpText
                {
                    public const string BodyContent = "Kore.Web.ContentManagement/ContentBlocks.HtmlBlock.HelpText.BodyContent";
                    public const string Script = "Kore.Web.ContentManagement/ContentBlocks.HtmlBlock.HelpText.Script";
                }

                public const string BodyContent = "Kore.Web.ContentManagement/ContentBlocks.HtmlBlock.BodyContent";
                public const string Script = "Kore.Web.ContentManagement/ContentBlocks.HtmlBlock.Script";
            }

            public static class LanguageSwitchBlock
            {
                public const string CustomTemplatePath = "Kore.Web.ContentManagement/ContentBlocks.LanguageSwitchBlock.CustomTemplatePath";
                public const string IncludeInvariant = "Kore.Web.ContentManagement/ContentBlocks.LanguageSwitchBlock.IncludeInvariant";
                public const string InvariantText = "Kore.Web.ContentManagement/ContentBlocks.LanguageSwitchBlock.InvariantText";
                public const string Style = "Kore.Web.ContentManagement/ContentBlocks.LanguageSwitchBlock.Style";
                public const string UseUrlPrefix = "Kore.Web.ContentManagement/ContentBlocks.LanguageSwitchBlock.UseUrlPrefix";
            }

            public static class Model
            {
                public const string BlockType = "Kore.Web.ContentManagement/ContentBlocks.Model.BlockType";
                public const string CustomTemplatePath = "Kore.Web.ContentManagement/ContentBlocks.Model.CustomTemplatePath";
                public const string IsEnabled = "Kore.Web.ContentManagement/ContentBlocks.Model.IsEnabled";
                public const string Order = "Kore.Web.ContentManagement/ContentBlocks.Model.Order";
                public const string Title = "Kore.Web.ContentManagement/ContentBlocks.Model.Title";
                public const string ZoneId = "Kore.Web.ContentManagement/ContentBlocks.Model.ZoneId";
            }

            public static class NewsletterSubscriptionBlock
            {
                public const string Email = "Kore.Web.ContentManagement/ContentBlocks.NewsletterSubscriptionBlock.Email";
                public const string EmailPlaceholder = "Kore.Web.ContentManagement/ContentBlocks.NewsletterSubscriptionBlock.EmailPlaceholder";
                public const string Name = "Kore.Web.ContentManagement/ContentBlocks.NewsletterSubscriptionBlock.Name";
                public const string NamePlaceholder = "Kore.Web.ContentManagement/ContentBlocks.NewsletterSubscriptionBlock.NamePlaceholder";
                public const string SignUpLabel = "Kore.Web.ContentManagement/ContentBlocks.NewsletterSubscriptionBlock.SignUpLabel";
            }

            public static class VideoBlock
            {
                public const string ControlId = "Kore.Web.ContentManagement/ContentBlocks.VideoBlock.ControlId";
                public const string Type = "Kore.Web.ContentManagement/ContentBlocks.VideoBlock.Type";
                public const string Source = "Kore.Web.ContentManagement/ContentBlocks.VideoBlock.Source";
                public const string ShowControls = "Kore.Web.ContentManagement/ContentBlocks.VideoBlock.ShowControls";
                public const string AutoPlay = "Kore.Web.ContentManagement/ContentBlocks.VideoBlock.AutoPlay";
                public const string Loop = "Kore.Web.ContentManagement/ContentBlocks.VideoBlock.Loop";
                public const string VideoTagNotSupported = "Kore.Web.ContentManagement/ContentBlocks.VideoBlock.VideoTagNotSupported";
            }

            public static class ZoneModel
            {
                public const string Name = "Kore.Web.ContentManagement/ContentBlocks.ZoneModel.Name";
            }
        }

        public static class Media
        {
            public const string FileBrowserTitle = "Kore.Web.ContentManagement/Media.FileBrowserTitle";
            public const string ManageMedia = "Kore.Web.ContentManagement/Media.ManageMedia";
            public const string Title = "Kore.Web.ContentManagement/Media.Title";
        }

        public static class Menus
        {
            public const string IsExternalUrl = "Kore.Web.ContentManagement/Menus.IsExternalUrl";
            public const string Items = "Kore.Web.ContentManagement/Menus.Items";
            public const string ManageMenuItems = "Kore.Web.ContentManagement/Menus.ManageMenuItems";
            public const string ManageMenus = "Kore.Web.ContentManagement/Menus.ManageMenus";
            public const string MenuItems = "Kore.Web.ContentManagement/Menus.MenuItems";
            public const string NewItem = "Kore.Web.ContentManagement/Menus.NewItem";
            public const string Title = "Kore.Web.ContentManagement/Menus.Title";

            public static class MenuItemModel
            {
                public const string CssClass = "Kore.Web.ContentManagement/Menus.MenuItemModel.CssClass";
                public const string Description = "Kore.Web.ContentManagement/Menus.MenuItemModel.Description";
                public const string Enabled = "Kore.Web.ContentManagement/Menus.MenuItemModel.Enabled";
                public const string Position = "Kore.Web.ContentManagement/Menus.MenuItemModel.Position";
                public const string Text = "Kore.Web.ContentManagement/Menus.MenuItemModel.Text";
                public const string Url = "Kore.Web.ContentManagement/Menus.MenuItemModel.Url";
            }

            public static class MenuModel
            {
                public const string Name = "Kore.Web.ContentManagement/Menus.MenuModel.Name";
                public const string UrlFilter = "Kore.Web.ContentManagement/Menus.MenuModel.UrlFilter";
            }
        }

        public static class Messages
        {
            public const string CircularRelationshipError = "Kore.Web.ContentManagement/Messages.CircularRelationshipError";
            public const string ConfirmClearLocalizableStrings = "Kore.Web.ContentManagement/Messages.ConfirmClearLocalizableStrings";
            public const string GetTranslationError = "Kore.Web.ContentManagement/Messages.GetTranslationError";
            public const string UpdateTranslationError = "Kore.Web.ContentManagement/Messages.UpdateTranslationError";
            public const string UpdateTranslationSuccess = "Kore.Web.ContentManagement/Messages.UpdateTranslationSuccess";
        }

        public static class Messaging
        {
            public const string GetTokensError = "Kore.Web.ContentManagement/Messaging.GetTokensError";
            public const string MessageTemplates = "Kore.Web.ContentManagement/Messaging.MessageTemplates";
            public const string QueuedEmails = "Kore.Web.ContentManagement/Messaging.QueuedEmails";
            public const string Title = "Kore.Web.ContentManagement/Messaging.Title";

            public static class MessageTemplateModel
            {
                public const string Body = "Kore.Web.ContentManagement/Messaging.MessageTemplateModel.Body";
                public const string Enabled = "Kore.Web.ContentManagement/Messaging.MessageTemplateModel.Enabled";
                public const string Name = "Kore.Web.ContentManagement/Messaging.MessageTemplateModel.Name";
                public const string Subject = "Kore.Web.ContentManagement/Messaging.MessageTemplateModel.Subject";
                public const string Tokens = "Kore.Web.ContentManagement/Messaging.MessageTemplateModel.Tokens";
            }

            public static class QueuedEmailModel
            {
                public const string CreatedOnUtc = "Kore.Web.ContentManagement/Messaging.QueuedEmailModel.CreatedOnUtc";
                public const string SentOnUtc = "Kore.Web.ContentManagement/Messaging.QueuedEmailModel.SentOnUtc";
                public const string SentTries = "Kore.Web.ContentManagement/Messaging.QueuedEmailModel.SentTries";
                public const string Subject = "Kore.Web.ContentManagement/Messaging.QueuedEmailModel.Subject";
                public const string ToAddress = "Kore.Web.ContentManagement/Messaging.QueuedEmailModel.ToAddress";
            }
        }

        public static class Navigation
        {
            public const string CMS = "Kore.Web.ContentManagement/Navigation.CMS";
        }

        public static class Newsletters
        {
            public const string ExportToCSV = "Kore.Web.ContentManagement/Newsletters.ExportToCSV";
            public const string Subscribers = "Kore.Web.ContentManagement/Newsletters.Subscribers";
            public const string SuccessfullySignedUp = "Kore.Web.ContentManagement/Newsletters.SuccessfullySignedUp";
            public const string Title = "Kore.Web.ContentManagement/Newsletters.Title";
        }

        public static class Pages
        {
            public const string ConfirmRestoreVersion = "Kore.Web.ContentManagement/Pages.ConfirmRestoreVersion";
            public const string History = "Kore.Web.ContentManagement/Pages.History";
            public const string ManagePages = "Kore.Web.ContentManagement/Pages.ManagePages";
            public const string Page = "Kore.Web.ContentManagement/Pages.Page";
            public const string PageHistory = "Kore.Web.ContentManagement/Pages.PageHistory";
            public const string PageHistoryRestoreConfirm = "Kore.Web.ContentManagement/Pages.PageHistoryRestoreConfirm";
            public const string PageHistoryRestoreError = "Kore.Web.ContentManagement/Pages.PageHistoryRestoreError";
            public const string PageHistoryRestoreSuccess = "Kore.Web.ContentManagement/Pages.PageHistoryRestoreSuccess";
            public const string Restore = "Kore.Web.ContentManagement/Pages.Restore";
            public const string RestoreVersion = "Kore.Web.ContentManagement/Pages.RestoreVersion";
            public const string SelectPageToBeginEdit = "Kore.Web.ContentManagement/Pages.SelectPageToBeginEdit";
            public const string Tags = "Kore.Web.ContentManagement/Pages.Tags";
            public const string Title = "Kore.Web.ContentManagement/Pages.Title";
            public const string Translations = "Kore.Web.ContentManagement/Pages.Translations";
            public const string Versions = "Kore.Web.ContentManagement/Pages.Versions";

            public const string CannotDeleteOnlyVersion = "Kore.Web.ContentManagement/Pages.CannotDeleteOnlyVersion";

            public static class PageModel
            {
                public const string IsEnabled = "Kore.Web.ContentManagement/Pages.PageModel.IsEnabled";
                public const string Name = "Kore.Web.ContentManagement/Pages.PageModel.Name";
                public const string Order = "Kore.Web.ContentManagement/Pages.PageModel.Order";
                public const string PageTypeId = "Kore.Web.ContentManagement/Pages.PageModel.PageTypeId";
                public const string Roles = "Kore.Web.ContentManagement/Pages.PageModel.Roles";
            }

            public static class PageVersionModel
            {
                public const string CultureCode = "Kore.Web.ContentManagement/Pages.PageVersionModel.CultureCode";
                public const string DateCreated = "Kore.Web.ContentManagement/Pages.PageVersionModel.DateCreated";
                public const string DateModified = "Kore.Web.ContentManagement/Pages.PageVersionModel.DateModified";
                public const string IsDraft = "Kore.Web.ContentManagement/Pages.PageVersionModel.IsDraft";
                public const string ShowOnMenus = "Kore.Web.ContentManagement/Pages.PageVersionModel.ShowOnMenus";
                public const string Slug = "Kore.Web.ContentManagement/Pages.PageVersionModel.Slug";
                public const string Status = "Kore.Web.ContentManagement/Pages.PageVersionModel.Status";
                public const string Title = "Kore.Web.ContentManagement/Pages.PageVersionModel.Title";
            }

            public static class PageTypeModel
            {
                public const string DisplayTemplatePath = "Kore.Web.ContentManagement/Pages.PageTypeModel.DisplayTemplatePath";
                public const string EditorTemplatePath = "Kore.Web.ContentManagement/Pages.PageTypeModel.EditorTemplatePath";
                public const string LayoutPath = "Kore.Web.ContentManagement/Pages.PageTypeModel.LayoutPath";
                public const string Name = "Kore.Web.ContentManagement/Pages.PageTypeModel.Name";
            }

            public static class PageTypes
            {
                public const string Title = "Kore.Web.ContentManagement/Pages.PageTypes.Title";

                public static class StandardPage
                {
                    public const string BodyContent = "Kore.Web.ContentManagement/Pages.PageTypes.StandardPage.BodyContent";
                    public const string MetaDescription = "Kore.Web.ContentManagement/Pages.PageTypes.StandardPage.MetaDescription";
                    public const string MetaKeywords = "Kore.Web.ContentManagement/Pages.PageTypes.StandardPage.MetaKeywords";
                    public const string MetaTitle = "Kore.Web.ContentManagement/Pages.PageTypes.StandardPage.MetaTitle";
                }
            }
        }

        public static class Settings
        {
            public static class Blog
            {
                public const string AccessRestrictions = "Kore.Web.ContentManagement/Settings.Blog.AccessRestrictions";
                public const string DateFormat = "Kore.Web.ContentManagement/Settings.Blog.DateFormat";
                public const string ItemsPerPage = "Kore.Web.ContentManagement/Settings.Blog.ItemsPerPage";
                public const string MenuPosition = "Kore.Web.ContentManagement/Settings.Blog.MenuPosition";
                public const string PageTitle = "Kore.Web.ContentManagement/Settings.Blog.PageTitle";
                public const string ShowOnMenus = "Kore.Web.ContentManagement/Settings.Blog.ShowOnMenus";
                public const string LayoutPathOverride = "Kore.Web.ContentManagement/Settings.Blog.LayoutPathOverride";
            }

            public static class Pages
            {
                public const string NumberOfPageVersionsToKeep = "Kore.Web.ContentManagement/Settings.Pages.NumberOfPageVersionsToKeep";
                public const string ShowInvariantVersionIfLocalizedUnavailable = "Kore.Web.ContentManagement/Settings.Pages.ShowInvariantVersionIfLocalizedUnavailable";
            }
        }

        public static class Sitemap
        {
            public static class Model
            {
                public static class ChangeFrequencies
                {
                    public const string Always = "Kore.Web.ContentManagement/Sitemap.Model.ChangeFrequencies.Always";
                    public const string Hourly = "Kore.Web.ContentManagement/Sitemap.Model.ChangeFrequencies.Hourly";
                    public const string Daily = "Kore.Web.ContentManagement/Sitemap.Model.ChangeFrequencies.Daily";
                    public const string Weekly = "Kore.Web.ContentManagement/Sitemap.Model.ChangeFrequencies.Weekly";
                    public const string Monthly = "Kore.Web.ContentManagement/Sitemap.Model.ChangeFrequencies.Monthly";
                    public const string Yearly = "Kore.Web.ContentManagement/Sitemap.Model.ChangeFrequencies.Yearly";
                    public const string Never = "Kore.Web.ContentManagement/Sitemap.Model.ChangeFrequencies.Never";
                }

                public const string ChangeFrequency = "Kore.Web.ContentManagement/SitemapModel.ChangeFrequency";
                public const string Id = "Kore.Web.ContentManagement/SitemapModel.Id";
                public const string Location = "Kore.Web.ContentManagement/SitemapModel.Location";
                public const string Priority = "Kore.Web.ContentManagement/SitemapModel.Priority";
            }

            public const string ConfirmGenerateFile = "Kore.Web.ContentManagement/Sitemap.ConfirmGenerateFile";
            public const string GenerateFile = "Kore.Web.ContentManagement/Sitemap.GenerateFile";
            public const string GenerateFileError = "Kore.Web.ContentManagement/Sitemap.GenerateFileError";
            public const string GenerateFileSuccess = "Kore.Web.ContentManagement/Sitemap.GenerateFileSuccess";
            public const string Title = "Kore.Web.ContentManagement/Sitemap.Title";
            public const string XMLSitemap = "Kore.Web.ContentManagement/Sitemap.XMLSitemap";
        }

        public static class UserProfile
        {
            public static class Newsletter
            {
                public const string SubscribeToNewsletters = "Kore.Web.ContentManagement/UserProfile.Newsletter.SubscribeToNewsletters";
            }
        }
    }
}