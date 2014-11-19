using System;
using System.Linq.Expressions;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    /// <summary>
    /// Represents a filtering abstraction that knows how to create predicate filtering expression.
    /// </summary>
    public interface IFilterDescriptor : ICloneable
    {
        /// <summary>
        /// Creates a predicate filter expression used for collection filtering.
        /// </summary>
        /// <param name="instance">The instance expression, which will be used for filtering.</param>
        /// <returns>A predicate filter expression.</returns>
        Expression CreateFilterExpression(Expression instance);
    }
}