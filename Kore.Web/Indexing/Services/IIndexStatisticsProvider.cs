using System;

namespace Kore.Web.Indexing.Services
{
    public interface IIndexStatisticsProvider
    {
        DateTime GetLastIndexedUtc(string indexName);

        IndexingStatus GetIndexingStatus(string indexName);
    }
}