using Kore.Data;
using Kore.Web.Common.Areas.Admin.Regions.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Common.Areas.Admin.Regions.Controllers.Api
{
    public class RegionApiController : GenericODataController<Region, int>
    {
        public RegionApiController(IRepository<Region> repository)
            : base(repository)
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