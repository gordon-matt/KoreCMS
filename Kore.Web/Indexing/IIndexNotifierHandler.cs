using Kore.Web.Events;

namespace Kore.Web.Indexing
{
    public interface IIndexNotifierHandler : IEventHandler
    {
        void UpdateIndex(string indexName);
    }
}