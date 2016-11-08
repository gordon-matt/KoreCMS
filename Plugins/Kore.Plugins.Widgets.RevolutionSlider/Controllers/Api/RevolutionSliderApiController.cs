using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;
using Kore.Plugins.Widgets.RevolutionSlider.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using Slider = Kore.Plugins.Widgets.RevolutionSlider.Data.Domain.RevolutionSlider;

namespace Kore.Plugins.Widgets.RevolutionSlider.Controllers.Api
{
    public class RevolutionSliderApiController : GenericTenantODataController<Slider, int>
    {
        public RevolutionSliderApiController(ISliderService service)
            : base(service)
        {
        }

        protected override int GetId(Slider entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Slider entity)
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