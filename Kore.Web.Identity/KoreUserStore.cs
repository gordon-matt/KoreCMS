using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;
using Kore.Web.Identity.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kore.Web.Identity
{
    public class KoreUserStore<TUser, TRole> : UserStore<TUser, TRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
        where TUser : KoreIdentityUser
        where TRole : KoreIdentityRole
    {
        private IWorkContext workContext;
        private IDbContextFactory contextFactory;

        public KoreUserStore(DbContext context)
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

        private IDbContextFactory ContextFactory
        {
            get
            {
                if (contextFactory == null)
                {
                    contextFactory = EngineContext.Current.Resolve<IDbContextFactory>();
                }
                return contextFactory;
            }
        }

        private int TenantId
        {
            get
            {
                return WorkContext.CurrentTenant.Id;
            }
        }

        public override Task<TUser> FindByEmailAsync(string email)
        {
            return this.GetUserAggregateAsync(x =>
                x.Email.ToUpper() == email.ToUpper()
                && (x.TenantId == this.TenantId) || (x.TenantId == null));
        }

        public override Task<TUser> FindByNameAsync(string userName)
        {
            return this.GetUserAggregateAsync(x =>
                x.UserName.ToUpper() == userName.ToUpper()
                && (x.TenantId == this.TenantId) || (x.TenantId == null));
        }

        public override Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "roleName");
            }

            TRole role;
            using (var context = ContextFactory.GetContext())
            {
                role = context.Set<TRole>().SingleOrDefault(x => x.TenantId == TenantId && x.Name == roleName);
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

            using (var context = ContextFactory.GetContext())
            {
                context.Set<IdentityUserRole>().Add(userRole);
                context.SaveChanges();
            }

            return Task.FromResult<int>(0);
        }

        public override Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "roleName");
            }

            TRole role;
            using (var context = ContextFactory.GetContext())
            {
                role = context.Set<TRole>().SingleOrDefault(x => x.TenantId == TenantId && x.Name == roleName);
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

        public override Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "roleName");
            }

            TRole role;
            using (var context = ContextFactory.GetContext())
            {
                role = context.Set<TRole>().SingleOrDefault(x => x.TenantId == TenantId && x.Name == roleName);

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