using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Kore.Web.Mvc.RoboUI.Expressions;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    /// <summary>
    ///     Base class for all <see cref="T:Kore.Web.Mvc.RoboUI.Filters.IFilterDescriptor" /> used for
    ///     handling the logic for property changed notifications.
    /// </summary>
    public abstract class FilterDescriptorBase : IFilterDescriptor
    {
        private ExpressionBuilderOptions options;

        internal ExpressionBuilderOptions ExpressionBuilderOptions
        {
            get
            {
                if (options == null)
                    options = new ExpressionBuilderOptions();
                return options;
            }
        }

        /// <summary>
        ///     Creates a filter expression by delegating its creation to
        ///     <see cref="M:Kore.Web.Mvc.RoboUI.Filters.FilterDescriptorBase.CreateFilterExpression(System.Linq.Expressions.ParameterExpression)" />
        ///     , if
        ///     <paramref name="instance" /> is <see cref="T:System.Linq.Expressions.ParameterExpression" />, otherwise throws
        ///     <see cref="T:System.ArgumentException" />
        /// </summary>
        /// <param name="instance">The instance expression, which will be used for filtering.</param>
        /// <returns>
        ///     A predicate filter expression.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///     Parameter should be of type
        ///     <see cref="T:System.Linq.Expressions.ParameterExpression" />
        /// </exception>
        public virtual Expression CreateFilterExpression(Expression instance)
        {
            var parameterExpression = instance as ParameterExpression;
            if (parameterExpression == null)
                throw new ArgumentException("Parameter should be of type ParameterExpression", "instance");
            return CreateFilterExpression(parameterExpression);
        }

        /// <summary>
        ///     Creates a predicate filter expression used for collection filtering.
        /// </summary>
        /// <param name="parameterExpression">The parameter expression, which will be used for filtering.</param>
        /// <returns>
        ///     A predicate filter expression.
        /// </returns>
        protected virtual Expression CreateFilterExpression(ParameterExpression parameterExpression)
        {
            return parameterExpression;
        }

        protected virtual void Serialize(IDictionary<string, object> json)
        {
        }

        public abstract object Clone();
    }
}