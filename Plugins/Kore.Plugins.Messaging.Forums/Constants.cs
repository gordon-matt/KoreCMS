namespace Kore.Plugins.Messaging.Forums
{
    public static class Constants
    {
        public const string PluginSystemName = "Kore.Plugins.Messaging.Forums";
        public const string RouteArea = "Plugins/Messaging/Forums";

        public static class Roles
        {
            public const string ForumModerators = "Forum Moderators";
        }

        public static class Tables
        {
            public const string Groups = "Kore_Plugins_Forums_Groups";
            public const string Forums = "Kore_Plugins_Forums_Forums";
            public const string Posts = "Kore_Plugins_Forums_Posts";
            public const string Subscriptions = "Kore_Plugins_Forums_Subscriptions";
            public const string Topics = "Kore_Plugins_Forums_Topics";
            public const string PrivateMessages = "Kore_Plugins_Forums_PrivateMessages";
        }
    }
}