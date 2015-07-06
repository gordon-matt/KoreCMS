using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;
using Kore.Plugins.Widgets.RevolutionSlider.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.RevolutionSlider.Controllers.Api
{
    public class RevolutionSlideApiController : GenericODataController<Slide, int>
    {
        public RevolutionSlideApiController(ISlideService service)
            : base(service)
        {
        }

        protected override int GetId(Slide entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Slide entity)
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