using Kore.Web.Indexing.Services;

namespace Kore.Web.Indexing
{
    public class IndexNotifierHandler : IIndexNotifierHandler
    {
        private readonly IIndexingTaskExecutor indexingTaskExecutor;

        public IndexNotifierHandler(IIndexingTaskExecutor indexingTaskExecutor)
        {
            this.indexingTaskExecutor = indexingTaskExecutor;
        }

        public void UpdateIndex(string indexName)
        {
            indexingTaskExecutor.UpdateIndex(indexName);
        }
    }
}