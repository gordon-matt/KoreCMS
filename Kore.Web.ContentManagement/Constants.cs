using System.Text.RegularExpressions;

namespace Kore.Web.ContentManagement
{
    public static class Constants
    {
        public static class RegexPatterns
        {
            public static readonly Regex Email = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static class Areas
        {
            public const string Blog = "Admin/Blog";
            public const string ContentBlocks = "Admin/ContentBlocks";
            public const string Localization = "Admin/Localization";
            public const string Media = "Admin/Media";
            public const string Membership = "Admin/Membership";
            public const string Messaging = "Admin/Messaging";
            public const string Menus = "Admin/Menus";
            public const string Newsletters = "Admin/Newsletters";
            public const string Pages = "Admin/Pages";
        }

        public static class CacheKeys
        {
            public const string ContentBlockScriptExpression = "Kore_CMS_Blocks_ScriptExpression_{0}";
            public const string MediaMimeType = "Kore_CMS_Media_MimType_{0}";
            public const string MediaImageEntityTypesAll = "Kore_CMS_Media_ImageEntityTypes_All";
        }
    }
}