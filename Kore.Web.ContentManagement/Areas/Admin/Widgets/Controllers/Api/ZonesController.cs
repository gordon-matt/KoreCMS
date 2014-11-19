using System;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Controllers.Api
{
    public class ZonesController : GenericODataController<Zone, Guid>
    {
        public ZonesController(IRepository<Zone> repository)
            : base(repository)
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
    }
}