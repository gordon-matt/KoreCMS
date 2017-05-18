namespace Kore.Data.ElasticSearch
{
    public class ElasticProvider<T> : ElasticSearchProvider<T>
        where T : class, IEntity
    {
        public ElasticProvider(string connectionString) :
            base(connectionString)
        {
        }
    }
}