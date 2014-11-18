using System.Collections;
using System.Collections.Generic;

namespace Kore.Collections.Generic
{
    public interface IPagedList : IEnumerable
    {
        int PageIndex { get; }

        int PageSize { get; }

        int ItemCount { get; }

        int PageCount { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }
    }

    public interface IPagedList<T> : IPagedList, IList<T>
    {
    }
}