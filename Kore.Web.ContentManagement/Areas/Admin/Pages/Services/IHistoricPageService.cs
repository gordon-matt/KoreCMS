using System.Linq;
using Kore.Caching;
using Kore.Collections;
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
        private readonly PageSettings pageSettings;

        public HistoricPageService(
            ICacheManager cacheManager,
            IRepository<HistoricPage> repository,
            PageSettings pageSettings)
            : base(cacheManager, repository)
        {
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