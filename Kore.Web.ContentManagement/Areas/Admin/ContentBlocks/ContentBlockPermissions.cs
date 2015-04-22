using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public class ContentBlockPermissions : IPermissionProvider
    {
        public static readonly Permission ManageContentBlocks = new Permission { Name = "ManageContentBlocks", Description = "Manage Content Blocks", Category = "Content Management" };

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageContentBlocks
            };
        }
    }
}