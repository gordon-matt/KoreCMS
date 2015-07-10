using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers.Api
{
    public class BlogCategoryApiController : GenericODataController<BlogCategory, int>
    {
        public BlogCategoryApiController(IBlogCategoryService service)
            : base(service)
        {
        }

        protected override int GetId(BlogCategory entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(BlogCategory entity)
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