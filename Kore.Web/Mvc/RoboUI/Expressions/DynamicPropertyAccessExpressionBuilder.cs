using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.CSharp.RuntimeBinder;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public class DynamicPropertyAccessExpressionBuilder : MemberAccessExpressionBuilderBase
    {
        public DynamicPropertyAccessExpressionBuilder(Type itemType, string memberName)
            : base(itemType, memberName)
        {
        }

        public override Expression CreateMemberAccessExpression()
        {
            if (string.IsNullOrEmpty(MemberName))
                return ParameterExpression;
            var instance = (Expression)ParameterExpression;
            foreach (IMemberAccessToken memberAccessToken in MemberAccessTokenizer.GetTokens(MemberName))
            {
                if (memberAccessToken is PropertyToken)
                {
                    string propertyName = ((PropertyToken)memberAccessToken).PropertyName;
                    instance = CreatePropertyAccessExpression(instance, propertyName);
                }
                else if (memberAccessToken is IndexerToken)
                    instance = CreateIndexerAccessExpression(instance, (IndexerToken)memberAccessToken);
            }
            return instance;
        }

        private Expression CreateIndexerAccessExpression(Expression instance, IndexerToken indexerToken)
        {
            return
                Expression.Dynamic(
                    Binder.GetIndex(CSharpBinderFlags.None, typeof(DynamicPropertyAccessExpressionBuilder),
                        new[]
                        {
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                            CSharpArgumentInfo.Create(
                                CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant,
                                null)
                        }), typeof(object), new[]
                        {
                            instance,
                            indexerToken.Arguments.Select(Expression.Constant).First()
                        });
        }

        private Expression CreatePropertyAccessExpression(Expression instance, string propertyName)
        {
            return
                Expression.Dynamic(
                    Binder.GetMember(CSharpBinderFlags.None, propertyName,
                        typeof(DynamicPropertyAccessExpressionBuilder),
                        new[]
                        {
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                        }), typeof(object), new[]
                        {
                            instance
                        });
        }
    }
}