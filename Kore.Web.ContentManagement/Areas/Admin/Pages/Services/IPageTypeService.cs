using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Services
{
    public interface IPageTypeService : IGenericDataService<PageType>
    {
        KorePageType GetKorePageType(string name);

        IEnumerable<KorePageType> GetKorePageTypes();
    }

    public class PageTypeService : GenericDataService<PageType>, IPageTypeService
    {
        private static Lazy<IEnumerable<KorePageType>> korePageTypes;

        public PageTypeService(ICacheManager cacheManager, IRepository<PageType> repository)
            : base(cacheManager, repository)
        {
            korePageTypes = new Lazy<IEnumerable<KorePageType>>(() =>
            {
                var typeFinder = EngineContext.Current.Resolve<ITypeFinder>();

                var pageTypes = typeFinder.FindClassesOfType<KorePageType>()
                    .Select(x => (KorePageType)Activator.CreateInstance(x));

                return pageTypes.Where(x => x.IsEnabled);
            });
        }

        #region IPageTypeService Members

        public KorePageType GetKorePageType(string name)
        {
            return korePageTypes.Value.FirstOrDefault(x => x.Name == name);
        }

        public IEnumerable<KorePageType> GetKorePageTypes()
        {
            return korePageTypes.Value;
        }

        #endregion IPageTypeService Members
    }
}