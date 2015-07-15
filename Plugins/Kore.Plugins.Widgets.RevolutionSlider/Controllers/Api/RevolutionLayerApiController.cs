using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;
using Kore.Plugins.Widgets.RevolutionSlider.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.RevolutionSlider.Controllers.Api
{
    public class RevolutionLayerApiController : GenericODataController<RevolutionLayer, int>
    {
        public RevolutionLayerApiController(ILayerService service)
            : base(service)
        {
        }

        protected override int GetId(RevolutionLayer entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(RevolutionLayer entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return Permissions.Read; }
        }

        protected override Permission WritePermission
        {
            get { return Permissions.Write; }
        }
    }
}