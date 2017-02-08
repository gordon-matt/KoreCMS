using System.Security.Claims;
using System.Threading.Tasks;
using Kore.Tenants.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kore.Web.Identity.Domain
{
    public abstract class KoreIdentityUser : IdentityUser, ITenantEntity
    {
        public int? TenantId { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public virtual async Task<ClaimsIdentity> GenerateUserIdentityAsync<TUser>(UserManager<TUser, string> manager)
            where TUser : KoreIdentityUser
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this as TUser, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual async Task<ClaimsIdentity> GenerateUserIdentityAsync<TUser>(UserManager<TUser, string> manager, string authenticationType)
            where TUser : KoreIdentityUser
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this as TUser, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}