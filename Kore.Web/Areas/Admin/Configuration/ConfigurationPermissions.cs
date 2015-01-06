using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Configuration
{
    public class ConfigurationPermissions : IPermissionProvider
    {
        public static readonly Permission ManageSettings = new Permission { Name = "ManageSettings", Category = "System", Description = "Manage Settings" };
        public static readonly Permission ManageThemes = new Permission { Name = "ManageThemes", Category = "System", Description = "Manage Themes" };

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageSettings,
                ManageThemes
            };
        }
    }
}