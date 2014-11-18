using System;
using System.Linq.Expressions;
using Kore.Web.Mvc.RoboUI.Expressions;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    internal static class ExpressionFactory
    {
        public static ConstantExpression EmptyStringExpression
        {
            get
            {
                return Expression.Constant(string.Empty);
            }
        }

        public static ConstantExpression ZeroExpression
        {
            get
            {
                return Expression.Constant(0);
            }
        }

        public static Expression MakeMemberAccess(Expression instance, string memberName, bool liftMemberAccessToNull)
        {
            Expression memberAccess = MakeMemberAccess(instance, memberName);
            if (liftMemberAccessToNull)
                return LiftMemberAccessToNull(memberAccess);
            return memberAccess;
        }

        public static Expression MakeMemberAccess(Expression instance, string memberName)
        {
            foreach (IMemberAccessToken token in MemberAccessTokenizer.GetTokens(memberName))
                instance = MemberAccessTokenExtensions.CreateMemberAccessExpression(token, instance);
            return instance;
        }

        public static Expression LiftMemberAccessToNull(Expression memberAccess)
        {
            Expression defaultValue = DefaltValueExpression(memberAccess.Type);
            return LiftMemberAccessToNullRecursive(memberAccess, memberAccess, defaultValue);
        }

        public static Expression DefaltValueExpression(Type type)
        {
            return Expression.Constant(TypeExtensions.DefaultValue(type), type);
        }

        internal static bool IsNotNullConstantExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
                return ((ConstantExpression)expression).Value != null;
            return false;
        }

        /// <exception cref="T:System.ArgumentException">Provided expression should have string type</exception>
        internal static Expression LiftStringExpressionToEmpty(Expression stringExpression)
        {
            if (stringExpression.Type != typeof(string))
                throw new ArgumentException("Provided expression should have string type", "stringExpression");
            if (IsNotNullConstantExpression(stringExpression))
                return stringExpression;
            return Expression.Coalesce(stringExpression, EmptyStringExpression);
        }

        private static Expression LiftMemberAccessToNullRecursive(Expression memberAccess, Expression conditionalExpression, Expression defaultValue)
        {
            Expression expressionFromExpression = GetInstanceExpressionFromExpression(memberAccess);
            if (expressionFromExpression == null)
                return conditionalExpression;
            conditionalExpression = CreateIfNullExpression(expressionFromExpression, conditionalExpression, defaultValue);
            return LiftMemberAccessToNullRecursive(expressionFromExpression, conditionalExpression, defaultValue);
        }

        private static Expression GetInstanceExpressionFromExpression(Expression memberAccess)
        {
            MemberExpression memberExpression = memberAccess as MemberExpression;
            if (memberExpression != null)
                return memberExpression.Expression;
            MethodCallExpression methodCallExpression = memberAccess as MethodCallExpression;
            if (methodCallExpression != null)
                return methodCallExpression.Object;
            return null;
        }

        private static Expression CreateIfNullExpression(Expression instance, Expression memberAccess, Expression defaultValue)
        {
            if (ShouldGenerateCondition(instance.Type))
                return CreateConditionExpression(instance, memberAccess, defaultValue);
            return memberAccess;
        }

        private static bool ShouldGenerateCondition(Type type)
        {
            if (type.IsValueType)
                return TypeExtensions.IsNullableType(type);
            return true;
        }

        private static Expression CreateConditionExpression(Expression instance, Expression memberAccess, Expression defaultValue)
        {
            Expression right = DefaltValueExpression(instance.Type);
            return Expression.Condition(Expression.NotEqual(instance, right), memberAccess, defaultValue);
        }
    }
}