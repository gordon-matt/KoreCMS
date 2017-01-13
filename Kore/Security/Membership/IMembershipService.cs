using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kore.Security.Membership
{
    //public enum UserStatus
    //{
    //    NotRegistered = 0,
    //    Unconfirmed = 1,
    //    Locked = 2,
    //    Active = 3
    //}

    public interface IMembershipService
    {
        bool SupportsRolePermissions { get; }

        Task<string> GenerateEmailConfirmationToken(object userId);

        Task ConfirmEmail(object userId, string token);

        #region Users

        IQueryable<KoreUser> GetAllUsersAsQueryable(int? tenantId);

        Task<IEnumerable<KoreUser>> GetAllUsers(int? tenantId);

        Task<IEnumerable<KoreUser>> GetUsers(int? tenantId, Expression<Func<KoreUser, bool>> predicate);

        Task<KoreUser> GetUserById(object userId);

        Task<KoreUser> GetUserByEmail(int? tenantId, string email);

        Task<KoreUser> GetUserByName(int? tenantId, string userName);

        Task<IEnumerable<KoreRole>> GetRolesForUser(object userId);

        Task<bool> DeleteUser(object userId);

        Task InsertUser(KoreUser user, string password);

        Task UpdateUser(KoreUser user);

        Task AssignUserToRoles(int? tenantId, object userId, IEnumerable<object> roleIds);

        Task ChangePassword(object userId, string newPassword);

        Task<string> GetUserDisplayName(KoreUser user);

        #endregion Users

        #region Roles

        Task<IEnumerable<KoreRole>> GetAllRoles(int? tenantId);

        Task<KoreRole> GetRoleById(object roleId);

        Task<IEnumerable<KoreRole>> GetRolesByIds(IEnumerable<object> roleIds);

        Task<KoreRole> GetRoleByName(int? tenantId, string roleName);

        Task<bool> DeleteRole(object roleId);

        Task InsertRole(KoreRole role);

        Task UpdateRole(KoreRole role);

        Task<IEnumerable<KoreUser>> GetUsersByRoleId(object roleId);

        Task<IEnumerable<KoreUser>> GetUsersByRoleName(int? tenantId, string roleName);

        #endregion Roles

        #region Permissions

        Task<IEnumerable<KorePermission>> GetAllPermissions(int? tenantId);

        Task<KorePermission> GetPermissionById(object permissionId);

        Task<KorePermission> GetPermissionByName(int? tenantId, string permissionName);

        Task<IEnumerable<KorePermission>> GetPermissionsForRole(int? tenantId, string roleName);

        Task AssignPermissionsToRole(object roleId, IEnumerable<object> permissionIds);

        Task<bool> DeletePermission(object permissionId);

        Task<bool> DeletePermissions(IEnumerable<object> permissionIds);

        Task InsertPermission(KorePermission permission);

        Task InsertPermissions(IEnumerable<KorePermission> permissions);

        Task UpdatePermission(KorePermission permission);

        #endregion Permissions

        #region Profile

        Task<IDictionary<string, string>> GetProfile(string userId);

        Task<IEnumerable<UserProfile>> GetProfiles(IEnumerable<string> userIds);

        Task UpdateProfile(string userId, IDictionary<string, string> profile, bool deleteExisting = false);

        Task<string> GetProfileEntry(string userId, string key);

        Task SaveProfileEntry(string userId, string key, string value);

        Task DeleteProfileEntry(string userId, string key);

        Task<IEnumerable<KoreUserProfileEntry>> GetProfileEntriesByKey(int? tenantId, string key);

        Task<IEnumerable<KoreUserProfileEntry>> GetProfileEntriesByKeyAndValue(int? tenantId, string key, string value);

        Task<bool> ProfileEntryExists(int? tenantId, string key, string value, string userId = null);

        #endregion Profile

        Task EnsureAdminRoleForTenant(int? tenantId);
    }

    public class UserProfile
    {
        public string UserId { get; set; }

        public IDictionary<string, string> Profile { get; set; }
    }
}