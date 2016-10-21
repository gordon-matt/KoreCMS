using Kore.Tenants.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KoreCMS.Data.Domain
{
    public class ApplicationRole : IdentityRole, ITenantEntity
    {
        public ApplicationRole()
            : base()
        {
        }

        public ApplicationRole(string roleName)
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