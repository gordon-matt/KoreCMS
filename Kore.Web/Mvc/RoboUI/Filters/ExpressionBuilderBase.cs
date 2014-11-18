using System;
using System.Linq.Expressions;
using Kore.Web.Mvc.RoboUI.Expressions;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public abstract class ExpressionBuilderBase
    {
        private readonly Type itemType;
        private readonly ExpressionBuilderOptions options;
        private ParameterExpression parameterExpression;

        protected ExpressionBuilderBase(Type itemType)
        {
            this.itemType = itemType;
            options = new ExpressionBuilderOptions();
        }

        public ExpressionBuilderOptions Options
        {
            get { return options; }
        }

        protected internal Type ItemType
        {
            get { return itemType; }
        }

        protected internal ParameterExpression ParameterExpression
        {
            get
            {
                if (parameterExpression == null)
                    parameterExpression = Expression.Parameter(ItemType, "item");
                return parameterExpression;
            }
            set { parameterExpression = value; }
        }
    }
}