using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets
{
    public class WidgetPermissions : IPermissionProvider
    {
        public static readonly Permission ManageWidgets = new Permission { Name = "ManageWidgets", Description = "Manage widgets", Category = "Content Management" };

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageWidgets
            };
        }
    }
}