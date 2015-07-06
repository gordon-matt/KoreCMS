using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.RevolutionSlider
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission Read = new Permission { Name = "Plugin_RevolutionSlider_Read", Category = "Plugin - Revolution Slider", Description = "Plugin: Revolution Slider - Read" };
        public static readonly Permission Write = new Permission { Name = "Plugin_RevolutionSlider_Write", Category = "Plugin - Revolution Slider", Description = "Plugin: Revolution Slider - Write" };

        #region IPermissionProvider Members

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                Read,
                Write
            };
        }

        #endregion IPermissionProvider Members
    }
}