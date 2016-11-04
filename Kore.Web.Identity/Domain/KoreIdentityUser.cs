using Kore.Tenants.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kore.Web.Identity.Domain
{
    public class KoreIdentityUser : IdentityUser, ITenantEntity
    {
        public int? TenantId { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}