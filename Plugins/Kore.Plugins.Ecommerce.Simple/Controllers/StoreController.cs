using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea("")]
    [RoutePrefix("store")]
    public class StoreController : KoreController
    {
        private readonly Lazy<IRepository<Category>> categoryRepository;
        private readonly Lazy<IRepository<Product>> productRepository;
        private readonly StoreSettings storeSettings;

        public StoreController(
            Lazy<IRepository<Category>> categoryRepository,
            Lazy<IRepository<Product>> productRepository,
            StoreSettings storeSettings)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
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
        public ActionResult Categories()
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store));

            string pageIndexParam = Request.Params["pageIndex"];
            int pageIndex = string.IsNullOrEmpty(pageIndexParam)
                ? 1
                : Convert.ToInt32(pageIndexParam);

            var model = categoryRepository.Value.Table
                .OrderBy(x => x.Name)
                .Skip((pageIndex - 1) * storeSettings.CategoriesPerPage)
                .Take(storeSettings.CategoriesPerPage)
                .ToList();

            int total = categoryRepository.Value.Count();

            ViewBag.PageCount = (int)Math.Ceiling((double)total / storeSettings.CategoriesPerPage);
            ViewBag.PageIndex = pageIndex;

            return View("Categories", model);
        }

        [Route("categories/{categorySlug}")]
        public ActionResult Products(string categorySlug)
        {
            var category = categoryRepository.Value.Table.FirstOrDefault(x => x.Slug == categorySlug);

            if (category == null)
            {
                return new HttpNotFoundResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store), Url.Action("Index"));
            WorkContext.Breadcrumbs.Add(category.Name);

            string pageIndexParam = Request.Params["pageIndex"];
            int pageIndex = string.IsNullOrEmpty(pageIndexParam)
                ? 1
                : Convert.ToInt32(pageIndexParam);

            var query = productRepository.Value.Table
                .Where(x => x.CategoryId == category.Id);

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
            var category = categoryRepository.Value.Table.FirstOrDefault(x => x.Slug == categorySlug);

            if (category == null)
            {
                return new HttpNotFoundResult();
            }

            var product = productRepository.Value.Table.FirstOrDefault(x => x.Slug == productSlug);

            if (product == null)
            {
                return new HttpNotFoundResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store), Url.Action("Index"));
            WorkContext.Breadcrumbs.Add(category.Name, Url.Action("Products") + "?pageIndex=1");
            WorkContext.Breadcrumbs.Add(product.Name);

            return View("Product", product);
        }
    }
}