using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Membership
{
    public class MembershipPermissions : IPermissionProvider
    {
        public static readonly Permission Manage = new Permission { Name = "Membership_Manage", Category = "Membership", Description = "Membership: Manage" };
        public static readonly Permission ReadPermissions = new Permission { Name = "Membership_Permissions_Read", Category = "Membership", Description = "Membership: Read Permissions" };
        public static readonly Permission ReadRoles = new Permission { Name = "Membership_Roles_Read", Category = "Membership", Description = "Membership: Read Roles" };
        public static readonly Permission ReadUsers = new Permission { Name = "Membership_Users_Read", Category = "Membership", Description = "Membership: Read Users" };
        public static readonly Permission WritePermissions = new Permission { Name = "Membership_Permissions_Write", Category = "Membership", Description = "Membership: Write Permissions" };
        public static readonly Permission WriteRoles = new Permission { Name = "Membership_Roles_Write", Category = "Membership", Description = "Membership: Write Roles" };
        public static readonly Permission WriteUsers = new Permission { Name = "Membership_Users_Write", Category = "Membership", Description = "Membership: Read Users" };

        public IEnumerable<Permission> GetPermissions()
        {
            yield return Manage;
            yield return ReadPermissions;
            yield return ReadRoles;
            yield return ReadUsers;
            yield return WritePermissions;
            yield return WriteRoles;
            yield return WriteUsers;
        }
    }
}