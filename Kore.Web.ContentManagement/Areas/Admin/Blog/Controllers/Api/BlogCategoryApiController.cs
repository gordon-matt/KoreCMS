using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers.Api
{
    public class BlogCategoryApiController : GenericODataController<Category, int>
    {
        public BlogCategoryApiController(ICategoryService service)
            : base(service)
        {
        }

        protected override int GetId(Category entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Category entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.BlogRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.BlogWrite; }
        }
    }
}