using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kore.Linq
{
    public static class Utils
    {
        public static string GetFullPropertyName<TElement, TValue>(Expression<Func<TElement, TValue>> expression)
        {
            MemberExpression memberExpression;
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expression.Body as UnaryExpression;
                    memberExpression = ((ue != null) ? ue.Operand : null) as MemberExpression;
                    break;

                default:
                    memberExpression = expression.Body as MemberExpression;
                    break;
            }

            var names = new List<string>();

            while (memberExpression != null)
            {
                names.Add(memberExpression.Member.Name);
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            names.Reverse();
            return string.Join(".", names);
        }
    }
}