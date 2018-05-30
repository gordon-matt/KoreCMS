using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin
{
    public class KoreWebPermissions : IPermissionProvider
    {
        // Localization
        public static readonly Permission LanguagesRead = new Permission { Name = "Languages_Read", Category = "Localization", Description = "Kore - Languages: Read" };
        public static readonly Permission LanguagesWrite = new Permission { Name = "Languages_Write", Category = "Localization", Description = "Kore - Languages: Write" };
        public static readonly Permission LocalizableStringsRead = new Permission { Name = "LocalizableStrings_Read", Category = "Localization", Description = "Kore - Localizable Strings: Read" };
        public static readonly Permission LocalizableStringsWrite = new Permission { Name = "LocalizableStrings_Write", Category = "Localization", Description = "Kore - Localizable Strings: Write" };

        // Log
        public static readonly Permission LogRead = new Permission { Name = "Log_Read", Category = "Log", Description = "Kore - Log: Read" };

        // Plugins
        public static readonly Permission PluginsManage = new Permission { Name = "Plugins_Manage", Category = "System", Description = "Kore - Plugins: Manage" };

        // Scheduled Tasks
        public static readonly Permission ScheduledTasksRead = new Permission { Name = "Scheduled_Tasks_Read", Category = "System", Description = "Kore - Scheduled Tasks: Read" };
        public static readonly Permission ScheduledTasksWrite = new Permission { Name = "Scheduled_Tasks_Write", Category = "System", Description = "Kore - Scheduled Tasks: Write" };

        // Settings
        public static readonly Permission SettingsRead = new Permission { Name = "Settings_Read", Category = "Configuration", Description = "Kore - Settings: Read" };
        public static readonly Permission SettingsWrite = new Permission { Name = "Settings_Write", Category = "Configuration", Description = "Kore - Settings: Write" };

        // Themes
        public static readonly Permission ThemesRead = new Permission { Name = "Themes_Read", Category = "Configuration", Description = "Kore - Themes: Read" };
        public static readonly Permission ThemesWrite = new Permission { Name = "Themes_Write", Category = "Configuration", Description = "Kore - Themes: Write" };

        // Membership
        public static readonly Permission MembershipManage = new Permission { Name = "Membership_Manage", Category = "Membership", Description = "Kore - Membership: Manage" };
        public static readonly Permission MembershipPermissionsRead = new Permission { Name = "Membership_Permissions_Read", Category = "Membership", Description = "Kore - Membership: Read Permissions" };
        public static readonly Permission MembershipPermissionsWrite = new Permission { Name = "Membership_Permissions_Write", Category = "Membership", Description = "Kore - Membership: Write Permissions" };
        public static readonly Permission MembershipRolesRead = new Permission { Name = "Membership_Roles_Read", Category = "Membership", Description = "Kore - Membership: Read Roles" };
        public static readonly Permission MembershipRolesWrite = new Permission { Name = "Membership_Roles_Write", Category = "Membership", Description = "Kore - Membership: Write Roles" };
        public static readonly Permission MembershipUsersRead = new Permission { Name = "Membership_Users_Read", Category = "Membership", Description = "Kore - Membership: Read Users" };
        public static readonly Permission MembershipUsersWrite = new Permission { Name = "Membership_Users_Write", Category = "Membership", Description = "Kore - Membership: Read Users" };

        public IEnumerable<Permission> GetPermissions()
        {
            yield return LanguagesRead;
            yield return LanguagesWrite;
            yield return LocalizableStringsRead;
            yield return LocalizableStringsWrite;

            yield return LogRead;

            yield return PluginsManage;

            yield return ScheduledTasksRead;
            yield return ScheduledTasksWrite;

            yield return SettingsRead;
            yield return SettingsWrite;

            yield return ThemesRead;
            yield return ThemesWrite;

            yield return MembershipManage;
            yield return MembershipPermissionsRead;
            yield return MembershipPermissionsWrite;
            yield return MembershipRolesRead;
            yield return MembershipRolesWrite;
            yield return MembershipUsersRead;
            yield return MembershipUsersWrite;
        }
    }
}