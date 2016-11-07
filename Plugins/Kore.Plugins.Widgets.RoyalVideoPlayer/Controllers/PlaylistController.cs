using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    public class PlaylistController : KoreController
    {
        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(Permissions.Read))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Plugins.Title));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.RoyalVideoPlayer));

            ViewBag.Title = T(LocalizableStrings.RoyalVideoPlayer);

            return PartialView();
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Create = T(KoreWebLocalizableStrings.General.Create).Text,
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                InsertRecordError = T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
                InsertRecordSuccess = T(KoreWebLocalizableStrings.General.InsertRecordSuccess).Text,
                Playlists = T(LocalizableStrings.Playlists).Text,
                SavePlaylistsError = T(LocalizableStrings.SavePlaylistsError).Text,
                SavePlaylistsSuccess = T(LocalizableStrings.SavePlaylistsSuccess).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Playlist = new
                    {
                        Name = T(LocalizableStrings.Models.Playlist.Name).Text,
                    },
                    Video = new
                    {
                        ThumbnailUrl = T(LocalizableStrings.Models.Video.ThumbnailUrl).Text,
                        Title = T(LocalizableStrings.Models.Video.Title).Text
                    }
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}