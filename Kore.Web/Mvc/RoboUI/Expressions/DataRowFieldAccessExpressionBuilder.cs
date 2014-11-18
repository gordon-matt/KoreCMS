using System;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Kore.Web.Mvc.RoboUI.Filters;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public class DataRowFieldAccessExpressionBuilder : MemberAccessExpressionBuilderBase
    {
        private static readonly MethodInfo dataRowFieldMethod = typeof(DataRowExtensions).GetMethod("Field",
            new[]
            {
                typeof (DataRow),
                typeof (string)
            });

        private readonly Type columnDataType;

        static DataRowFieldAccessExpressionBuilder()
        {
        }

        public DataRowFieldAccessExpressionBuilder(Type memberType, string memberName)
            : base(typeof(DataRow), memberName)
        {
            if (memberType.IsValueType && !TypeExtensions.IsNullableType(memberType))
                columnDataType = typeof(Nullable<>).MakeGenericType(new[]
                {
                    memberType
                });
            else
                columnDataType = memberType;
        }

        public Type ColumnDataType
        {
            get { return columnDataType; }
        }

        public override Expression CreateMemberAccessExpression()
        {
            ConstantExpression constantExpression = Expression.Constant(MemberName);
            return Expression.Call(dataRowFieldMethod.MakeGenericMethod(new[]
            {
                columnDataType
            }), ParameterExpression, constantExpression);
        }
    }
}