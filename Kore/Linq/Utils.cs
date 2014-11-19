using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kore.Linq
{
    public static class Utils
    {
        public static string GetFullPropertyName<TElement, TValue>(Expression<Func<TElement, TValue>> expression)
        {
            MemberExpression me;
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expression.Body as UnaryExpression;
                    me = ((ue != null) ? ue.Operand : null) as MemberExpression;
                    break;

                default:
                    me = expression.Body as MemberExpression;
                    break;
            }

            var names = new List<string>();

            while (me != null)
            {
                names.Add(me.Member.Name);
                me = me.Expression as MemberExpression;
            }

            names.Reverse();
            return string.Join(".", names);
        }
    }
}