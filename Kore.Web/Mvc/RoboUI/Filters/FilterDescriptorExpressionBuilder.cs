using System;
using System.Globalization;
using System.Linq.Expressions;
using Kore.Web.Mvc.RoboUI.Expressions;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class FilterDescriptorExpressionBuilder : FilterExpressionBuilder
    {
        private readonly FilterDescriptor descriptor;

        public FilterDescriptorExpressionBuilder(ParameterExpression parameterExpression, FilterDescriptor descriptor)
            : base(parameterExpression)
        {
            this.descriptor = descriptor;
        }

        public FilterDescriptor FilterDescriptor
        {
            get { return descriptor; }
        }

        /// <exception cref="T:System.ArgumentException"><c>ArgumentException</c>.</exception>
        public override Expression CreateBodyExpression()
        {
            Expression memberExpression = CreateMemberExpression();
            Type type = memberExpression.Type;

            Expression valueExpression = CreateValueExpression(type, descriptor.Value, CultureInfo.InvariantCulture);
            bool flag = true;
            if (TypesAreDifferent(descriptor, memberExpression, valueExpression))
            {
                if (!TryConvertExpressionTypes(ref memberExpression, ref valueExpression))
                    flag = false;
            }
            else if (TypeExtensions.IsEnumType(memberExpression.Type) || TypeExtensions.IsEnumType(valueExpression.Type))
            {
                if (!TryPromoteNullableEnums(ref memberExpression, ref valueExpression))
                    flag = false;
            }
            else if (TypeExtensions.IsNullableType(type) && memberExpression.Type != valueExpression.Type &&
                     !TryConvertNullableValue(memberExpression, ref valueExpression))
                flag = false;
            if (flag)
                return descriptor.Operator.CreateExpression(memberExpression, valueExpression,
                    Options.LiftMemberAccessToNull);
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                "Operator '{0}' is incompatible with operand types '{1}' and '{2}'", (object)descriptor.Operator,
                (object)TypeExtensions.GetTypeName(memberExpression.Type),
                (object)TypeExtensions.GetTypeName(valueExpression.Type)));
        }

        public FilterDescription CreateFilterDescription()
        {
            return new PredicateFilterDescription(CreateFilterExpression().Compile());
        }

        protected virtual Expression CreateMemberExpression()
        {
            Type memberType = FilterDescriptor.MemberType;
            MemberAccessExpressionBuilderBase expressionBuilderBase =
                ExpressionBuilderFactory.MemberAccess(ParameterExpression.Type, memberType, FilterDescriptor.Member);
            expressionBuilderBase.Options.CopyFrom(Options);
            expressionBuilderBase.ParameterExpression = ParameterExpression;
            Expression expression = expressionBuilderBase.CreateMemberAccessExpression();
            if (memberType != null &&
                TypeExtensions.GetNonNullableType(expression.Type) != TypeExtensions.GetNonNullableType(memberType))
                expression = Expression.Convert(expression, memberType);
            return expression;
        }

        private static Expression CreateConstantExpression(object value)
        {
            if (value == null)
                return ExpressionConstants.NullLiteral;
            return Expression.Constant(value);
        }

        private static Expression CreateValueExpression(Type targetType, object value, CultureInfo culture)
        {
            if (targetType != typeof(string) && (!targetType.IsValueType || TypeExtensions.IsNullableType(targetType)) &&
                string.Compare(value as string, "null", StringComparison.OrdinalIgnoreCase) == 0)
                value = null;
            if (value != null)
            {
                Type nonNullableType = TypeExtensions.GetNonNullableType(targetType);
                if (value.GetType() != nonNullableType)
                {
                    if (nonNullableType.IsEnum)
                        value = Enum.Parse(nonNullableType, value.ToString(), true);
                    else if (nonNullableType == typeof(Guid))
                        value = new Guid(value.ToString());
                    else if (value is IConvertible)
                        value = Convert.ChangeType(value, nonNullableType, culture);
                }
            }
            return CreateConstantExpression(value);
        }

        private static Expression PromoteExpression(Expression expr, Type type, bool exact)
        {
            if (expr.Type == type)
                return expr;
            var constantExpression = expr as ConstantExpression;
            if (constantExpression != null && constantExpression == ExpressionConstants.NullLiteral &&
                (!type.IsValueType || TypeExtensions.IsNullableType(type)))
                return Expression.Constant(null, type);
            if (!TypeExtensions.IsCompatibleWith(expr.Type, type))
                return null;
            if (type.IsValueType || exact)
                return Expression.Convert(expr, type);
            return expr;
        }

        private static bool TryConvertExpressionTypes(ref Expression memberExpression, ref Expression valueExpression)
        {
            if (memberExpression.Type != valueExpression.Type)
            {
                if (!memberExpression.Type.IsAssignableFrom(valueExpression.Type))
                {
                    if (!valueExpression.Type.IsAssignableFrom(memberExpression.Type))
                        return false;
                    memberExpression = Expression.Convert(memberExpression, valueExpression.Type);
                }
                else
                    valueExpression = Expression.Convert(valueExpression, memberExpression.Type);
            }
            return true;
        }

        private static bool TryConvertNullableValue(Expression memberExpression, ref Expression valueExpression)
        {
            var constantExpression = valueExpression as ConstantExpression;
            if (constantExpression != null)
            {
                try
                {
                    valueExpression = Expression.Constant(constantExpression.Value, memberExpression.Type);
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool TryPromoteNullableEnums(ref Expression memberExpression, ref Expression valueExpression)
        {
            if (memberExpression.Type != valueExpression.Type)
            {
                Expression expression1 = PromoteExpression(valueExpression, memberExpression.Type, true);
                if (expression1 == null)
                {
                    Expression expression2 = PromoteExpression(memberExpression, valueExpression.Type, true);
                    if (expression2 == null)
                        return false;
                    memberExpression = expression2;
                }
                else
                    valueExpression = expression1;
            }
            return true;
        }

        private static bool TypesAreDifferent(FilterDescriptor descriptor, Expression memberExpression,
            Expression valueExpression)
        {
            if ((descriptor.Operator == FilterOperator.IsEqualTo || descriptor.Operator == FilterOperator.IsNotEqualTo) &&
                !memberExpression.Type.IsValueType)
                return !valueExpression.Type.IsValueType;
            return false;
        }
    }
}