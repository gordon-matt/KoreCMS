using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Data;
using Kore.Data.Services;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Services
{
    public interface IPageTypeService : IGenericDataService<PageType>
    {
        IEnumerable<KorePageType> GetKorePageTypes();
    }

    public class PageTypeService : GenericDataService<PageType>, IPageTypeService
    {
        private static Lazy<IEnumerable<KorePageType>> korePageTypes;

        public PageTypeService(IRepository<PageType> repository)
            : base(repository)
        {
            korePageTypes = new Lazy<IEnumerable<KorePageType>>(() =>
            {
                var typeFinder = EngineContext.Current.Resolve<ITypeFinder>();
                return typeFinder.FindClassesOfType<KorePageType>()
                    .Select(x => (KorePageType)Activator.CreateInstance(x));
            });
        }

        #region IPageTypeService Members

        public IEnumerable<KorePageType> GetKorePageTypes()
        {
            return korePageTypes.Value;
        }

        #endregion IPageTypeService Members
    }
}