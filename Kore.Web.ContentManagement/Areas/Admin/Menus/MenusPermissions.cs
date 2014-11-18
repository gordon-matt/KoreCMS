using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus
{
    public class MenusPermissions : IPermissionProvider
    {
        public static readonly Permission ManageMenus = new Permission { Name = "ManageMenus", Category = "Content Management", Description = "Manage menus" };

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ManageMenus;
        }
    }
}