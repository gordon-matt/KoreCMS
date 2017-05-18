using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace Kore.Data.ElasticSearch
{
    public interface IElasticSearchProvider<T>
        where T : class, IEntity
    {
        ISearchResponse<T> Find(Func<SearchDescriptor<T>, ISearchRequest> selector);

        Task<ISearchResponse<T>> FindAsync(Func<SearchDescriptor<T>, ISearchRequest> selector);

        ISearchResponse<T> Scroll(Time scrollTime, string scrollId);

        Task<ISearchResponse<T>> ScrollAsync(Time scrollTime, string scrollId);

        IDeleteResponse Delete(T entity);

        IBulkResponse Delete(IEnumerable<T> entities);

        Task<IDeleteResponse> DeleteAsync(T entity);

        Task<IBulkResponse> DeleteAsync(IEnumerable<T> entities);

        IDeleteByQueryResponse DeleteAll();

        IIndexResponse InsertOrUpdate(T entity);

        IBulkResponse InsertOrUpdate(IEnumerable<T> entities);

        Task<IIndexResponse> InsertOrUpdateAsync(T entity);

        Task<IBulkResponse> InsertOrUpdateAsync(IEnumerable<T> entities);
    }
}