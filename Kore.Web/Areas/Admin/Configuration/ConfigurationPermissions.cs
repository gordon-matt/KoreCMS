using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Configuration
{
    public class ConfigurationPermissions : IPermissionProvider
    {
        public static readonly Permission ManageSettings = new Permission { Name = "ManageSettings", Category = "System", Description = "Manage settings" };

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageSettings
            };
        }
    }
}