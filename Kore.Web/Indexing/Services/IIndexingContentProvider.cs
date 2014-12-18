using System;
using System.Collections.Generic;

namespace Kore.Web.Indexing.Services
{
    public interface IIndexingContentProvider
    {
        IEnumerable<IDocumentIndex> GetDocuments(Func<string, IDocumentIndex> factory);
    }
}