namespace Kore.Web
{
    public static class KoreWebConstants
    {
        public static class Areas
        {
            public const string Configuration = "Admin/Configuration";
            public const string Indexing = "Admin/Indexing";
            public const string Plugins = "Admin/Plugins";
            public const string ScheduledTasks = "Admin/ScheduledTasks";
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
    }
}