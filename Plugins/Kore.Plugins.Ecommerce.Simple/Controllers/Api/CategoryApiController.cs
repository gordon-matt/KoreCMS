using System.Web.Http;
using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Http.OData;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class CategoryApiController : GenericODataController<Category, int>
    {
        public CategoryApiController(IRepository<Category> repository)
            : base(repository)
        {
        }

        protected override int GetId(Category entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Category entity)
        {
        }
    }
}