using System;
using System.Linq.Expressions;
using Kore.Web.Mvc.RoboUI.Filters;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public abstract class MemberAccessExpressionBuilderBase : ExpressionBuilderBase
    {
        private readonly string memberName;

        protected MemberAccessExpressionBuilderBase(Type itemType, string memberName)
            : base(itemType)
        {
            this.memberName = memberName;
        }

        public string MemberName
        {
            get { return memberName; }
        }

        public abstract Expression CreateMemberAccessExpression();

        public LambdaExpression CreateLambdaExpression()
        {
            return Expression.Lambda(CreateMemberAccessExpression(), new[]
            {
                ParameterExpression
            });
        }
    }
}