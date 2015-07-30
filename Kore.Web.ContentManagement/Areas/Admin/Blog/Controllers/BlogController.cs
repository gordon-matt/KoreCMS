using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Blog)]
    public class BlogController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index(string slug)
        {
            if (!CheckPermission(CmsPermissions.BlogRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Blog.Title));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Blog.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Blog.ManageBlog);

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Blog.Index");
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
    UpdateRecordError: '{9}',
    UpdateRecordSuccess: '{10}',
    Columns: {{
        Category: {{
            Name: '{11}',
        }},
        Post: {{
            Headline: '{12}',
            DateCreatedUtc: '{13}',
        }},
        Tag: {{
            Name: '{14}',
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
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(KoreCmsLocalizableStrings.Blog.CategoryModel.Name),
   T(KoreCmsLocalizableStrings.Blog.PostModel.Headline),
   T(KoreCmsLocalizableStrings.Blog.PostModel.DateCreatedUtc),
   T(KoreCmsLocalizableStrings.Blog.TagModel.Name));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}