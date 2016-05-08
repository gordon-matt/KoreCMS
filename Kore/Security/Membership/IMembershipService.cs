using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        string GenerateEmailConfirmationToken(object userId);

        void ConfirmEmail(object userId, string token);

        #region Users

        IEnumerable<KoreUser> GetAllUsers();

        IQueryable<KoreUser> GetAllUsersAsQueryable();

        IEnumerable<KoreUser> GetUsers(Expression<Func<KoreUser, bool>> predicate);

        KoreUser GetUserById(object userId);

        KoreUser GetUserByEmail(string email);

        KoreUser GetUserByName(string userName);

        IEnumerable<KoreRole> GetRolesForUser(object userId);

        bool DeleteUser(object userId);

        void InsertUser(KoreUser user, string password);

        void UpdateUser(KoreUser user);

        void AssignUserToRoles(object userId, IEnumerable<object> roleIds);

        void ChangePassword(object userId, string newPassword);

        string GetUserDisplayName(KoreUser user);

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

        bool DeletePermissions(IEnumerable<object> permissionIds);

        void InsertPermission(KorePermission permission);

        void InsertPermissions(IEnumerable<KorePermission> permissions);

        void UpdatePermission(KorePermission permission);

        #endregion Permissions

        #region Profile

        IDictionary<string, string> GetProfile(string userId);

        void UpdateProfile(string userId, IDictionary<string, string> profile, bool deleteExisting = false);

        string GetProfileEntry(string userId, string key);

        void SaveProfileEntry(string userId, string key, string value);

        void DeleteProfileEntry(string userId, string key);

        IEnumerable<KoreUserProfileEntry> GetProfileEntriesByKey(string key);

        IEnumerable<KoreUserProfileEntry> GetProfileEntriesByKeyAndValue(string key, string value);

        #endregion Profile
    }
}