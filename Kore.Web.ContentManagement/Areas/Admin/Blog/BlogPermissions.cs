using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog
{
    public class BlogPermissions : IPermissionProvider
    {
        public static readonly Permission ManageBlog = new Permission { Name = "ManageBlog", Category = "Content Management", Description = "Manage Blog" };

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageBlog
            };
        }
    }
}