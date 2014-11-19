using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Kore.Web.Mvc.RoboUI.Filters;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public class CustomTypeDescriptorPropertyAccessExpressionBuilder : MemberAccessExpressionBuilderBase
    {
        private static readonly MethodInfo propertyMethod = typeof(CustomTypeDescriptorExtensions).GetMethod("Property");
        private readonly Type propertyType;

        static CustomTypeDescriptorPropertyAccessExpressionBuilder()
        {
        }

        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="elementType" /> did not implement
        ///     <see cref="T:System.ComponentModel.ICustomTypeDescriptor" />.
        /// </exception>
        public CustomTypeDescriptorPropertyAccessExpressionBuilder(Type elementType, Type memberType, string memberName)
            : base(elementType, memberName)
        {
            if (!TypeExtensions.IsCompatibleWith(elementType, typeof(ICustomTypeDescriptor)))
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, "ElementType: {0} did not implement {1}", new object[]
                    {
                        elementType,
                        typeof (ICustomTypeDescriptor)
                    }), "elementType");
            propertyType = GetPropertyType(memberType);
        }

        public Type PropertyType
        {
            get { return propertyType; }
        }

        private Type GetPropertyType(Type memberType)
        {
            Type descriptorProvider = GetPropertyTypeFromTypeDescriptorProvider();
            if (descriptorProvider != null)
                memberType = descriptorProvider;
            if (!memberType.IsValueType || TypeExtensions.IsNullableType(memberType))
                return memberType;
            return typeof(Nullable<>).MakeGenericType(new[]
            {
                memberType
            });
        }

        private Type GetPropertyTypeFromTypeDescriptorProvider()
        {
            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(ItemType)[MemberName];
            if (propertyDescriptor != null)
                return propertyDescriptor.PropertyType;
            return null;
        }

        public override Expression CreateMemberAccessExpression()
        {
            ConstantExpression constantExpression = Expression.Constant(MemberName);
            return Expression.Call(propertyMethod.MakeGenericMethod(new[]
            {
                propertyType
            }), ParameterExpression, constantExpression);
        }
    }
}