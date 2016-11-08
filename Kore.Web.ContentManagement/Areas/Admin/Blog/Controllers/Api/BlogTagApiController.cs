﻿using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers.Api
{
    public class BlogTagApiController : GenericTenantODataController<BlogTag, int>
    {
        public BlogTagApiController(IBlogTagService service)
            : base(service)
        {
        }

        protected override int GetId(BlogTag entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(BlogTag entity)
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