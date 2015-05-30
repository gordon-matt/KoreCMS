using System;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class ZoneApiController : GenericODataController<Zone, Guid>
    {
        public ZoneApiController(IZoneService service)
            : base(service)
        {
        }

        protected override Guid GetId(Zone entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Zone entity)
        {
            entity.Id = Guid.NewGuid();
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.ContentZonesRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.ContentZonesWrite; }
        }
    }
}