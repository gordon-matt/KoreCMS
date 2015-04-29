using System.Collections.Generic;

namespace Kore.Web.Indexing
{
    public interface ISearchFieldProvider
    {
        IEnumerable<string> IndexFields { get; }
    }
}