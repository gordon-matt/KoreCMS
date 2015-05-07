using System.Web.Http;
using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Http.OData;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class ProductsController : GenericODataController<Product, int>
    {
        public ProductsController(IRepository<Product> repository)
            : base(repository)
        {
        }

        protected override int GetId(Product entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Product entity)
        {
        }
    }
}