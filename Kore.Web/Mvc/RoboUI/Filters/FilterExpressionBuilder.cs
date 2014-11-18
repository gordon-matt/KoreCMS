using System.Linq.Expressions;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public abstract class FilterExpressionBuilder : ExpressionBuilderBase
    {
        protected FilterExpressionBuilder(ParameterExpression parameterExpression)
            : base(parameterExpression.Type)
        {
            ParameterExpression = parameterExpression;
        }

        public abstract Expression CreateBodyExpression();

        /// <exception cref="T:System.ArgumentException"><c>ArgumentException</c>.</exception>
        public LambdaExpression CreateFilterExpression()
        {
            return Expression.Lambda(CreateBodyExpression(), new[]
            {
                ParameterExpression
            });
        }

        /// <exception cref="T:System.ArgumentException"><c>ArgumentException</c>.</exception>
        public Expression<TDelegate> CreateFilterExpression<TDelegate>()
        {
            return Expression.Lambda<TDelegate>(CreateBodyExpression(), new[]
            {
                ParameterExpression
            });
        }
    }
}