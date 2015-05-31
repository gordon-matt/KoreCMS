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

            return View("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Blog.Index");
        }
    }
}