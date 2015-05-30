using Kore.Web.Common.Areas.Admin.Regions.Domain;
using Kore.Web.Common.Areas.Admin.Regions.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Common.Areas.Admin.Regions.Controllers.Api
{
    public class RegionApiController : GenericODataController<Region, int>
    {
        public RegionApiController(IRegionService service)
            : base(service)
        {
        }

        protected override int GetId(Region entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Region entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return Permissions.RegionsRead; }
        }

        protected override Permission WritePermission
        {
            get { return Permissions.RegionsWrite; }
        }
    }
}