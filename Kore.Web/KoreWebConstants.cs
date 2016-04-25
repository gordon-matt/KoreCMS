using Kore.Infrastructure;
using Kore.Web.Configuration;

namespace Kore.Web
{
    public static class KoreWebConstants
    {
        private static string defaultAdminLayoutPath;
        private static string defaultFrontendLayoutPath;

        public static string DefaultAdminLayoutPath
        {
            get
            {
                if (string.IsNullOrEmpty(defaultAdminLayoutPath))
                {
                    var siteSettings = EngineContext.Current.Resolve<KoreSiteSettings>();

                    defaultAdminLayoutPath = string.IsNullOrEmpty(siteSettings.AdminLayoutPath)
                        ? "~/Areas/Admin/Views/Shared/_Layout.cshtml"
                        : siteSettings.AdminLayoutPath;
                }
                return defaultAdminLayoutPath;
            }
        }

        public static string DefaultFrontendLayoutPath
        {
            get
            {
                if (string.IsNullOrEmpty(defaultFrontendLayoutPath))
                {
                    var siteSettings = EngineContext.Current.Resolve<KoreSiteSettings>();

                    defaultFrontendLayoutPath = string.IsNullOrEmpty(siteSettings.DefaultFrontendLayoutPath)
                        ? "~/Views/Shared/_Layout.cshtml"
                        : siteSettings.DefaultFrontendLayoutPath;
                }
                return defaultFrontendLayoutPath;
            }
        }

        public static class Areas
        {
            public const string Admin = "Admin";
            public const string Configuration = "Admin/Configuration";
            public const string Indexing = "Admin/Indexing";
            public const string Log = "Admin/Log";
            public const string Membership = "Admin/Membership";
            public const string Plugins = "Admin/Plugins";
            public const string ScheduledTasks = "Admin/ScheduledTasks";
            public const string Tenants = "Admin/Tenants";
        }

        public static class CacheKeys
        {
            public const string CurrentCulture = "CacheKeys.CurrentCulture";
        }

        public static class Indexing
        {
            public const string DefaultIndexName = "Search";
        }

        public static class Roles
        {
            public const string Administrators = "Administrators";
        }

        //public static class Features
        //{
        //    public const string Media = "Kore.ContentManagement.Media";
        //}

        public static class StateProviders
        {
            public const string CurrentCultureCode = "CurrentCultureCode";
            public const string CurrentDesktopTheme = "CurrentDesktopTheme";
            public const string CurrentMobileTheme = "CurrentMobileTheme";
            public const string CurrentUser = "CurrentUser";
        }

        /// <summary>
        /// Resets static variables to NULL
        /// </summary>
        public static void ResetCache()
        {
            defaultAdminLayoutPath = null;
            defaultFrontendLayoutPath = null;
        }
    }
}