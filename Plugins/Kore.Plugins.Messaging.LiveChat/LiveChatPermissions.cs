using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Messaging.LiveChat
{
    public class LiveChatPermissions : IPermissionProvider
    {
        public static readonly Permission Manage = new Permission { Name = "Plugin_LiveChat_Manage", Category = "Plugin - LiveChat", Description = "Plugin: Live Chat - Manage" };
        public static readonly Permission ViewAgentPage = new Permission { Name = "Plugin_LiveChat_ViewAgentPage", Category = "Plugin - LiveChat", Description = "Plugin: Live Chat - View Agent Page" };
        public static readonly Permission ViewSetupPage = new Permission { Name = "Plugin_LiveChat_ViewSetupPage", Category = "Plugin - LiveChat", Description = "Plugin: Live Chat - View Setup Page" };

        #region IPermissionProvider Members

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                Manage,
                ViewAgentPage,
                ViewSetupPage
            };
        }

        #endregion IPermissionProvider Members
    }
}