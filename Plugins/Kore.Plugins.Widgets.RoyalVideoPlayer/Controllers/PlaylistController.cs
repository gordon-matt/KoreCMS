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
            string json = string.Format(
@"{{
    Create: '{0}',
    Delete: '{1}',
    DeleteRecordConfirm: '{2}',
    DeleteRecordError: '{3}',
    DeleteRecordSuccess: '{4}',
    Edit: '{5}',
    GetRecordError: '{6}',
    InsertRecordError: '{7}',
    InsertRecordSuccess: '{8}',
    Playlists: '{9}',
    SavePlaylistsError: '{10}',
    SavePlaylistsSuccess: '{11}',
    UpdateRecordError: '{12}',
    UpdateRecordSuccess: '{13}',
    Columns: {{
        Playlist: {{
            Name: '{14}'
        }},
        Video: {{
            ThumbnailUrl: '{15}',
            Title: '{16}'
        }}
    }}
}}",
   T(KoreWebLocalizableStrings.General.Create),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.Edit),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(LocalizableStrings.Playlists),
   T(LocalizableStrings.SavePlaylistsError),
   T(LocalizableStrings.SavePlaylistsSuccess),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(LocalizableStrings.Models.Playlist.Name),
   T(LocalizableStrings.Models.Video.ThumbnailUrl),
   T(LocalizableStrings.Models.Video.Title));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}