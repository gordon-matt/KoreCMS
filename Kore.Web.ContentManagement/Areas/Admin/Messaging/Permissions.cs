using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Messaging
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageMessages = new Permission
        {
            Name = "ManageMessages",
            Category = "Content Management",
            Description = "Manage Messages"
        };

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageMessages
            };
        }
    }
}