using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple
{
    public class SimpleCommercePermissions : IPermissionProvider
    {
        public static readonly Permission ManageStore = new Permission { Name = "ManageStore", Category = "e-Commerce", Description = "Manage Store" };

        #region IPermissionProvider Members

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageStore
            };
        }

        #endregion IPermissionProvider Members
    }
}