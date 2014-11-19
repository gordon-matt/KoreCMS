using System.Collections.Generic;

namespace Kore.Security.Membership
{
    public enum UserStatus
    {
        NotRegistered = 0,
        Unconfirmed = 1,
        Locked = 2,
        Active = 3
    }

    public interface IMembershipService
    {
        bool SupportsRolePermissions { get; }

        #region Users

        IEnumerable<KoreUser> GetAllUsers();

        KoreUser GetUserById(object userId);

        KoreUser GetUserByName(string userName);

        IEnumerable<KoreRole> GetRolesForUser(object userId);

        bool DeleteUser(object userId);

        void InsertUser(KoreUser user, string password);

        void UpdateUser(KoreUser user);

        void AssignUserToRoles(object userId, IEnumerable<object> roleIds);

        void ChangePassword(object userId, string newPassword);

        #endregion Users

        #region Roles

        IEnumerable<KoreRole> GetAllRoles();

        KoreRole GetRoleById(object roleId);

        KoreRole GetRoleByName(string roleName);

        bool DeleteRole(object roleId);

        void InsertRole(KoreRole role);

        void UpdateRole(KoreRole role);

        IEnumerable<KoreUser> GetUsersByRoleId(object roleId);

        IEnumerable<KoreUser> GetUsersByRoleName(string roleName);

        #endregion Roles

        #region Permissions

        IEnumerable<KorePermission> GetAllPermissions();

        KorePermission GetPermissionById(object permissionId);

        KorePermission GetPermissionByName(string permissionName);

        IEnumerable<KorePermission> GetPermissionsForRole(string roleName);

        void AssignPermissionsToRole(object roleId, IEnumerable<object> permissionIds);

        bool DeletePermission(object permissionId);

        void InsertPermission(KorePermission permission);

        void UpdatePermission(KorePermission permission);

        #endregion Permissions
    }
}