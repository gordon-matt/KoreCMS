using System.Linq.Expressions;
using System.Reflection;
using Kore.Web.Mvc.RoboUI.Expressions;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class FilterDescriptionExpressionBuilder : FilterExpressionBuilder
    {
        private readonly FilterDescription filterDescription;

        public FilterDescriptionExpressionBuilder(ParameterExpression parameterExpression,
            FilterDescription filterDescription)
            : base(parameterExpression)
        {
            this.filterDescription = filterDescription;
        }

        public FilterDescription FilterDescription
        {
            get { return filterDescription; }
        }

        private Expression FilterDescriptionExpression
        {
            get { return Expression.Constant(filterDescription); }
        }

        private MethodInfo SatisfiesFilterMethodInfo
        {
            get
            {
                return filterDescription.GetType().GetMethod("SatisfiesFilter", new[]
                {
                    typeof (object)
                });
            }
        }

        public override Expression CreateBodyExpression()
        {
            if (filterDescription.IsActive)
                return CreateActiveFilterExpression();
            return ExpressionConstants.TrueLiteral;
        }

        protected virtual Expression CreateActiveFilterExpression()
        {
            return CreateSatisfiesFilterExpression();
        }

        private MethodCallExpression CreateSatisfiesFilterExpression()
        {
            Expression expression = ParameterExpression;
            if (expression.Type.IsValueType)
                expression = Expression.Convert(expression, typeof(object));
            return Expression.Call(FilterDescriptionExpression, SatisfiesFilterMethodInfo, new[]
            {
                expression
            });
        }
    }
}