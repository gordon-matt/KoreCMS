using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.ContentManagement.Infrastructure;

namespace Kore.Plugins.Ecommerce.Simple.Infrastructure
{
    public class AutoMenuProvider : IAutoMenuProvider
    {
        private readonly Lazy<IRepository<Category>> categoryRepository;
        private readonly Lazy<IRepository<Product>> productRepository;
        private readonly StoreSettings storeSettings;

        public AutoMenuProvider(
            Lazy<IRepository<Category>> categoryRepository,
            Lazy<IRepository<Product>> productRepository,
            StoreSettings storeSettings)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
            this.storeSettings = storeSettings;
        }

        #region IAutoMenuProvider Members

        public string RootUrlSlug
        {
            get { return "store"; }
        }

        public MenuItem GetRootMenuItem()
        {
            if (!storeSettings.ShowOnMenus)
            {
                return null;
            }

            return new MenuItem
            {
                Text = storeSettings.PageTitle,
                Url = "/store",
                Enabled = true,
                ParentId = null,
                Position = storeSettings.MenuPosition
            };
        }

        public IEnumerable<MenuItem> GetSubMenuItems(string currentUrlSlug)
        {
            if (!storeSettings.ShowOnMenus)
            {
                return Enumerable.Empty<MenuItem>();
            }

            if (!currentUrlSlug.StartsWithAny(RootUrlSlug, "categories"))
            {
                return Enumerable.Empty<MenuItem>();
            }

            string[] split = currentUrlSlug.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            switch (split.Length)
            {
                case 1:
                    {
                        var menuItems = categoryRepository.Value.Table
                            .OrderBy(x => x.Name)
                            .Select((x, index) => new MenuItem
                            {
                                Text = x.Name,
                                Url = "/" + x.Slug,
                                Enabled = true,
                                ParentId = null,
                                Position = index
                            });
                        return menuItems;
                    }
                case 2:
                    {
                        string categorySlug = split[1];
                        var category = categoryRepository.Value.Table.FirstOrDefault(x => x.Slug == categorySlug);

                        if (category == null)
                        {
                            return Enumerable.Empty<MenuItem>();
                        }

                        var menuItems = categoryRepository.Value.Table
                            .Where(x => x.ParentId == category.Id)
                            .OrderBy(x => x.Name)
                            .Select((x, index) => new MenuItem
                            {
                                Text = x.Name,
                                Url = "/" + x.Slug,
                                Enabled = true,
                                ParentId = null,
                                Position = index
                            });
                        return menuItems;
                    }
                case 3:
                    {
                        //TODO: Can we add anything here? Probably not...
                        return Enumerable.Empty<MenuItem>();
                    }
                default: return Enumerable.Empty<MenuItem>();
            }
        }

        #endregion
    }
}
