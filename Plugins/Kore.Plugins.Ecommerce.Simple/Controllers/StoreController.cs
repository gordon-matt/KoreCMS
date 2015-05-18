using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Mvc;
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

            #endregion

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

            return View("Products", model);
        }

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

            #endregion

            return View("Product", product);
        }
    }
}