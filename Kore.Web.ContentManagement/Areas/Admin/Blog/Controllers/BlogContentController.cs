using System.Data.Entity;
using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Data;
using Kore.EntityFramework;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers
{
    [RouteArea("")]
    [RoutePrefix("blog")]
    public class BlogContentController : KoreController
    {
        private readonly BlogSettings blogSettings;
        private readonly Lazy<IBlogPostService> postService;
        private readonly Lazy<IBlogCategoryService> categoryService;
        private readonly Lazy<IBlogTagService> tagService;
        private readonly Lazy<IMembershipService> membershipService;

        public BlogContentController(
            BlogSettings blogSettings,
            Lazy<IBlogPostService> postService,
            Lazy<IBlogCategoryService> categoryService,
            Lazy<IBlogTagService> tagService,
            Lazy<IMembershipService> membershipService)
        {
            this.blogSettings = blogSettings;
            this.categoryService = categoryService;
            this.postService = postService;
            this.tagService = tagService;
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

            if (blogSettings.UseAjax)
            {
                return PostsAjax();
            }
            else
            {
                string pageIndexParam = Request.Params["pageIndex"];
                int pageIndex = string.IsNullOrEmpty(pageIndexParam)
                    ? 1
                    : Convert.ToInt32(pageIndexParam);

                var model = postService.Value.Query()
                    .OrderByDescending(x => x.DateCreatedUtc)
                    .Skip((pageIndex - 1) * blogSettings.ItemsPerPage)
                    .Take(blogSettings.ItemsPerPage)
                    .ToList();

                return Posts(pageIndex, model);
            }
        }

        [Compress]
        [Route("category/{categorySlug}")]
        public ActionResult Category(string categorySlug)
        {
            // If there are access restrictions
            if (!PageSecurityHelper.CheckUserHasAccessToBlog(User))
            {
                return new HttpUnauthorizedResult();
            }

            var category = categoryService.Value.FindOne(x => x.UrlSlug == categorySlug);

            if (category == null)
            {
                throw new EntityNotFoundException(string.Concat(
                    "Could not find a blog category with slug, '", categorySlug, "'"));
            }

            if (blogSettings.UseAjax)
            {
                ViewBag.CategoryId = category.Id;
                return PostsAjax();
            }
            else
            {
                string pageIndexParam = Request.Params["pageIndex"];
                int pageIndex = string.IsNullOrEmpty(pageIndexParam)
                    ? 1
                    : Convert.ToInt32(pageIndexParam);

                var model = postService.Value.Query()
                    .Include(x => x.Tags)
                    .Where(x => x.CategoryId == category.Id)
                    .OrderByDescending(x => x.DateCreatedUtc)
                    .Skip((pageIndex - 1) * blogSettings.ItemsPerPage)
                    .Take(blogSettings.ItemsPerPage)
                    .ToList();

                return Posts(pageIndex, model);
            }
        }

        [Compress]
        [Route("tag/{tagSlug}")]
        public ActionResult Tag(string tagSlug)
        {
            var tag = tagService.Value.FindOne(x => x.UrlSlug == tagSlug);

            if (tag == null)
            {
                throw new EntityNotFoundException(string.Concat(
                    "Could not find a blog tag with slug, '", tagSlug, "'"));
            }

            if (blogSettings.UseAjax)
            {
                ViewBag.TagId = tag.Id;
                return PostsAjax();
            }
            else
            {
                string pageIndexParam = Request.Params["pageIndex"];
                int pageIndex = string.IsNullOrEmpty(pageIndexParam)
                    ? 1
                    : Convert.ToInt32(pageIndexParam);

                var model = postService.Value.Query()
                    .Include(x => x.Tags)
                    .Where(x => x.Tags.Any(y => y.TagId == tag.Id))
                    .OrderByDescending(x => x.DateCreatedUtc)
                    .Skip((pageIndex - 1) * blogSettings.ItemsPerPage)
                    .Take(blogSettings.ItemsPerPage)
                    .ToList();

                return Posts(pageIndex, model);
            }
        }

        private ActionResult Posts(int pageIndex, IEnumerable<BlogPost> model)
        {
            bool isChildAction = ControllerContext.IsChildAction;
            ViewBag.IsChildAction = isChildAction;

            var userNames = model
                .Select(x => x.UserId)
                .Distinct()
                .ToDictionary(
                    k => k,
                    v => membershipService.Value.GetUserById(v).UserName);

            int total = postService.Value.Count();

            ViewBag.PageCount = (int)Math.Ceiling((double)total / blogSettings.ItemsPerPage);
            ViewBag.PageIndex = pageIndex;
            ViewBag.UserNames = userNames;

            var tags = tagService.Value.Find();

            ViewBag.Tags = tags.ToDictionary(k => k.Id, v => v.Name);
            ViewBag.TagUrls = tags.ToDictionary(k => k.Id, v => v.UrlSlug);

            var viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, "Index", null);

            // If someone has provided a custom template (see LocationFormatProvider)
            if (viewEngineResult.View != null)
            {
                if (isChildAction)
                {
                    return PartialView("Index", model);
                }
                return View("Index", model);
            }

            // Else use default template
            if (isChildAction)
            {
                return PartialView("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.Index", model);
            }
            return View("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.Index", model);
        }

        private ActionResult PostsAjax()
        {
            bool isChildAction = ControllerContext.IsChildAction;
            ViewBag.IsChildAction = isChildAction;

            var viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, "IndexAjax", null);

            // If someone has provided a custom template (see LocationFormatProvider)
            if (viewEngineResult.View != null)
            {
                if (isChildAction)
                {
                    return PartialView("IndexAjax");
                }
                return View("IndexAjax");
            }

            // Else use default template
            if (isChildAction)
            {
                return PartialView("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.IndexAjax");
            }
            return View("Kore.Web.ContentManagement.Areas.Admin.Blog.Views.BlogContent.IndexAjax");
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

            var model = postService.Value.FindOne(x => x.Slug == slug);

            string previousEntrySlug = null;
            string nextEntrySlug = null;

            bool hasPreviousEntry = postService.Value.Query().Any(x => x.DateCreatedUtc < model.DateCreatedUtc);
            if (hasPreviousEntry)
            {
                var previousEntryDate = postService.Value.Query(x => x.DateCreatedUtc < model.DateCreatedUtc)
                    .Select(x => x.DateCreatedUtc)
                    .Max();

                previousEntrySlug = postService.Value.FindOne(x => x.DateCreatedUtc == previousEntryDate).Slug;
            }

            bool hasNextEntry = postService.Value.Query().Any(x => x.DateCreatedUtc > model.DateCreatedUtc);
            if (hasNextEntry)
            {
                var nextEntryDate = postService.Value.Query(x => x.DateCreatedUtc > model.DateCreatedUtc)
                    .Select(x => x.DateCreatedUtc)
                    .Min();

                nextEntrySlug = postService.Value.FindOne(x => x.DateCreatedUtc == nextEntryDate).Slug;
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