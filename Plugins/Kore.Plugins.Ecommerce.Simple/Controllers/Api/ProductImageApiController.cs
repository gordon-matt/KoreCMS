using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    public class ProductImageApiController : GenericODataController<ProductImage, int>
    {
        public ProductImageApiController(IRepository<ProductImage> repository)
            : base(repository)
        {
        }

        protected override int GetId(ProductImage entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(ProductImage entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return SimpleCommercePermissions.ReadProducts; }
        }

        protected override Permission WritePermission
        {
            get { return SimpleCommercePermissions.WriteProducts; }
        }
    }
}