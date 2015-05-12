using Kore.Collections;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Services
{
    public interface IHistoricPageService : IGenericDataService<HistoricPage>
    {
    }

    public class HistoricPageService : GenericDataService<HistoricPage>, IHistoricPageService
    {
        private readonly ICacheManager cacheManager;
        private readonly PageSettings pageSettings;

        public HistoricPageService(
            IRepository<HistoricPage> repository,
            ICacheManager cacheManager,
            PageSettings pageSettings)
            : base(repository)
        {
            this.cacheManager = cacheManager;
            this.pageSettings = pageSettings;
        }

        public override int Insert(HistoricPage record)
        {
            var pages = Repository.Table.Where(x => x.PageId == record.PageId);

            if (pages.Count() > (pageSettings.NumberOfPageVersionsToKeep - 1))
            {
                var pagesToKeep = pages
                    .OrderByDescending(x => x.ArchivedDate)
                    .Take(pageSettings.NumberOfPageVersionsToKeep - 1)
                    .Select(x => x.Id)
                    .ToList();

                var pagesToDelete = pages.Where(x => !pagesToKeep.Contains(x.Id)).ToHashSet();

                Delete(pagesToDelete);
            }

            // now insert new record
            return base.Insert(record);
        }
    }
}