using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace Kore.Data.ElasticSearch
{
    public abstract class ElasticSearchProvider<T> : IElasticSearchProvider<T>
        where T : class, IEntity
    {
        private const string defaultIndex = "test_index";
        private readonly ElasticClient client;

        protected ElasticSearchProvider(string connectionString)
        {
            ConnectionString = connectionString;

            var uri = new Uri(ConnectionString);
            var settings = new ConnectionSettings(uri);
            settings.DefaultIndex(defaultIndex);
            client = new ElasticClient(settings);
        }

        protected string ConnectionString { get; }

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

        public IDeleteResponse Delete(T entity)
        {
            var documentPath = new DocumentPath<T>(entity);
            return client.Delete(documentPath, x => x.Index(defaultIndex));
        }

        public IBulkResponse Delete(IEnumerable<T> entities)
        {
            return client.DeleteMany(entities);
        }

        public async Task<IDeleteResponse> DeleteAsync(T entity)
        {
            var documentPath = new DocumentPath<T>(entity);
            return await client.DeleteAsync(documentPath, x => x.Index(defaultIndex));
        }

        public async Task<IBulkResponse> DeleteAsync(IEnumerable<T> entities)
        {
            return await client.DeleteManyAsync(entities);
        }

        public IDeleteByQueryResponse DeleteAll()
        {
            return client.DeleteByQuery<T>(x => x.AllIndices());
        }

        public IIndexResponse InsertOrUpdate(T entity)
        {
            return client.Index(entity, x => x.Index(defaultIndex));
        }

        public IBulkResponse InsertOrUpdate(IEnumerable<T> entities)
        {
            return client.IndexMany(entities, defaultIndex);
        }

        public async Task<IIndexResponse> InsertOrUpdateAsync(T entity)
        {
            return await client.IndexAsync(entity, x => x.Index(defaultIndex));
        }

        public async Task<IBulkResponse> InsertOrUpdateAsync(IEnumerable<T> entities)
        {
            return await client.IndexManyAsync(entities, defaultIndex);
        }
    }
}