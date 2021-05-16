using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace Kore.Data.ElasticSearch
{
    public abstract class ElasticSearchProvider<T> : IElasticSearchProvider<T>
        where T : class
    {
        private readonly ElasticClient client;

        protected ElasticSearchProvider(string connectionString, string indexPrefix)
        {
            ConnectionString = connectionString;
            IndexName = string.Concat(indexPrefix, "_", typeof(T).Name.ToLowerInvariant());

            var uri = new Uri(ConnectionString);
            var settings = new ConnectionSettings(uri);
            settings.DefaultIndex(IndexName);
            client = new ElasticClient(settings);
        }

        protected string ConnectionString { get; }

        protected string IndexName { get; }

        public bool Any(IEnumerable<Func<QueryContainerDescriptor<T>, QueryContainer>> filters)
        {
            var query = Find(x => x.Query(q => q.Bool(b => b.Filter(filters))).Take(0));
            return query.Total > 0;
        }

        public ISearchResponse<T> Find(Func<SearchDescriptor<T>, ISearchRequest> selector)
        {
            return client.Search(selector);
        }

        public async Task<ISearchResponse<T>> FindAsync(Func<SearchDescriptor<T>, ISearchRequest> selector)
        {
            return await client.SearchAsync(selector);
        }

        public ISearchResponse<T> Scroll(Time scrollTime, string scrollId)
        {
            return client.Scroll<T>(scrollTime, scrollId);
        }

        public async Task<ISearchResponse<T>> ScrollAsync(Time scrollTime, string scrollId)
        {
            return await client.ScrollAsync<T>(scrollTime, scrollId);
        }

        public DeleteResponse Delete(T entity)
        {
            var documentPath = new DocumentPath<T>(entity);
            return client.Delete(documentPath, x => x.Index(IndexName));
        }

        public BulkResponse Delete(IEnumerable<T> entities)
        {
            return client.DeleteMany(entities);
        }

        public async Task<DeleteResponse> DeleteAsync(T entity)
        {
            var documentPath = new DocumentPath<T>(entity);
            return await client.DeleteAsync(documentPath, x => x.Index(IndexName));
        }

        public async Task<BulkResponse> DeleteAsync(IEnumerable<T> entities)
        {
            return await client.DeleteManyAsync(entities);
        }

        public DeleteByQueryResponse DeleteAll()
        {
            return client.DeleteByQuery<T>(x => x.AllIndices());
        }

        public IndexResponse InsertOrUpdate(T entity)
        {
            return client.Index(entity, x => x.Index(IndexName));
        }

        public BulkResponse InsertOrUpdate(IEnumerable<T> entities)
        {
            return client.IndexMany(entities, IndexName);
        }

        public async Task<IndexResponse> InsertOrUpdateAsync(T entity)
        {
            return await client.IndexAsync(entity, x => x.Index(IndexName));
        }

        public async Task<BulkResponse> InsertOrUpdateAsync(IEnumerable<T> entities)
        {
            return await client.IndexManyAsync(entities, IndexName);
        }
    }
}