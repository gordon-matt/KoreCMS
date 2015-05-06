using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Data;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers
{
    [RouteArea("")]
    [RoutePrefix("blog")]
    public class BlogContentController : KoreController
    {
        private readonly BlogSettings blogSettings;
        private readonly Lazy<IRepository<BlogEntry>> blogRepository;
        private readonly Lazy<IMembershipService> membershipService;

        public BlogContentController(
            BlogSettings blogSettings,
            Lazy<IRepository<BlogEntry>> blogRepository,
            Lazy<IMembershipService> membershipService)
        {
            this.blogSettings = blogSettings;
            this.blogRepository = blogRepository;
            this.membershipService = membershipService;
        }

        [Route("")]
        public ActionResult Index()
        {
            // If there are access restrictions
            if (!PageSecurityHelper.CheckUserHasAccessToBlog(User))
            {
                return new HttpUnauthorizedResult();
            }

            // If Use Ajax
            if (blogSettings.UseAjax)
            {
                var viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, "Index", null);

                // If someone has provided a custom template (see LocationFormatProvider)
                if (viewEngineResult.View != null)
                {
                    return View();
                }

                // Else use default template
                return View("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.IndexAjax");
            }
            else
            {
                string pageIndexParam = Request.Params["pageIndex"];
                int pageIndex = string.IsNullOrEmpty(pageIndexParam)
                    ? 1
                    : Convert.ToInt32(pageIndexParam);

                var model = blogRepository.Value.Table
                    .OrderByDescending(x => x.DateCreated)
                    .Skip((pageIndex - 1) * blogSettings.ItemsPerPage)
                    .Take(blogSettings.ItemsPerPage)
                    .ToList();

                var userNames = model
                    .Select(x => x.UserId)
                    .Distinct()
                    .ToDictionary(
                        k => k,
                        v => membershipService.Value.GetUserById(v).UserName);

                int total = blogRepository.Value.Count();

                ViewBag.PageCount = (int)Math.Ceiling((double)total / blogSettings.ItemsPerPage);
                ViewBag.PageIndex = pageIndex;
                ViewBag.UserNames = userNames;

                var viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, "Index", null);

                // If someone has provided a custom template (see LocationFormatProvider)
                if (viewEngineResult.View != null)
                {
                    return View(model);
                }

                // Else use default template
                return View("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.Index", model);
            }
        }

        [Route("{slug}")]
        public ActionResult Details(string slug)
        {
            // If there are access restrictions
            if (!PageSecurityHelper.CheckUserHasAccessToBlog(User))
            {
                return new HttpUnauthorizedResult();
            }

            var model = blogRepository.Value.Table.FirstOrDefault(x => x.Slug == slug);

            string previousEntrySlug = null;
            string nextEntrySlug = null;

            bool hasPreviousEntry = blogRepository.Value.Table.Any(x => x.DateCreated < model.DateCreated);
            if (hasPreviousEntry)
            {
                var previousEntryDate = blogRepository.Value.Table
                    .Where(x => x.DateCreated < model.DateCreated)
                    .Select(x => x.DateCreated)
                    .Max();

                previousEntrySlug = blogRepository.Value.Table.First(x => x.DateCreated == previousEntryDate).Slug;
            }

            bool hasNextEntry = blogRepository.Value.Table.Any(x => x.DateCreated > model.DateCreated);
            if (hasNextEntry)
            {
                var nextEntryDate = blogRepository.Value.Table
                    .Where(x => x.DateCreated > model.DateCreated)
                    .Select(x => x.DateCreated)
                    .Min();

                nextEntrySlug = blogRepository.Value.Table.First(x => x.DateCreated == nextEntryDate).Slug;
            }

            ViewBag.PreviousEntrySlug = previousEntrySlug;
            ViewBag.NextEntrySlug = nextEntrySlug;
            ViewBag.UserName = membershipService.Value.GetUserById(model.UserId).UserName;
            return View("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.Details", model);
        }
    }
}