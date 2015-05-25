using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Maintenance.Watchdog
{
    public class WatchdogPermissions : IPermissionProvider
    {
        public static readonly Permission Read = new Permission { Name = "Plugin_Watchdog_Read", Category = "Plugin - Watchdog", Description = "Plugin: Watchdog - Read" };
        public static readonly Permission Write = new Permission { Name = "Plugin_Watchdog_Write", Category = "Plugin - Watchdog", Description = "Plugin: Watchdog - Write" };
        public static readonly Permission StartStopServices = new Permission { Name = "Plugin_Watchdog_StartStopServices", Category = "Plugin - Watchdog", Description = "Plugin: Watchdog - Start/Stop Services" };

        #region IPermissionProvider Members

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                Read,
                Write,
                StartStopServices
            };
        }

        #endregion IPermissionProvider Members
    }
}