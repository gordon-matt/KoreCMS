using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Kore;
using Kore.Infrastructure;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KoreCMS.Data
{
    public class TenantUserStore : UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        private IWorkContext workContext;

        public TenantUserStore(DbContext context)
            : base(context)
        {
        }

        private IWorkContext WorkContext
        {
            get
            {
                if (workContext == null)
                {
                    workContext = EngineContext.Current.Resolve<IWorkContext>();
                }
                return workContext;
            }
        }

        private int TenantId
        {
            get
            {
                return WorkContext.CurrentTenant.Id;
            }
        }

        public override Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return this.GetUserAggregateAsync(x =>
                x.Email.ToUpper() == email.ToUpper()
                && (x.TenantId == this.TenantId) || (x.TenantId == null));
        }

        public override Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return this.GetUserAggregateAsync(x =>
                x.UserName.ToUpper() == userName.ToUpper()
                && (x.TenantId == this.TenantId) || (x.TenantId == null));
        }

        public override Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "roleName");
            }

            ApplicationRole role;
            using (var context = new ApplicationDbContext())
            {
                role = context.Set<ApplicationRole>().SingleOrDefault(x => x.TenantId == TenantId && x.Name == roleName);
            }

            if (role == null)
            {
                throw new InvalidOperationException(string.Format("Role {0} does not exist.", roleName));
            }

            var userRole = new IdentityUserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            using (var context = new ApplicationDbContext())
            {
                context.Set<IdentityUserRole>().Add(userRole);
                context.SaveChanges();
            }

            return Task.FromResult<int>(0);
        }

        public override Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "roleName");
            }

            ApplicationRole role;
            using (var context = new ApplicationDbContext())
            {
                role = context.Set<ApplicationRole>().SingleOrDefault(x => x.TenantId == TenantId && x.Name == roleName);
            }

            bool flag = false;
            if (role != null)
            {
                flag = role.Users.Any(ur =>
                {
                    if (!ur.RoleId.Equals(role.Id))
                    {
                        return false;
                    }
                    return ur.UserId.Equals(user.Id);
                });
            }
            return Task.FromResult(flag);
        }

        public override Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "roleName");
            }

            ApplicationRole role;
            using (var context = new ApplicationDbContext())
            {
                role = context.Set<ApplicationRole>().SingleOrDefault(x => x.TenantId == TenantId && x.Name == roleName);

                if (role != null)
                {
                    string id = role.Id;
                    string tKey = user.Id;
                    var userRole = context.Set<IdentityUserRole>().FirstOrDefault(r => id == r.RoleId && r.UserId == tKey);
                    if (userRole != null)
                    {
                        context.Set<IdentityUserRole>().Remove(userRole);
                        context.SaveChanges();
                    }
                }
                return Task.FromResult<int>(0);
            }
        }
    }
}