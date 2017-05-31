namespace Kore.Data.ElasticSearch
{
    public class ElasticProvider<T> : ElasticSearchProvider<T>
        where T : class
    {
        public ElasticProvider(string connectionString, string indexPrefix) :
            base(connectionString, indexPrefix)
        {
        }
    }
}