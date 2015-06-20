using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission Read = new Permission { Name = "Plugin_RoyalVideoPlayer_Read", Category = "Plugin - Royal Video Player", Description = "Plugin: Royal Video Player - Read" };
        public static readonly Permission Write = new Permission { Name = "Plugin_RoyalVideoPlayer_Write", Category = "Plugin - Royal Video Player", Description = "Plugin: Royal Video Player - Write" };

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