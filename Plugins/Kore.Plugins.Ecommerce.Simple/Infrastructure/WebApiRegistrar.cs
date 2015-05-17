using System.Web.Http;
using System.Web.Http.OData.Builder;
using Kore.Plugins.Ecommerce.Simple.Controllers.Api;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Models;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Ecommerce.Simple.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            //builder.EntitySet<ShoppingCartItem>("CartApi");
            builder.EntitySet<Category>("CategoryApi");
            builder.EntitySet<Order>("OrderApi");
            builder.EntitySet<Product>("ProductApi");

            // Special
            builder.EntitySet<CategoryTreeItem>("CategoryTreeApi");

            config.Routes.MapODataRoute("OData_Kore_Plugin_SimpleCommerce", "odata/kore/plugins/simple-commerce", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}