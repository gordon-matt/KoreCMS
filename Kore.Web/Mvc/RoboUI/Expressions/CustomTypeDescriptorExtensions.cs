using System;
using System.ComponentModel;
using System.Globalization;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    internal static class CustomTypeDescriptorExtensions
    {
        /// <exception cref="T:System.ArgumentException"><c>ArgumentException</c>.</exception>
        public static T Property<T>(this ICustomTypeDescriptor typeDescriptor, string propertyName)
        {
            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeDescriptor)[propertyName];
            if (propertyDescriptor != null)
                return UnboxT<T>.Unbox(propertyDescriptor.GetValue(typeDescriptor));
            throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture,
                    "Property with specified name: {0} cannot be found on type: {1}", new object[]
                    {
                        propertyName,
                        typeDescriptor.GetType()
                    }), "propertyName");
        }
    }
}