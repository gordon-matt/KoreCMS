using System.Linq.Expressions;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public abstract class FilterDescription : FilterDescriptorBase
    {
        /// <summary>
        ///     If false <see cref="M:Kore.Web.Mvc.RoboUI.Filters.FilterDescription.SatisfiesFilter(System.Object)" />
        ///     will not execute.
        /// </summary>
        public virtual bool IsActive
        {
            get { return true; }
        }

        /// <summary>
        ///     The method checks whether the passed parameter satisfies filter criteria.
        /// </summary>
        public abstract bool SatisfiesFilter(object dataItem);

        /// <summary>
        ///     Creates a predicate filter expression that calls
        ///     <see cref="M:Kore.Web.Mvc.RoboUI.Filters.FilterDescription.SatisfiesFilter(System.Object)" />.
        /// </summary>
        /// <param name="parameterExpression">
        ///     The parameter expression, which parameter
        ///     will be passed to
        ///     <see cref="M:Kore.Web.Mvc.RoboUI.Filters.FilterDescription.SatisfiesFilter(System.Object)" /> method.
        /// </param>
        protected override Expression CreateFilterExpression(ParameterExpression parameterExpression)
        {
            return new FilterDescriptionExpressionBuilder(parameterExpression, this).CreateBodyExpression();
        }
    }
}