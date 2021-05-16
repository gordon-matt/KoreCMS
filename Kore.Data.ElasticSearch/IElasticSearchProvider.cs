using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace Kore.Data.ElasticSearch
{
    public interface IElasticSearchProvider<T>
        where T : class
    {
        bool Any(IEnumerable<Func<QueryContainerDescriptor<T>, QueryContainer>> filters);

        ISearchResponse<T> Find(Func<SearchDescriptor<T>, ISearchRequest> selector);

        Task<ISearchResponse<T>> FindAsync(Func<SearchDescriptor<T>, ISearchRequest> selector);

        ISearchResponse<T> Scroll(Time scrollTime, string scrollId);

        Task<ISearchResponse<T>> ScrollAsync(Time scrollTime, string scrollId);

        DeleteResponse Delete(T entity);

        BulkResponse Delete(IEnumerable<T> entities);

        Task<DeleteResponse> DeleteAsync(T entity);

        Task<BulkResponse> DeleteAsync(IEnumerable<T> entities);

        DeleteByQueryResponse DeleteAll();

        IndexResponse InsertOrUpdate(T entity);

        BulkResponse InsertOrUpdate(IEnumerable<T> entities);

        Task<IndexResponse> InsertOrUpdateAsync(T entity);

        Task<BulkResponse> InsertOrUpdateAsync(IEnumerable<T> entities);
    }
}