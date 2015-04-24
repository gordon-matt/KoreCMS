using System;
using System.Web.Http;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class BlogsController : GenericODataController<BlogEntry, Guid>
    {
        public BlogsController(IRepository<BlogEntry> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(BlogEntry entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(BlogEntry entity)
        {
            entity.Id = Guid.NewGuid();
        }
    }
}