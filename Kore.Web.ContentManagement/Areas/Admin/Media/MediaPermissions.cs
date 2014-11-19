using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Media
{
    public class MediaPermissions : IPermissionProvider
    {
        public static readonly Permission ManageMedia = new Permission { Name = "ManageMedia", Category = "Content Management", Description = "Manage media" };

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageMedia
            };
        }
    }
}