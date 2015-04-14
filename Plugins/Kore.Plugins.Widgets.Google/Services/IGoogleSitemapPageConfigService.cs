//using System.Collections.Generic;
//using System.Linq;
//using Kore.Collections;
//using Kore.Data;
//using Kore.Data.Services;
//using Kore.Plugins.Widgets.Google.Data.Domain;
//using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

//namespace Kore.Plugins.Widgets.Google.Services
//{
//    public interface IGoogleSitemapPageConfigService : IGenericDataService<GoogleSitemapPageConfig>
//    {
//        IEnumerable<GoogleSitemapPageConfig> GetConfig();

//        void SetConfig(IEnumerable<GoogleSitemapPageConfig> config);
//    }

//    public class GoogleSitemapPageConfigService : GenericDataService<GoogleSitemapPageConfig>, IGoogleSitemapPageConfigService
//    {
//        private IRepository<Page> pageRepository;

//        public GoogleSitemapPageConfigService(
//            IRepository<GoogleSitemapPageConfig> repository,
//            IRepository<Page> pageRepository)
//            : base(repository)
//        {
//            this.pageRepository = pageRepository;
//        }

//        #region IGoogleSitemapPageConfigService Members

//        public IEnumerable<GoogleSitemapPageConfig> GetConfig()
//        {
//            // First ensure that current pages are in the config
//            var config = Repository.Table.ToHashSet();
//            var configPageIds = config.Select(x => x.PageId).ToHashSet();
//            var pages = pageRepository.Table.ToHashSet();
//            var pageIds = pages.Select(x => x.Id).ToHashSet();

//            var newPageIds = pageIds.Except(configPageIds);
//            var pageIdsToDelete = configPageIds.Except(pageIds);

//            if (pageIdsToDelete.Any())
//            {
//                Repository.Delete(x => pageIdsToDelete.Contains(x.PageId));
//            }

//            if (newPageIds.Any())
//            {
//                var toInsert = pages
//                    .Where(x => newPageIds.Contains(x.Id))
//                    .Select(x => new GoogleSitemapPageConfig
//                    {
//                        PageId = x.Id,
//                        ChangeFrequency = ChangeFrequency.Weekly,
//                        Priority = .5f
//                    });

//                Repository.Insert(toInsert);
//            }

//            // Then return the config
//            return Repository.Table
//                .ToHashSet();
//        }

//        public void SetConfig(IEnumerable<GoogleSitemapPageConfig> config)
//        {
//            Update(config);
//        }

//        #endregion IGoogleSitemapPageConfigService Members
//    }
//}