using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using Kore;
using Kore.Collections;
using Kore.Data;
using Kore.Exceptions;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Infrastructure;
using Kore.Web.Security.Membership;
using KoreCMS.Data;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Kore.EntityFramework.Data;
using Kore.EntityFramework.Data.EntityFramework;

namespace KoreCMS.Services
{
    public abstract class IdentityMembershipService : IMembershipService, IDisposable
    {
        private readonly ApplicationDbContext dbContext;

        private readonly UserStore<ApplicationUser> userStore;
        private readonly ApplicationUserManager userManager;
        private readonly RoleStore<ApplicationRole> roleStore;
        private readonly ApplicationRoleManager roleManager;
        private readonly IRepository<UserProfileEntry> userProfileRepository;

        private static Dictionary<string, List<KoreRole>> cachedUserRoles;
        private static Dictionary<string, List<KorePermission>> cachedRolePermissions;

        static IdentityMembershipService()
        {
            cachedUserRoles = new Dictionary<string, List<KoreRole>>();
            cachedRolePermissions = new Dictionary<string, List<KorePermission>>();
        }

        public IdentityMembershipService(IRepository<UserProfileEntry> userProfileRepository)
        {
            var settings = DataSettingsManager.LoadSettings();
            dbContext = new ApplicationDbContext(settings.ConnectionString);

            this.userStore = new UserStore<ApplicationUser>(dbContext);
            this.roleStore = new RoleStore<ApplicationRole>(dbContext);
            this.userManager = new ApplicationUserManager(userStore);
            this.roleManager = new ApplicationRoleManager(roleStore);
            this.userProfileRepository = userProfileRepository;
        }

        #region IMembershipService Members

        public bool SupportsRolePermissions
        {
            get { return true; }
        }

        public async Task<string> GenerateEmailConfirmationToken(object userId)
        {
            string id = userId.ToString();
            return await userManager.GenerateEmailConfirmationTokenAsync(id);
        }

        public async Task ConfirmEmail(object userId, string token)
        {
            await userManager.ConfirmEmailAsync(userId.ToString(), token);
        }

        #region Users

        public IQueryable<KoreUser> GetAllUsersAsQueryable(int? tenantId)
        {
            IQueryable<ApplicationUser> query = dbContext.Users;

            if (tenantId.HasValue)
            {
                query = query.Where(x => x.TenantId == tenantId);
            }

            return query
                .Select(x => new KoreUser
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    IsLockedOut = x.LockoutEnabled
                });
        }

        public async Task<IEnumerable<KoreUser>> GetAllUsers(int? tenantId)
        {
            return await GetAllUsersAsQueryable(tenantId).ToHashSetAsync();
        }

        public async Task<IEnumerable<KoreUser>> GetUsers(int? tenantId, Expression<Func<KoreUser, bool>> predicate)
        {
            return await GetAllUsersAsQueryable(tenantId)
                .Where(predicate)
                .ToHashSetAsync();
        }

        public async Task<KoreUser> GetUserById(object userId)
        {
            string id = userId.ToString();
            //var user = userManager.FindById(id);

            var user = dbContext.Users.Find(userId);

            if (user == null)
            {
                return null;
            }

            return await Task.FromResult(new KoreUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsLockedOut = user.LockoutEnabled
            });
        }

        public async Task<KoreUser> GetUserByEmail(int? tenantId, string email)
        {
            ApplicationUser user;

            if (tenantId.HasValue)
            {
                user = await dbContext.Users.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Email == email);
            }
            else
            {
                user = await userManager.FindByEmailAsync(email);
            }

            if (user == null)
            {
                return null;
            }

            return new KoreUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsLockedOut = user.LockoutEnabled
            };
        }

        public async Task<KoreUser> GetUserByName(int? tenantId, string userName)
        {
            ApplicationUser user;

            if (tenantId.HasValue)
            {
                user = await dbContext.Users.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.UserName == userName);
            }
            else
            {
                user = await userManager.FindByNameAsync(userName);
            }

            if (user == null)
            {
                return null;
            }

            return new KoreUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsLockedOut = user.LockoutEnabled
            };
        }

        public async Task<IEnumerable<KoreRole>> GetRolesForUser(object userId)
        {
            string id = userId.ToString();
            if (cachedUserRoles.ContainsKey(id))
            {
                return cachedUserRoles[id];
            }

            var roleNames = await userManager.GetRolesAsync(id);

            var roles = roleManager.Roles
                .Where(x => roleNames.Contains(x.Name))
                .Select(x => new KoreRole
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            cachedUserRoles.Add(id, roles);
            return roles;
        }

        public async Task<bool> DeleteUser(object userId)
        {
            string id = userId.ToString();
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                return result.Succeeded;
            }

            return false;
        }

        public async Task InsertUser(int? tenantId, KoreUser user, string password)
        {
            // Check for spaces in UserName above, because of this:
            // http://stackoverflow.com/questions/30078332/bug-in-asp-net-identitys-usermanager
            string userName = (user.UserName.Contains(" ") ? user.UserName.Replace(" ", "_") : user.UserName);

            var appUser = new ApplicationUser
            {
                TenantId = tenantId,
                UserName = userName,
                Email = user.Email,
                LockoutEnabled = user.IsLockedOut
            };

            var result = await userManager.CreateAsync(appUser, password);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors);
                throw new KoreException(errorMessage);
            }
        }

        public async Task UpdateUser(KoreUser user)
        {
            string userId = user.Id.ToString();
            var existingUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.LockoutEnabled = user.IsLockedOut;
                var result = await userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(Environment.NewLine, result.Errors);
                    throw new KoreException(errorMessage);
                }
            }
        }

        public async Task AssignUserToRoles(object userId, IEnumerable<object> roleIds)
        {
            string uId = userId.ToString();

            var ids = roleIds.Select(x => Convert.ToString(x));
            var roleNames = await roleManager.Roles.Where(x => ids.Contains(x.Id)).Select(x => x.Name).ToListAsync();

            var currentRoles = await userManager.GetRolesAsync(uId);

            var toDelete = currentRoles.Where(x => !roleNames.Contains(x));
            var toAdd = roleNames.Where(x => !currentRoles.Contains(x));

            if (toDelete.Any())
            {
                foreach (string roleName in toDelete)
                {
                    var result = await userManager.RemoveFromRoleAsync(uId, roleName);

                    if (!result.Succeeded)
                    {
                        string errorMessage = string.Join(Environment.NewLine, result.Errors);
                        throw new KoreException(errorMessage);
                    }
                }
                cachedUserRoles.Remove(uId);
            }

            if (toAdd.Any())
            {
                foreach (string roleName in toAdd)
                {
                    var result = await userManager.AddToRoleAsync(uId, roleName);

                    if (!result.Succeeded)
                    {
                        string errorMessage = string.Join(Environment.NewLine, result.Errors);
                        throw new KoreException(errorMessage);
                    }
                }
                cachedUserRoles.Remove(uId);
            }
        }

        public async Task ChangePassword(object userId, string newPassword)
        {
            //TODO: This doesn't seem to be working very well; no errors, but can't login with the given password
            string id = userId.ToString();
            var result = await userManager.RemovePasswordAsync(id);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors);
                throw new KoreException(errorMessage);
            }

            result = await userManager.AddPasswordAsync(id, newPassword);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors);
                throw new KoreException(errorMessage);
            }
            //var user = userManager.FindById(id);
            //string passwordHash = userManager.PasswordHasher.HashPassword(newPassword);
            //userStore.SetPasswordHashAsync(user, passwordHash);
            //userManager.UpdateSecurityStamp(id);
        }

        public async Task<string> GetUserDisplayName(KoreUser user)
        {
            var profile = await GetProfile(user.Id);

            bool hasFamilyName = profile.ContainsKey(AccountUserProfileProvider.Fields.FamilyName);
            bool hasGivenNames = profile.ContainsKey(AccountUserProfileProvider.Fields.GivenNames);

            if (hasFamilyName && hasGivenNames)
            {
                string familyName = profile[AccountUserProfileProvider.Fields.FamilyName];
                string givenNames = profile[AccountUserProfileProvider.Fields.GivenNames];

                if (profile.ContainsKey(AccountUserProfileProvider.Fields.ShowFamilyNameFirst))
                {
                    bool showFamilyNameFirst = bool.Parse(profile[AccountUserProfileProvider.Fields.ShowFamilyNameFirst]);

                    if (showFamilyNameFirst)
                    {
                        return familyName + " " + givenNames;
                    }
                    return givenNames + " " + familyName;
                }
                return givenNames + " " + familyName;
            }
            else if (hasFamilyName)
            {
                return profile[AccountUserProfileProvider.Fields.FamilyName];
            }
            else if (hasGivenNames)
            {
                return profile[AccountUserProfileProvider.Fields.GivenNames];
            }
            else
            {
                return user.UserName;
            }
        }

        #endregion Users

        #region Roles

        public async Task<IEnumerable<KoreRole>> GetAllRoles(int? tenantId)
        {
            IQueryable<ApplicationRole> query = roleManager.Roles;

            if (tenantId.HasValue)
            {
                query = query.Where(x => x.TenantId == tenantId);
            }

            return await query
                .Select(x => new KoreRole
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<KoreRole> GetRoleById(object roleId)
        {
            string id = roleId.ToString();
            var role = await roleManager.FindByIdAsync(id);

            return new KoreRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<IEnumerable<KoreRole>> GetRolesByIds(IEnumerable<object> roleIds)
        {
            var ids = roleIds.ToListOf<string>();
            var roles = new List<ApplicationRole>();

            foreach (string id in ids)
            {
                var role = await roleManager.FindByIdAsync(id);
                roles.Add(role);
            }

            return roles.Select(x => new KoreRole
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public async Task<KoreRole> GetRoleByName(int? tenantId, string roleName)
        {
            ApplicationRole role;

            if (tenantId.HasValue)
            {
                role = await roleManager.Roles.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Name == roleName);
            }
            else
            {
                role = await roleManager.FindByNameAsync(roleName);
            }

            if (role == null)
            {
                return null;
            }

            return new KoreRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<bool> DeleteRole(object roleId)
        {
            string id = roleId.ToString();
            var role = await roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                return result.Succeeded;
            }

            return false;
        }

        public async Task InsertRole(int? tenantId, KoreRole role)
        {
            var result = await roleManager.CreateAsync(new ApplicationRole
            {
                TenantId = tenantId,
                Name = role.Name
            });

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors);
                throw new KoreException(errorMessage);
            }
        }

        public async Task UpdateRole(KoreRole role)
        {
            string id = role.Id.ToString();
            var existingRole = await roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRole != null)
            {
                existingRole.Name = role.Name;
                var result = await roleManager.UpdateAsync(existingRole);

                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(Environment.NewLine, result.Errors);
                    throw new KoreException(errorMessage);
                }
            }
        }

        public async Task<IEnumerable<KoreUser>> GetUsersByRoleId(object roleId)
        {
            string rId = roleId.ToString();
            var role = await roleManager.Roles.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == rId);

            var userIds = role.Users.Select(x => x.UserId).ToList();
            var users = await userManager.Users.Where(x => userIds.Contains(x.Id)).ToHashSetAsync();

            return users.Select(x => new KoreUser
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                IsLockedOut = x.LockoutEnabled
            });
        }

        public async Task<IEnumerable<KoreUser>> GetUsersByRoleName(int? tenantId, string roleName)
        {
            ApplicationRole role;

            if (tenantId.HasValue)
            {
                role = await roleManager.Roles.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Name == roleName);
            }
            else
            {
                role = await roleManager.FindByNameAsync(roleName);
            }

            var userIds = role.Users.Select(x => x.UserId).ToList();
            var users = await userManager.Users.Where(x => userIds.Contains(x.Id)).ToHashSetAsync();

            return users.Select(x => new KoreUser
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                IsLockedOut = x.LockoutEnabled
            });
        }

        #endregion Roles

        #region Permissions

        public async Task<IEnumerable<KorePermission>> GetAllPermissions(int? tenantId)
        {
            IQueryable<Permission> query = dbContext.Permissions;

            if (tenantId.HasValue)
            {
                query = query.Where(x => x.TenantId == tenantId);
            }

            return (await query.ToListAsync()).Select(x => new KorePermission
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Category = x.Category,
                Description = x.Description
            }).ToList();
        }

        public async Task<KorePermission> GetPermissionById(object permissionId)
        {
            int id = Convert.ToInt32(permissionId);
            var entity = await dbContext.Permissions.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return null;
            }

            return new KorePermission
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                Category = entity.Category,
                Description = entity.Description
            };
        }

        public async Task<KorePermission> GetPermissionByName(int? tenantId, string permissionName)
        {
            Permission entity = null;

            if (tenantId.HasValue)
            {
                entity = await dbContext.Permissions.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Name == permissionName);
            }
            else
            {
                entity = await dbContext.Permissions.FirstOrDefaultAsync(x => x.Name == permissionName);
            }

            if (entity == null)
            {
                return null;
            }

            return new KorePermission
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                Category = entity.Category,
                Description = entity.Description
            };
        }

        public async Task<IEnumerable<KorePermission>> GetPermissionsForRole(int? tenantId, string roleName)
        {
            if (cachedRolePermissions.ContainsKey(roleName))
            {
                return cachedRolePermissions[roleName];
            }

            var query = dbContext.Permissions.Include(x => x.Roles);

            List<Permission> permissions = null;
            if (tenantId.HasValue)
            {
                permissions = await (from p in query
                                     from rp in p.Roles
                                     where p.TenantId == tenantId && rp.Name == roleName
                                     select p).ToListAsync();
            }
            else
            {
                permissions = await (from p in query
                                     from rp in p.Roles
                                     where rp.Name == roleName
                                     select p).ToListAsync();
            }

            var rolePermissions = permissions.Select(x => new KorePermission
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Category = x.Category,
                Description = x.Description
            }).ToList();

            cachedRolePermissions.Add(roleName, rolePermissions);
            return rolePermissions;
        }

        // TODO: This should not have raw SQL, else we cannot suppoirt PG, MySQL, etc..
        public async Task AssignPermissionsToRole(object roleId, IEnumerable<object> permissionIds)
        {
            // The code below is all a little unusual, because we don't have a navigation property on the role entity

            string rId = roleId.ToString();

            if (roleId is Array)
            {
                rId = ((Array)roleId).GetValue(0).ToString();
            }

            var pIds = permissionIds.ToListOf<int>();

            using (var transactionScope = new TransactionScope())
            {
                string deleteStatement = string.Format("DELETE FROM [RolePermissions] WHERE RoleId = '{0}'", rId);
                dbContext.Database.ExecuteSqlCommand(deleteStatement);

                //This EF code doesn't work, probably because we confused it by removing permissions "behind its back" above..

                //var role = dbContext.Roles.Find(rId);
                //foreach (int pId in pIds)
                //{
                //    var permission = dbContext.Permissions.Find(pId);
                //    permission.Roles.Add(role);
                //}

                //...so we need to add them manually as well
                var insertStatements = pIds
                    .Select(id => string.Format("INSERT INTO [RolePermissions](RoleId, PermissionId) VALUES('{0}', {1})", rId, id))
                    .ToList();

                if (insertStatements.Any())
                {
                    dbContext.Database.ExecuteSqlCommand(string.Join(System.Environment.NewLine, insertStatements));
                }

                transactionScope.Complete();
            }

            string roleName = (await GetRoleById(rId)).Name;
            cachedRolePermissions.Remove(roleName);

            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeletePermission(object permissionId)
        {
            var id = Convert.ToInt32(permissionId);
            var existing = await dbContext.Permissions.FirstOrDefaultAsync(x => x.Id == id);

            if (existing != null)
            {
                dbContext.Permissions.Remove(existing);
                int rowsAffected = await dbContext.SaveChangesAsync();
                return rowsAffected > 0;
            }
            return false;
        }

        public async Task<bool> DeletePermissions(IEnumerable<object> permissionIds)
        {
            var ids = permissionIds.Select(x => x.ToString()).ToListOf<int>();
            var toDelete = await dbContext.Permissions.Where(x => ids.Contains(x.Id)).ToListAsync();

            if (toDelete.Any())
            {
                dbContext.Permissions.RemoveRange(toDelete);
                int rowsAffected = await dbContext.SaveChangesAsync();
                return rowsAffected > 0;
            }
            return false;
        }

        public async Task InsertPermission(int? tenantId, KorePermission permission)
        {
            dbContext.Permissions.Add(new Permission
            {
                TenantId = tenantId,
                Name = permission.Name,
                Category = permission.Category,
                Description = permission.Description
            });
            await dbContext.SaveChangesAsync();
        }

        public async Task InsertPermissions(int? tenantId, IEnumerable<KorePermission> permissions)
        {
            var toInsert = permissions.Select(x => new Permission
            {
                TenantId = tenantId,
                Name = x.Name,
                Category = x.Category,
                Description = x.Description
            });
            dbContext.Permissions.AddRange(toInsert);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdatePermission(KorePermission permission)
        {
            var id = Convert.ToInt32(permission.Id);
            var existing = await dbContext.Permissions.FirstOrDefaultAsync(x => x.Id == id);
            existing.Name = permission.Name;
            existing.Category = permission.Category;
            existing.Description = permission.Description;
            await dbContext.SaveChangesAsync();
        }

        #endregion Permissions

        #region Profile

        public async Task<IDictionary<string, string>> GetProfile(string userId)
        {
            using (var connection = userProfileRepository.OpenConnection())
            {
                return await connection.Query(x => x.UserId == userId).ToDictionaryAsync(k => k.Key, v => v.Value);
            }
        }

        public async Task<IEnumerable<UserProfile>> GetProfiles(IEnumerable<string> userIds)
        {
            using (var connection = userProfileRepository.OpenConnection())
            {
                var entries = await connection.Query(x => userIds.Contains(x.UserId)).ToListAsync();
                return entries.GroupBy(x => x.UserId).Select(x => new UserProfile
                {
                    UserId = x.Key,
                    Profile = x.ToDictionary(k => k.Key, v => v.Value)
                });
            }
        }

        public async Task UpdateProfile(string userId, IDictionary<string, string> profile, bool deleteExisting = false)
        {
            List<UserProfileEntry> entries = null;
            using (var connection = userProfileRepository.OpenConnection())
            {
                entries = await connection.Query(x => x.UserId == userId).ToListAsync();
            }

            if (deleteExisting)
            {
                await userProfileRepository.DeleteAsync(entries);

                var toInsert = profile.Select(x => new UserProfileEntry
                {
                    UserId = userId,
                    Key = x.Key,
                    Value = x.Value
                }).ToList();

                await userProfileRepository.InsertAsync(toInsert);
            }
            else
            {
                var toUpdate = new List<UserProfileEntry>();
                var toInsert = new List<UserProfileEntry>();

                foreach (var keyValue in profile)
                {
                    var existing = entries.FirstOrDefault(x => x.Key == keyValue.Key);

                    if (existing != null)
                    {
                        existing.Value = keyValue.Value;
                        toUpdate.Add(existing);
                    }
                    else
                    {
                        toInsert.Add(new UserProfileEntry
                        {
                            UserId = userId,
                            Key = keyValue.Key,
                            Value = keyValue.Value
                        });
                    }
                }

                if (toUpdate.Any())
                {
                    await userProfileRepository.UpdateAsync(toUpdate);
                }

                if (toInsert.Any())
                {
                    await userProfileRepository.InsertAsync(toInsert);
                }
            }
        }

        public async Task<string> GetProfileEntry(string userId, string key)
        {
            var entry = await userProfileRepository.FindOneAsync(x =>
                x.UserId == userId &&
                x.Key == key);

            if (entry != null)
            {
                return entry.Value;
            }

            return null;
        }

        public async Task SaveProfileEntry(string userId, string key, string value)
        {
            var entry = await userProfileRepository.FindOneAsync(x =>
                x.UserId == userId &&
                x.Key == key);

            if (entry != null)
            {
                entry.Value = value;
                await userProfileRepository.UpdateAsync(entry);
            }
            else
            {
                await userProfileRepository.InsertAsync(new UserProfileEntry
                {
                    UserId = userId,
                    Key = key,
                    Value = value
                });
            }
        }

        public async Task DeleteProfileEntry(string userId, string key)
        {
            var entry = await userProfileRepository.FindOneAsync(x =>
                x.UserId == userId &&
                x.Key == key);

            if (entry != null)
            {
                await userProfileRepository.DeleteAsync(entry);
            }
        }

        public async Task<IEnumerable<KoreUserProfileEntry>> GetProfileEntriesByKey(int? tenantId, string key)
        {
            using (var connection = userProfileRepository.OpenConnection())
            {
                var query = connection.Query();

                if (tenantId.HasValue)
                {
                    query = query.Where(x => x.TenantId == tenantId && x.Key == key);
                }
                else
                {
                    query = query.Where(x => x.Key == key);
                }

                return (await query.ToHashSetAsync())
                    .Select(x => new KoreUserProfileEntry
                    {
                        Id = x.Id.ToString(),
                        UserId = x.UserId,
                        Key = x.Key,
                        Value = x.Value
                    });
            }
        }

        public async Task<IEnumerable<KoreUserProfileEntry>> GetProfileEntriesByKeyAndValue(int? tenantId, string key, string value)
        {
            using (var connection = userProfileRepository.OpenConnection())
            {
                var query = connection.Query();

                if (tenantId.HasValue)
                {
                    query = query.Where(x => x.TenantId == tenantId && x.Key == key && x.Value == value);
                }
                else
                {
                    query = query.Where(x => x.Key == key && x.Value == value);
                }

                return (await query.ToHashSetAsync())
                    .Select(x => new KoreUserProfileEntry
                    {
                        Id = x.Id.ToString(),
                        UserId = x.UserId,
                        Key = x.Key,
                        Value = x.Value
                    });
            }
        }

        public async Task<bool> ProfileEntryExists(int? tenantId, string key, string value, string userId = null)
        {
            using (var connection = userProfileRepository.OpenConnection())
            {
                IQueryable<UserProfileEntry> query = null;

                if (tenantId.HasValue)
                {
                    query = connection.Query(x => x.TenantId == tenantId && x.Key == key && x.Value == value);
                }
                else
                {
                    query = connection.Query(x => x.Key == key && x.Value == value);
                }

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.UserId == userId);
                }
                return await query.AnyAsync();
            }
        }

        #endregion Profile

        #endregion IMembershipService Members

        #region IDisposable Members

        public void Dispose()
        {
            dbContext.DisposeIfNotNull();
        }

        #endregion IDisposable Members
    }
}