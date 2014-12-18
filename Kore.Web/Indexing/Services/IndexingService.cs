using System.Collections.Generic;
using Kore.Localization;
using Kore.Web.Mvc.Notify;

namespace Kore.Web.Indexing.Services
{
    public class IndexingService : IIndexingService
    {
        private readonly IIndexManager indexManager;
        private readonly IEnumerable<IIndexNotifierHandler> indexNotifierHandlers;
        private readonly IIndexStatisticsProvider indexStatisticsProvider;
        private readonly IIndexingTaskExecutor indexingTaskExecutor;
        private readonly INotifier notifier;

        public IndexingService(
            IIndexManager indexManager,
            IEnumerable<IIndexNotifierHandler> indexNotifierHandlers,
            IIndexStatisticsProvider indexStatisticsProvider,
            IIndexingTaskExecutor indexingTaskExecutor,
            INotifier notifier)
        {
            this.indexManager = indexManager;
            this.indexNotifierHandlers = indexNotifierHandlers;
            this.indexStatisticsProvider = indexStatisticsProvider;
            this.indexingTaskExecutor = indexingTaskExecutor;
            this.notifier = notifier;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void RebuildIndex(string indexName)
        {
            if (!indexManager.HasIndexProvider())
            {
                notifier.Warning(T("There is no search index to rebuild."));
                return;
            }

            if (indexingTaskExecutor.DeleteIndex(indexName))
            {
                notifier.Info(T("The index, '{0}' has been rebuilt.", indexName));
                UpdateIndex(indexName);
            }
            else
            {
                notifier.Warning(T("The index, '{0}' could not be rebuilt. It might already be in use. Please try again later.", indexName));
            }
        }

        public void UpdateIndex(string indexName)
        {
            foreach (var handler in indexNotifierHandlers)
            {
                handler.UpdateIndex(indexName);
            }

            notifier.Info(T("The search index has been updated."));
        }

        IndexEntry IIndexingService.GetIndexEntry(string indexName)
        {
            var provider = indexManager.GetSearchIndexProvider();
            if (provider == null)
                return null;

            return new IndexEntry
            {
                IndexName = indexName,
                DocumentCount = provider.NumDocs(indexName),
                Fields = provider.GetFields(indexName),
                LastUpdateUtc = indexStatisticsProvider.GetLastIndexedUtc(indexName),
                IndexingStatus = indexStatisticsProvider.GetIndexingStatus(indexName)
            };
        }
    }
}