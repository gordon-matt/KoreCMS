using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Navigation;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea("")]
    [RoutePrefix("store")]
    public class StoreController : KoreController
    {
        private readonly Lazy<ICategoryService> categoryService;
        private readonly Lazy<IProductService> productService;
        private readonly StoreSettings storeSettings;

        public StoreController(
            Lazy<ICategoryService> categoryService,
            Lazy<IProductService> productService,
            StoreSettings storeSettings)
        {
            this.categoryService = categoryService;
            this.productService = productService;
            this.storeSettings = storeSettings;
        }

        //[OutputCache(Duration = 600, VaryByParam = "categoryId")]
        [Compress]
        [Route("categories")]
        public ActionResult Categories(int? categoryId = null)
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store));

            string pageIndexParam = Request.Params["pageIndex"];
            int pageIndex = string.IsNullOrEmpty(pageIndexParam)
                ? 1
                : Convert.ToInt32(pageIndexParam);

            var model = categoryService.Value.Repository.Table
                .Where(x => x.ParentId == categoryId)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .Skip((pageIndex - 1) * storeSettings.CategoriesPerPage)
                .Take(storeSettings.CategoriesPerPage)
                .ToList();

            int total = categoryService.Value.Count();

            ViewBag.PageCount = (int)Math.Ceiling((double)total / storeSettings.CategoriesPerPage);
            ViewBag.PageIndex = pageIndex;

            return View("Categories", model);
        }

        [ChildActionOnly]
        [Route("render-filter")]
        public ActionResult Filter()
        {
            // This will only support 2 levels, but for now that's fine. We will support more later.

            var model = categoryService.Value.Repository.Table
                .Where(x => x.ParentId == null)
                .Include(x => x.SubCategories)
                .ToHashSet();

            return PartialView(model);
        }

        //[OutputCache(Duration = 600, VaryByParam = "none")] //TODO: Uncomment when ready
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.Title = T(LocalizableStrings.Store);

            if (storeSettings.UseAjax)
            {
                WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store));
                return View();
            }

            return Categories();
        }

        //[OutputCache(Duration = 600, VaryByParam = "categorySlug;productSlug")]
        [Compress]
        [Route("categories/{categorySlug}/{productSlug}")]
        public ActionResult Product(string categorySlug, string productSlug)
        {
            var category = categoryService.Value.FindOne(x => x.Slug == categorySlug);

            if (category == null)
            {
                return new HttpNotFoundResult();
            }

            var product = productService.Value.FindOne(x => x.Slug == productSlug);

            if (product == null)
            {
                return new HttpNotFoundResult();
            }

            #region Breadcrumbs

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store), Url.Action("Index"));

            var breadcrumbs = new List<Breadcrumb>();
            if (category != null)
            {
                var parentId = category.ParentId;
                while (parentId != null)
                {
                    var parentCategory = categoryService.Value.FindOne(y => y.Id == parentId);

                    if (parentCategory == null)
                    {
                        break;
                    }

                    breadcrumbs.Add(new Breadcrumb
                    {
                        Text = parentCategory.Name,
                        Url = Url.Action("Products", new { categorySlug = parentCategory.Slug })
                    });

                    parentId = parentCategory.ParentId;
                }

                breadcrumbs.Reverse();
            }

            WorkContext.Breadcrumbs.AddRange(breadcrumbs);
            WorkContext.Breadcrumbs.Add(category.Name, Url.Action("Products"));
            WorkContext.Breadcrumbs.Add(product.Name);

            #endregion Breadcrumbs

            ViewBag.MetaKeywords = product.MetaKeywords;
            ViewBag.MetaDescription = product.MetaDescription;

            return View("Product", product);
        }

        //[OutputCache(Duration = 600, VaryByParam = "categorySlug")]
        [Compress]
        [Route("categories/{categorySlug}")]
        public ActionResult Products(string categorySlug)
        {
            var category = categoryService.Value.FindOne(x => x.Slug == categorySlug);

            if (category == null)
            {
                return new HttpNotFoundResult();
            }

            #region Breadcrumbs

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store), Url.Action("Index"));

            var breadcrumbs = new List<Breadcrumb>();
            if (category != null)
            {
                var parentId = category.ParentId;
                while (parentId != null)
                {
                    var parentCategory = categoryService.Value.FindOne(y => y.Id == parentId);

                    if (parentCategory == null)
                    {
                        break;
                    }

                    breadcrumbs.Add(new Breadcrumb
                    {
                        Text = parentCategory.Name,
                        Url = Url.Action("Products", new { categorySlug = parentCategory.Slug })
                    });

                    parentId = parentCategory.ParentId;
                }

                breadcrumbs.Reverse();

                breadcrumbs.Add(new Breadcrumb
                {
                    Text = category.Name
                });
            }

            WorkContext.Breadcrumbs.AddRange(breadcrumbs);

            #endregion Breadcrumbs

            string pageIndexParam = Request.Params["pageIndex"];
            int pageIndex = string.IsNullOrEmpty(pageIndexParam)
                ? 1
                : Convert.ToInt32(pageIndexParam);

            var query = productService.Value.Repository.Table.Where(x => x.CategoryId == category.Id);

            int total = query.Count();

            var model = query
                .OrderBy(x => x.Name)
                .Skip((pageIndex - 1) * storeSettings.ProductsPerPage)
                .Take(storeSettings.ProductsPerPage)
                .ToList();

            ViewBag.CategorySlug = categorySlug;
            ViewBag.PageCount = (int)Math.Ceiling((double)total / storeSettings.ProductsPerPage);
            ViewBag.PageIndex = pageIndex;
            ViewBag.MetaKeywords = category.MetaKeywords;
            ViewBag.MetaDescription = category.MetaDescription;

            return View("Products", model);
        }
    }
}