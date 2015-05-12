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
            public const string ContentBlocksByPageId = "Kore_CMS_Blocks_{0}";
            public const string ContentBlocksByPageIdAndZoneAndIncDisabled = "Kore_CMS_Blocks_{0}_{1}_{2}";
            public const string ContentBlockScriptExpression = "Kore_CMS_Blocks_ScriptExpression_{0}";
            public const string ContentZoneById = "Kore_CMS_Zones_{0}";
            public const string LanguagesActive = "Kore_CMS_Languages_Active";
            public const string LanguagesAll = "Kore_CMS_Languages_All";
            public const string LanguagesForCultureCode = "Kore_CMS_Languages_{0}";
            public const string LanguagesRightToLeft = "Kore_CMS_Languages_RTL";
            public const string MediaMimeType = "Kore_CMS_Media_MimType_{0}";
            public const string MediaImageEntityTypesAll = "Kore_CMS_Media_ImageEntityTypes_All";
            public const string MenuItemsByMenuIdAndEnabled = "Kore_CMS_Menus_{0}_{1}";
            public const string MessageTemplateByName = "Kore_CMS_Messaging_Template_{0}";
        }
    }
}