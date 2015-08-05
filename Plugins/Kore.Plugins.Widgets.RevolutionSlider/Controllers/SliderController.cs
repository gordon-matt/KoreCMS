using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Plugins.Widgets.RevolutionSlider.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    public class SliderController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(Permissions.Read))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Plugins.Title));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.RevolutionSlider));

            ViewBag.Title = T(LocalizableStrings.RevolutionSlider);

            return PartialView();
        }

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
    Layers: '{9}',
    Sliders: '{10}',
    Slides: '{11}',
    UpdateRecordError: '{12}',
    UpdateRecordSuccess: '{13}',
    Columns: {{
        Layer: {{
            Start: '{14}',
            Speed: '{15}',
            CaptionText: '{16}',
            X: '{17}',
            Y: '{18}'
        }},
        Slider: {{
            Name: '{19}'
        }},
        Slide: {{
            Order: '{20}',
            Title: '{21}',
            Link: '{22}',
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
   T(LocalizableStrings.Layers),
   T(LocalizableStrings.Sliders),
   T(LocalizableStrings.Slides),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(LocalizableStrings.Models.Layer.Start),
   T(LocalizableStrings.Models.Layer.Speed),
   T(LocalizableStrings.Models.Layer.CaptionText),
   T(LocalizableStrings.Models.Layer.X),
   T(LocalizableStrings.Models.Layer.Y),
   T(LocalizableStrings.Models.Slider.Name),
   T(LocalizableStrings.Models.Slide.Order),
   T(LocalizableStrings.Models.Slide.Title),
   T(LocalizableStrings.Models.Slide.Link));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}