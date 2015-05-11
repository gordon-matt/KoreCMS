using System;
using System.Web.Http;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class PageTypeApiController : GenericODataController<PageType, Guid>
    {
        public PageTypeApiController(IRepository<PageType> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(PageType entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(PageType entity)
        {
            entity.Id = Guid.NewGuid();
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.PageTypesRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.PageTypesWrite; }
        }
    }
}