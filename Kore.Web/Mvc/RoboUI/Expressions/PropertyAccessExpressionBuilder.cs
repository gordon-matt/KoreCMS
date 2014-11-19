using System;
using System.Linq.Expressions;
using Kore.Web.Mvc.RoboUI.Filters;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public class PropertyAccessExpressionBuilder : MemberAccessExpressionBuilderBase
    {
        public PropertyAccessExpressionBuilder(Type itemType, string memberName)
            : base(itemType, memberName)
        {
        }

        public override Expression CreateMemberAccessExpression()
        {
            if (string.IsNullOrEmpty(MemberName))
                return ParameterExpression;
            return ExpressionFactory.MakeMemberAccess(ParameterExpression, MemberName, Options.LiftMemberAccessToNull);
        }
    }
}