using Kore.Tenants.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kore.Web.Identity.Domain
{
    public abstract class KoreIdentityRole : IdentityRole, ITenantEntity
    {
        public KoreIdentityRole()
            : base()
        {
        }

        public KoreIdentityRole(string roleName)
            : base(roleName)
        {
        }

        public int? TenantId { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}