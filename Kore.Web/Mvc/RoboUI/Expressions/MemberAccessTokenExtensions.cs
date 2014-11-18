using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Kore.Web.Mvc.RoboUI.Filters;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public static class MemberAccessTokenExtensions
    {
        /// <exception cref="T:System.ArgumentException">
        ///     Invalid name for property or field; or indexer with the specified
        ///     arguments.
        /// </exception>
        public static Expression CreateMemberAccessExpression(IMemberAccessToken token, Expression instance)
        {
            MemberInfo memberInfoForType = GetMemberInfoForType(token, instance.Type);
            if (memberInfoForType == null)
                throw new ArgumentException(FormatInvalidTokenErrorMessage(token, instance.Type));
            var indexerToken = token as IndexerToken;
            if (indexerToken == null)
                return Expression.MakeMemberAccess(instance, memberInfoForType);
            IEnumerable<Expression> indexerArguments = GetIndexerArguments(indexerToken);
            return Expression.Call(instance, (MethodInfo)memberInfoForType, indexerArguments);
        }

        /// <exception cref="T:System.InvalidOperationException"><c>InvalidOperationException</c>.</exception>
        private static MemberInfo GetMemberInfoForType(IMemberAccessToken token, Type targetType)
        {
            var token1 = token as PropertyToken;
            if (token1 != null)
                return GetMemberInfoFromPropertyToken(token1, targetType);
            var token2 = token as IndexerToken;
            if (token2 != null)
                return GetMemberInfoFromIndexerToken(token2, targetType);
            throw new InvalidOperationException(TypeExtensions.GetTypeName(token.GetType()) + " is not supported");
        }

        private static MemberInfo GetMemberInfoFromPropertyToken(PropertyToken token, Type targetType)
        {
            return TypeExtensions.FindPropertyOrField(targetType, token.PropertyName);
        }

        private static MemberInfo GetMemberInfoFromIndexerToken(IndexerToken token, Type targetType)
        {
            PropertyInfo indexerPropertyInfo = TypeExtensions.GetIndexerPropertyInfo(targetType,
                token.Arguments.Select(a => a.GetType()).ToArray());
            if (indexerPropertyInfo != null)
                return indexerPropertyInfo.GetGetMethod();
            return null;
        }

        private static IEnumerable<Expression> GetIndexerArguments(IndexerToken indexerToken)
        {
            return indexerToken.Arguments.Select(a => (Expression)Expression.Constant(a));
        }

        private static string FormatInvalidTokenErrorMessage(IMemberAccessToken token, Type type)
        {
            var propertyToken = token as PropertyToken;
            string str1;
            string str2;
            if (propertyToken != null)
            {
                str1 = "property or field";
                str2 = propertyToken.PropertyName;
            }
            else
            {
                str1 = "indexer with arguments";
                str2 = string.Join(",",
                    ((IndexerToken)token).Arguments.Where(a => a != null).Select(a => a.ToString()).ToArray());
            }
            return string.Format(CultureInfo.CurrentCulture, "Invalid {0} - '{1}' for type: {2}",
                (object)str1, (object)str2, (object)TypeExtensions.GetTypeName(type));
        }
    }
}