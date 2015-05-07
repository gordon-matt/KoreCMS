using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Kore;
using Kore.Collections;
using Kore.Data;
using Kore.Exceptions;
using Kore.Security.Membership;
using Kore.Web.Security.Membership;
using KoreCMS.Data;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KoreCMS.Services
{
    public abstract class IdentityMembershipService<TDbContext> : IMembershipService, IDisposable
        where TDbContext : IdentityDbContext<ApplicationUser>, IKoreSecurityDbContext, new()
    {
        private readonly TDbContext dbContext;

        private readonly UserStore<ApplicationUser> userStore;
        private readonly ApplicationUserManager userManager;
        private readonly RoleStore<IdentityRole> roleStore;
        private readonly RoleManager<IdentityRole> roleManager;
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
            dbContext = new TDbContext();
            this.userStore = new UserStore<ApplicationUser>(dbContext);
            this.roleStore = new RoleStore<IdentityRole>(dbContext);
            this.userManager = new ApplicationUserManager(userStore);
            this.roleManager = new RoleManager<IdentityRole>(roleStore);
            this.userProfileRepository = userProfileRepository;
        }

        #region IMembershipService Members

        public bool SupportsRolePermissions
        {
            get { return true; }
        }

        #region Users

        public IEnumerable<KoreUser> GetAllUsers()
        {
            return GetAllUsersAsQueryable().ToHashSet();
        }

        public IQueryable<KoreUser> GetAllUsersAsQueryable()
        {
            return dbContext.Users.Select(x => new KoreUser
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                IsLockedOut = x.LockoutEnabled
            });
        }

        public IEnumerable<KoreUser> GetUsers(Expression<Func<KoreUser, bool>> predicate)
        {
            return GetAllUsersAsQueryable()
                .Where(predicate)
                .ToHashSet();
        }

        public KoreUser GetUserById(object userId)
        {
            string id = userId.ToString();
            //var user = userManager.FindById(id);

            var user = dbContext.Users.Find(userId);

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

        public KoreUser GetUserByEmail(string email)
        {
            var user = userManager.FindByEmailAsync(email).Result;

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

        public KoreUser GetUserByName(string userName)
        {
            var user = userManager.FindByNameAsync(userName).Result;

            //var user = dbContext.Users.FirstOrDefault(x => x.UserName == userName);

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

        public IEnumerable<KoreRole> GetRolesForUser(object userId)
        {
            string id = userId.ToString();
            if (cachedUserRoles.ContainsKey(id))
            {
                return cachedUserRoles[id];
            }

            var roleNames = userManager.GetRolesAsync(id).Result;

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

        public bool DeleteUser(object userId)
        {
            string id = userId.ToString();
            var user = userManager.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                var result = userManager.Delete(user);
                return result.Succeeded;
            }

            return false;
        }

        public void InsertUser(KoreUser user, string password)
        {
            string userName = (user.UserName.Contains(" ") ? user.UserName.Replace(" ", "_") : user.UserName);

            var appUser = new ApplicationUser
            {
                UserName = userName,
                Email = user.Email,
                LockoutEnabled = user.IsLockedOut
            };

            // Check for spaces in UserName above, because of this:
            // http://stackoverflow.com/questions/30078332/bug-in-asp-net-identitys-usermanager
            var result = userManager.Create(appUser, password);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors);
                throw new KoreException(errorMessage);
            }
        }

        public void UpdateUser(KoreUser user)
        {
            string userId = user.Id.ToString();
            var existingUser = userManager.Users.FirstOrDefault(x => x.Id == userId);

            if (user != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.LockoutEnabled = user.IsLockedOut;
                var result = userManager.Update(existingUser);

                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(Environment.NewLine, result.Errors);
                    throw new KoreException(errorMessage);
                }
            }
        }

        public void AssignUserToRoles(object userId, IEnumerable<object> roleIds)
        {
            string uId = userId.ToString();

            var ids = roleIds.Select(x => Convert.ToString(x));
            var roleNames = roleManager.Roles.Where(x => ids.Contains(x.Id)).Select(x => x.Name).ToList();

            var currentRoles = userManager.GetRoles(uId);

            var toDelete = currentRoles.Where(x => !roleNames.Contains(x));
            var toAdd = roleNames.Where(x => !currentRoles.Contains(x));

            if (toDelete.Any())
            {
                foreach (string roleName in toDelete)
                {
                    var result = userManager.RemoveFromRole(uId, roleName);

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
                    var result = userManager.AddToRole(uId, roleName);

                    if (!result.Succeeded)
                    {
                        string errorMessage = string.Join(Environment.NewLine, result.Errors);
                        throw new KoreException(errorMessage);
                    }
                }
                cachedUserRoles.Remove(uId);
            }
        }

        public void ChangePassword(object userId, string newPassword)
        {
            //TODO: This doesn't seem to be working very well; no errors, but can't login with the given password
            string id = userId.ToString();
            var result = userManager.RemovePassword(id);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors);
                throw new KoreException(errorMessage);
            }

            result = userManager.AddPassword(id, newPassword);

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

        public string GetUserDisplayName(KoreUser user)
        {
            var profile = GetProfile(user.Id);

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

        public IEnumerable<KoreRole> GetAllRoles()
        {
            return roleManager.Roles.Select(x => new KoreRole
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public KoreRole GetRoleById(object roleId)
        {
            string id = roleId.ToString();
            var role = roleManager.FindById(id);
            return new KoreRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public KoreRole GetRoleByName(string roleName)
        {
            var role = roleManager.FindByName(roleName);

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

        public bool DeleteRole(object roleId)
        {
            string id = roleId.ToString();
            var role = roleManager.FindById(id);

            if (role != null)
            {
                var result = roleManager.Delete(role);
                return result.Succeeded;
            }

            return false;
        }

        public void InsertRole(KoreRole role)
        {
            var result = roleManager.Create(new IdentityRole { Name = role.Name });

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors);
                throw new KoreException(errorMessage);
            }
        }

        public void UpdateRole(KoreRole role)
        {
            string id = role.Id.ToString();
            var existingRole = roleManager.Roles.FirstOrDefault(x => x.Id == id);

            if (existingRole != null)
            {
                existingRole.Name = role.Name;
                var result = roleManager.Update(existingRole);

                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(Environment.NewLine, result.Errors);
                    throw new KoreException(errorMessage);
                }
            }
        }

        public IEnumerable<KoreUser> GetUsersByRoleId(object roleId)
        {
            string rId = roleId.ToString();
            var role = roleManager.FindById(rId);

            var userIds = role.Users.Select(x => x.UserId).ToList();
            var users = userManager.Users.Where(x => userIds.Contains(x.Id));

            return users.Select(x => new KoreUser
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                IsLockedOut = x.LockoutEnabled
            });
        }

        public IEnumerable<KoreUser> GetUsersByRoleName(string roleName)
        {
            var role = roleManager.FindByName(roleName);

            var userIds = role.Users.Select(x => x.UserId).ToList();
            var users = userManager.Users.Where(x => userIds.Contains(x.Id));

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

        public IEnumerable<KorePermission> GetAllPermissions()
        {
            return dbContext.Permissions.ToList().Select(x => new KorePermission
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Category = x.Category,
                Description = x.Description
            }).ToList();
        }

        public KorePermission GetPermissionById(object permissionId)
        {
            int id = Convert.ToInt32(permissionId);
            var entity = dbContext.Permissions.FirstOrDefault(x => x.Id == id);

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

        public KorePermission GetPermissionByName(string permissionName)
        {
            var entity = dbContext.Permissions.FirstOrDefault(x => x.Name == permissionName);

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

        public IEnumerable<KorePermission> GetPermissionsForRole(string roleName)
        {
            if (cachedRolePermissions.ContainsKey(roleName))
            {
                return cachedRolePermissions[roleName];
            }

            var permissions = (from p in dbContext.Permissions.Include(x => x.Roles)
                               from rp in p.Roles
                               where rp.Name == roleName
                               select p).ToList();

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

        public void AssignPermissionsToRole(object roleId, IEnumerable<object> permissionIds)
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

            string roleName = GetRoleById(rId).Name;
            cachedRolePermissions.Remove(roleName);

            dbContext.SaveChanges();
        }

        public bool DeletePermission(object permissionId)
        {
            var id = Convert.ToInt32(permissionId);
            var existing = dbContext.Permissions.FirstOrDefault(x => x.Id == id);

            if (existing != null)
            {
                dbContext.Permissions.Remove(existing);
                int rowsAffected = dbContext.SaveChanges();
                return rowsAffected > 0;
            }
            return false;
        }

        public void InsertPermission(KorePermission permission)
        {
            dbContext.Permissions.Add(new Permission
            {
                Name = permission.Name,
                Category = permission.Category,
                Description = permission.Description
            });
            dbContext.SaveChanges();
        }

        public void UpdatePermission(KorePermission permission)
        {
            var id = Convert.ToInt32(permission.Id);
            var existing = dbContext.Permissions.FirstOrDefault(x => x.Id == id);
            existing.Name = permission.Name;
            existing.Category = permission.Category;
            existing.Description = permission.Description;
            dbContext.SaveChanges();
        }

        #endregion Permissions

        #region Profile

        public IDictionary<string, string> GetProfile(string userId)
        {
            return userProfileRepository.Table.Where(x => x.UserId == userId).ToDictionary(k => k.Key, v => v.Value);
        }

        public void UpdateProfile(string userId, IDictionary<string, string> profile, bool deleteExisting = false)
        {
            var entries = userProfileRepository.Table.Where(x => x.UserId == userId).ToList();

            if (deleteExisting)
            {
                userProfileRepository.Delete(entries);

                var toInsert = profile.Select(x => new UserProfileEntry
                {
                    UserId = userId,
                    Key = x.Key,
                    Value = x.Value
                }).ToList();

                userProfileRepository.Insert(toInsert);
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
                    userProfileRepository.Update(toUpdate);
                }

                if (toInsert.Any())
                {
                    userProfileRepository.Insert(toInsert);
                }
            }
        }

        public string GetProfileEntry(string userId, string key)
        {
            var entry = userProfileRepository.Table.FirstOrDefault(x =>
                x.UserId == userId &&
                x.Key == key);

            if (entry != null)
            {
                return entry.Value;
            }

            return null;
        }

        public void SaveProfileEntry(string userId, string key, string value)
        {
            var entry = userProfileRepository.Table.FirstOrDefault(x =>
                x.UserId == userId &&
                x.Key == key);

            if (entry != null)
            {
                entry.Value = value;
                userProfileRepository.Update(entry);
            }
            else
            {
                userProfileRepository.Insert(new UserProfileEntry
                {
                    UserId = userId,
                    Key = key,
                    Value = value
                });
            }
        }

        public void DeleteProfileEntry(string userId, string key)
        {
            var entry = userProfileRepository.Table.FirstOrDefault(x =>
                x.UserId == userId &&
                x.Key == key);

            if (entry != null)
            {
                userProfileRepository.Delete(entry);
            }
        }

        public IEnumerable<KoreUserProfileEntry> GetProfileEntriesByKey(string key)
        {
            return userProfileRepository.Table
                .Where(x => x.Key == key)
                .ToHashSet()
                .Select(x => new KoreUserProfileEntry
                {
                    Id = x.Id.ToString(),
                    UserId = x.UserId,
                    Key = x.Key,
                    Value = x.Value
                });
        }

        public IEnumerable<KoreUserProfileEntry> GetProfileEntriesByKeyAndValue(string key, string value)
        {
            return userProfileRepository.Table
                .Where(x =>
                    x.Key == key &&
                    x.Value == value)
                .ToHashSet()
                .Select(x => new KoreUserProfileEntry
                {
                    Id = x.Id.ToString(),
                    UserId = x.UserId,
                    Key = x.Key,
                    Value = x.Value
                });
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