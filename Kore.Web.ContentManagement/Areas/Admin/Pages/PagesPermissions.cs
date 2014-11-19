using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class PagesPermissions : IPermissionProvider
    {
        public static readonly Permission ManagePages = new Permission { Name = "ManagePages", Category = "Content Management", Description = "Manage Pages" };

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManagePages
            };
        }
    }
}