using System;
using System.Web.Http;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    public class PageApiController : GenericTenantODataController<Page, Guid>
    {
        private readonly IPageVersionService pageVersionService;
        private readonly IWebWorkContext workContext;

        public PageApiController(
            IPageService service,
            IPageVersionService pageVersionService,
            IWebWorkContext workContext)
            : base(service)
        {
            this.pageVersionService = pageVersionService;
            this.workContext = workContext;
        }

        protected override Guid GetId(Page entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Page entity)
        {
            entity.Id = Guid.NewGuid();
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.PagesRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.PagesWrite; }
        }

        [HttpGet]
        public IHttpActionResult GetTopLevelPages()
        {
            if (!CheckPermission(ReadPermission))
            {
                return Unauthorized();
            }

            int tenantId = GetTenantId();
            var topLevelPages = (Service as IPageService).GetTopLevelPages(tenantId);

            return Ok(topLevelPages);
        }
    }
}