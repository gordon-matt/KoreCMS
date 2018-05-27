using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Blog)]
    public class BlogController : KoreController
    {
        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
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
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Category = new
                    {
                        Name = T(KoreCmsLocalizableStrings.Blog.CategoryModel.Name).Text,
                    },
                    Post = new
                    {
                        Headline = T(KoreCmsLocalizableStrings.Blog.PostModel.Headline).Text,
                        DateCreatedUtc = T(KoreCmsLocalizableStrings.Blog.PostModel.DateCreatedUtc).Text,
                    },
                    Tag = new
                    {
                        Name = T(KoreCmsLocalizableStrings.Blog.TagModel.Name).Text,
                    }
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}