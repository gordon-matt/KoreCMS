using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Blog)]
    public class BlogController : KoreController
    {
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