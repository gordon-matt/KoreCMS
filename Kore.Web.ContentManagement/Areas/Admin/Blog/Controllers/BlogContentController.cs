﻿using System.Data.Entity;
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
using Kore.Exceptions;
using System.Threading.Tasks;

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
        public async Task<ActionResult> Index()
        {
            // If there are access restrictions
            if (!await PageSecurityHelper.CheckUserHasAccessToBlog(User))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Blog.Title));

            if (blogSettings.UseAjax)
            {
                return PostsAjax();
            }
            else
            {
                int tenantId = WorkContext.CurrentTenant.Id;

                string pageIndexParam = Request.Params["pageIndex"];
                int pageIndex = string.IsNullOrEmpty(pageIndexParam)
                    ? 1
                    : Convert.ToInt32(pageIndexParam);

                List<BlogPost> model = null;
                using (var connection = postService.Value.OpenConnection())
                {
                    model = await connection.Query(x => x.TenantId == tenantId)
                        .Include(x => x.Category)
                        .Include(x => x.Tags)
                        .OrderByDescending(x => x.DateCreatedUtc)
                        .Skip((pageIndex - 1) * blogSettings.ItemsPerPage)
                        .Take(blogSettings.ItemsPerPage)
                        .ToListAsync();
                }

                return await Posts(pageIndex, model);
            }
        }

        [Compress]
        [Route("category/{categorySlug}")]
        public async Task<ActionResult> Category(string categorySlug)
        {
            // If there are access restrictions
            if (!await PageSecurityHelper.CheckUserHasAccessToBlog(User))
            {
                return new HttpUnauthorizedResult();
            }

            int tenantId = WorkContext.CurrentTenant.Id;
            var category = await categoryService.Value.FindOneAsync(x => x.TenantId == tenantId && x.UrlSlug == categorySlug);

            if (category == null)
            {
                throw new EntityNotFoundException(string.Concat(
                    "Could not find a blog category with slug, '", categorySlug, "'"));
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Blog.Title), Url.Action("Index"));
            WorkContext.Breadcrumbs.Add(category.Name);

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

                List<BlogPost> model = null;
                using (var connection = postService.Value.OpenConnection())
                {
                    model = await connection.Query()
                        .Include(x => x.Category)
                        .Include(x => x.Tags)
                        .Where(x => x.CategoryId == category.Id)
                        .OrderByDescending(x => x.DateCreatedUtc)
                        .Skip((pageIndex - 1) * blogSettings.ItemsPerPage)
                        .Take(blogSettings.ItemsPerPage)
                        .ToListAsync();
                }

                return await Posts(pageIndex, model);
            }
        }

        [Compress]
        [Route("tag/{tagSlug}")]
        public async Task<ActionResult> Tag(string tagSlug)
        {
            int tenantId = WorkContext.CurrentTenant.Id;

            var tag = await tagService.Value.FindOneAsync(x =>
                x.TenantId == tenantId
                && x.UrlSlug == tagSlug);

            if (tag == null)
            {
                throw new EntityNotFoundException(string.Concat(
                    "Could not find a blog tag with slug, '", tagSlug, "'"));
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Blog.Title), Url.Action("Index"));
            WorkContext.Breadcrumbs.Add(tag.Name);

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

                List<BlogPost> model = null;
                using (var connection = postService.Value.OpenConnection())
                {
                    model = await connection.Query()
                        .Include(x => x.Category)
                        .Include(x => x.Tags)
                        .Where(x => x.Tags.Any(y => y.TagId == tag.Id))
                        .OrderByDescending(x => x.DateCreatedUtc)
                        .Skip((pageIndex - 1) * blogSettings.ItemsPerPage)
                        .Take(blogSettings.ItemsPerPage)
                        .ToListAsync();
                }

                return await Posts(pageIndex, model);
            }
        }

        private async Task<ActionResult> Posts(int pageIndex, IEnumerable<BlogPost> model)
        {
            bool isChildAction = ControllerContext.IsChildAction;
            ViewBag.IsChildAction = isChildAction;

            var userIds = model.Select(x => x.UserId).Distinct();
            var userNames = (await membershipService.Value.GetUsers(WorkContext.CurrentTenant.Id, x => userIds.Contains(x.Id))).ToDictionary(k => k.Id, v => v.UserName);

            int total = await postService.Value.CountAsync();

            ViewBag.PageCount = (int)Math.Ceiling((double)total / blogSettings.ItemsPerPage);
            ViewBag.PageIndex = pageIndex;
            ViewBag.UserNames = userNames;

            var tags = await tagService.Value.FindAsync();
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
        public async Task<ActionResult> Details(string slug)
        {
            // If there are access restrictions
            if (!await PageSecurityHelper.CheckUserHasAccessToBlog(User))
            {
                return new HttpUnauthorizedResult();
            }

            int tenantId = WorkContext.CurrentTenant.Id;

            BlogPost model = null;
            DateTime? previousEntryDate = null;
            DateTime? nextEntryDate = null;
            using (var connection = postService.Value.OpenConnection())
            {
                model = await connection
                    .Query(x => x.TenantId == tenantId && x.Slug == slug)
                    .Include(x => x.Category)
                    .Include(x => x.Tags)
                    .FirstOrDefaultAsync();

                if (model == null)
                {
                    throw new KoreException("Blog post not found!");
                }

                bool hasPreviousEntry = await connection.Query(x => x.TenantId == tenantId).AnyAsync(x => x.DateCreatedUtc < model.DateCreatedUtc);
                if (hasPreviousEntry)
                {
                    previousEntryDate = await connection.Query(x => x.TenantId == tenantId && x.DateCreatedUtc < model.DateCreatedUtc)
                        .Select(x => x.DateCreatedUtc)
                        .MaxAsync();
                }

                bool hasNextEntry = await connection.Query(x => x.TenantId == tenantId).AnyAsync(x => x.DateCreatedUtc > model.DateCreatedUtc);
                if (hasNextEntry)
                {
                    nextEntryDate = await connection.Query(x => x.TenantId == tenantId && x.DateCreatedUtc > model.DateCreatedUtc)
                        .Select(x => x.DateCreatedUtc)
                        .MinAsync();
                }
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Blog.Title), Url.Action("Index"));
            WorkContext.Breadcrumbs.Add(model.Headline);

            ViewBag.PreviousEntrySlug = previousEntryDate.HasValue
                ? (await postService.Value.FindOneAsync(x => x.TenantId == tenantId && x.DateCreatedUtc == previousEntryDate)).Slug
                : null;

            ViewBag.NextEntrySlug = nextEntryDate.HasValue
                ? (await postService.Value.FindOneAsync(x => x.TenantId == tenantId && x.DateCreatedUtc == nextEntryDate)).Slug
                : null;

            ViewBag.UserName = (await membershipService.Value.GetUserById(model.UserId)).UserName;

            var tags = await tagService.Value.FindAsync(x => x.TenantId == tenantId);
            ViewBag.Tags = tags.ToDictionary(k => k.Id, v => v.Name);
            ViewBag.TagUrls = tags.ToDictionary(k => k.Id, v => v.UrlSlug);

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