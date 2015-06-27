using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Data;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

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

        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            // If there are access restrictions
            if (!PageSecurityHelper.CheckUserHasAccessToBlog(User))
            {
                return new HttpUnauthorizedResult();
            }

            bool isChildAction = ControllerContext.IsChildAction;
            ViewBag.IsChildAction = isChildAction;

            // If Use Ajax
            if (blogSettings.UseAjax)
            {
                var viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, "Index", null);

                // If someone has provided a custom template (see LocationFormatProvider)
                if (viewEngineResult.View != null)
                {
                    if (isChildAction)
                    {
                        return PartialView();
                    }
                    return View();
                }

                // Else use default template
                if (isChildAction)
                {
                    return PartialView("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.IndexAjax");
                }
                return View("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.IndexAjax");
            }
            else
            {
                string pageIndexParam = Request.Params["pageIndex"];
                int pageIndex = string.IsNullOrEmpty(pageIndexParam)
                    ? 1
                    : Convert.ToInt32(pageIndexParam);

                var model = blogRepository.Value.Table
                    .OrderByDescending(x => x.DateCreatedUtc)
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
                    if (isChildAction)
                    {
                        return PartialView();
                    }
                    return View(model);
                }

                // Else use default template
                if (isChildAction)
                {
                    return PartialView("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.Index", model);
                }
                return View("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.Index", model);
            }
        }

        [Compress]
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

            bool hasPreviousEntry = blogRepository.Value.Table.Any(x => x.DateCreatedUtc < model.DateCreatedUtc);
            if (hasPreviousEntry)
            {
                var previousEntryDate = blogRepository.Value.Table
                    .Where(x => x.DateCreatedUtc < model.DateCreatedUtc)
                    .Select(x => x.DateCreatedUtc)
                    .Max();

                previousEntrySlug = blogRepository.Value.Table.First(x => x.DateCreatedUtc == previousEntryDate).Slug;
            }

            bool hasNextEntry = blogRepository.Value.Table.Any(x => x.DateCreatedUtc > model.DateCreatedUtc);
            if (hasNextEntry)
            {
                var nextEntryDate = blogRepository.Value.Table
                    .Where(x => x.DateCreatedUtc > model.DateCreatedUtc)
                    .Select(x => x.DateCreatedUtc)
                    .Min();

                nextEntrySlug = blogRepository.Value.Table.First(x => x.DateCreatedUtc == nextEntryDate).Slug;
            }

            ViewBag.PreviousEntrySlug = previousEntrySlug;
            ViewBag.NextEntrySlug = nextEntrySlug;
            ViewBag.UserName = membershipService.Value.GetUserById(model.UserId).UserName;

            var viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, "Details", null);

            // If someone has provided a custom template (see LocationFormatProvider)
            if (viewEngineResult.View != null)
            {
                return View(model);
            }

            // Else use default template
            return View("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.Details", model);
        }
    }
}