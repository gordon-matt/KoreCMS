using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.Google
{
    public class GooglePermissions : IPermissionProvider
    {
        public static readonly Permission SitemapRead = new Permission { Name = "Plugin_Google_Sitemap_Read", Category = "Plugin - Google", Description = "Plugin: Google Sitemap - Read" };
        public static readonly Permission SitemapWrite = new Permission { Name = "Plugin_Google_Sitemap_Write", Category = "Plugin - Google", Description = "Plugin: Google Sitemap - Write" };

        #region IPermissionProvider Members

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                SitemapRead,
                SitemapWrite
            };
        }

        #endregion IPermissionProvider Members
    }
}