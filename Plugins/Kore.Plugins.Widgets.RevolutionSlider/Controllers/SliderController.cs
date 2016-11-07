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
        [OutputCache(Duration = 86400, VaryByParam = "none")]
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
                Layers = T(LocalizableStrings.Layers).Text,
                Sliders = T(LocalizableStrings.Sliders).Text,
                Slides = T(LocalizableStrings.Slides).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Layer = new
                    {
                        Start = T(LocalizableStrings.Models.Layer.Start).Text,
                        Speed = T(LocalizableStrings.Models.Layer.Speed).Text,
                        CaptionText = T(LocalizableStrings.Models.Layer.CaptionText).Text,
                        X = T(LocalizableStrings.Models.Layer.X).Text,
                        Y = T(LocalizableStrings.Models.Layer.Y).Text,
                    },
                    Slider = new
                    {
                        Name = T(LocalizableStrings.Models.Slider.Name).Text,
                    },
                    Slide = new
                    {
                        Order = T(LocalizableStrings.Models.Slide.Order).Text,
                        Title = T(LocalizableStrings.Models.Slide.Title).Text,
                        Link = T(LocalizableStrings.Models.Slide.Link).Text,
                    }
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}