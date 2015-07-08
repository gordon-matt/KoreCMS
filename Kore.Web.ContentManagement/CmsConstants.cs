using System.Text.RegularExpressions;

namespace Kore.Web.ContentManagement
{
    public static class CmsConstants
    {
        public static class RegexPatterns
        {
            public static readonly Regex Email = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static class Areas
        {
            public const string Blocks = "Admin/Blocks";
            public const string Blog = "Admin/Blog";
            public const string Localization = "Admin/Localization";
            public const string Media = "Admin/Media";
            public const string Messaging = "Admin/Messaging";
            public const string Menus = "Admin/Menus";
            public const string Newsletters = "Admin/Newsletters";
            public const string Pages = "Admin/Pages";
            public const string Sitemap = "Admin/Sitemap";
        }

        public static class CacheKeys
        {
            public const string ContentBlockScriptExpression = "Kore_CMS_Blocks_ScriptExpression_{0}";
            public const string MediaMimeType = "Kore_CMS_Media_MimType_{0}";
            public const string MediaImageEntityTypesAll = "Kore_CMS_Media_ImageEntityTypes_All";
        }

        internal static class Tables
        {
            internal const string BlogPosts = "Kore_BlogPosts";
            internal const string BlogPostTags = "Kore_BlogPostTags";
            internal const string BlogCategories = "Kore_BlogCategories";
            internal const string BlogTags = "Kore_BlogTags";
            internal const string ContentBlocks = "Kore_ContentBlocks";
            internal const string EntityTypeContentBlocks = "Kore_EntityTypeContentBlocks";
            internal const string HistoricPages = "Kore_HistoricPages";
            internal const string MenuItems = "Kore_MenuItems";
            internal const string Menus = "Kore_Menus";
            internal const string MessageTemplates = "Kore_MessageTemplates";
            internal const string Pages = "Kore_Pages";
            internal const string PageVersions = "Kore_PageVersions";
            internal const string PageTypes = "Kore_PageTypes";
            internal const string SitemapConfig = "Kore_SitemapConfig";
            internal const string QueuedEmails = "Kore_QueuedEmails";
            internal const string QueuedSMS = "Kore_QueuedSMS";
            internal const string Zones = "Kore_Zones";
        }
    }
}