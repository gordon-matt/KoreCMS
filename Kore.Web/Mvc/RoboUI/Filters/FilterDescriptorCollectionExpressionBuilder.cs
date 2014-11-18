using System.Collections.Generic;
using System.Linq.Expressions;
using Kore.Web.Mvc.RoboUI.Expressions;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class FilterDescriptorCollectionExpressionBuilder : FilterExpressionBuilder
    {
        private readonly IEnumerable<IFilterDescriptor> filterDescriptors;
        private readonly FilterCompositionLogicalOperator logicalOperator;

        public FilterDescriptorCollectionExpressionBuilder(ParameterExpression parameterExpression,
            IEnumerable<IFilterDescriptor> filterDescriptors)
            : this(parameterExpression, filterDescriptors, FilterCompositionLogicalOperator.And)
        {
        }

        public FilterDescriptorCollectionExpressionBuilder(ParameterExpression parameterExpression,
            IEnumerable<IFilterDescriptor> filterDescriptors, FilterCompositionLogicalOperator logicalOperator)
            : base(parameterExpression)
        {
            this.filterDescriptors = filterDescriptors;
            this.logicalOperator = logicalOperator;
        }

        public override Expression CreateBodyExpression()
        {
            Expression left = null;
            foreach (IFilterDescriptor filterDescriptor in filterDescriptors)
            {
                InitilializeExpressionBuilderOptions(filterDescriptor);
                Expression filterExpression = filterDescriptor.CreateFilterExpression(ParameterExpression);
                left = left != null ? ComposeExpressions(left, filterExpression, logicalOperator) : filterExpression;
            }
            return left ?? ExpressionConstants.TrueLiteral;
        }

        private static Expression ComposeExpressions(Expression left, Expression right,
            FilterCompositionLogicalOperator logicalOperator)
        {
            switch (logicalOperator)
            {
                case FilterCompositionLogicalOperator.Or:
                    return Expression.OrElse(left, right);

                default:
                    return Expression.AndAlso(left, right);
            }
        }

        private void InitilializeExpressionBuilderOptions(IFilterDescriptor filterDescriptor)
        {
            var filterDescriptorBase = filterDescriptor as FilterDescriptorBase;
            if (filterDescriptorBase == null)
                return;
            filterDescriptorBase.ExpressionBuilderOptions.CopyFrom(Options);
        }
    }
}