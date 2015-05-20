using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple
{
    public class SimpleCommercePermissions : IPermissionProvider
    {
        public static readonly Permission ReadCategories = new Permission { Name = "Plugin_SimpleCommerce_ReadCategories", Category = "Plugin - Simple Commerce", Description = "Plugin: Simple Commerce - Read Categories" };
        public static readonly Permission ReadProducts = new Permission { Name = "Plugin_SimpleCommerce_ReadProducts", Category = "Plugin - Simple Commerce", Description = "Plugin: Simple Commerce - Read Products" };
        public static readonly Permission WriteCategories = new Permission { Name = "Plugin_SimpleCommerce_WriteCategories", Category = "Plugin - Simple Commerce", Description = "Plugin: Simple Commerce - Write Categories" };
        public static readonly Permission WriteProducts = new Permission { Name = "Plugin_SimpleCommerce_WriteProducts", Category = "Plugin - Simple Commerce", Description = "Plugin: Simple Commerce - Write Products" };
        public static readonly Permission ReadOrders = new Permission { Name = "Plugin_SimpleCommerce_ReadOrders", Category = "Plugin - Simple Commerce", Description = "Plugin: Simple Commerce - Read Orders" };
        public static readonly Permission ReadAddresses = new Permission { Name = "Plugin_SimpleCommerce_ReadAddresses", Category = "Plugin - Simple Commerce", Description = "Plugin: Simple Commerce - Read Addresses" };
        public static readonly Permission WriteAddresses = new Permission { Name = "Plugin_SimpleCommerce_WriteAddresses", Category = "Plugin - Simple Commerce", Description = "Plugin: Simple Commerce - Write Addresses" };
        
        #region IPermissionProvider Members

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ReadAddresses,
                ReadCategories,
                ReadOrders,
                ReadProducts,
                WriteAddresses,
                WriteCategories,
                WriteProducts
            };
        }

        #endregion IPermissionProvider Members
    }
}