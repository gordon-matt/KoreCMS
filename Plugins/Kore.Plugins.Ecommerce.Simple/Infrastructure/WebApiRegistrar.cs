using System.Web.Http;
using System.Web.OData.Builder;
using Kore.Plugins.Ecommerce.Simple.Controllers.Api;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Infrastructure;
using System.Web.OData.Extensions;

namespace Kore.Plugins.Ecommerce.Simple.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            //builder.EntitySet<ShoppingCartItem>("CartApi");
            builder.EntitySet<SimpleCommerceAddress>("SimpleCommerceAddressApi");
            builder.EntitySet<SimpleCommerceCategory>("SimpleCommerceCategoryApi");
            builder.EntitySet<SimpleCommerceOrder>("SimpleCommerceOrderApi");
            builder.EntitySet<SimpleCommerceOrderLine>("SimpleCommerceOrderLineApi");
            builder.EntitySet<SimpleCommerceOrderNote>("SimpleCommerceOrderNoteApi");
            builder.EntitySet<SimpleCommerceProduct>("SimpleCommerceProductApi");

            // Special
            builder.EntitySet<CategoryTreeItem>("SimpleCommerceCategoryTreeApi");

            config.MapODataServiceRoute("OData_Kore_Plugin_SimpleCommerce", "odata/kore/plugins/simple-commerce", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}