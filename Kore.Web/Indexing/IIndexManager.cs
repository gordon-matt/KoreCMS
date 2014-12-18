namespace Kore.Web.Indexing
{
    public interface IIndexManager //: IDependency
    {
        bool HasIndexProvider();

        IIndexProvider GetSearchIndexProvider();
    }
}