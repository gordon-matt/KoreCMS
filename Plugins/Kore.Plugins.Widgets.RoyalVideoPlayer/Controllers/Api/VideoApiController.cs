using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Controllers.Api
{
    public class VideoApiController : GenericODataController<Video, int>
    {
        public VideoApiController(IVideoService service)
            : base(service)
        {
        }

        protected override int GetId(Video entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Video entity)
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