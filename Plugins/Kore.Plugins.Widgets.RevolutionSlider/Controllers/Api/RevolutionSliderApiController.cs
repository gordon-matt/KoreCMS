using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;
using Kore.Plugins.Widgets.RevolutionSlider.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.RevolutionSlider.Controllers.Api
{
    public class RevolutionSliderApiController : GenericODataController<RevolutionSlider, int>
    {
        public RevolutionSliderApiController(ISliderService service)
            : base(service)
        {
        }

        protected override int GetId(RevolutionSlider entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(RevolutionSlider entity)
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