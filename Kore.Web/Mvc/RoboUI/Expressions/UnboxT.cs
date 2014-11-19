using System;
using System.Globalization;
using System.Reflection;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public static class UnboxT<T>
    {
        internal static readonly Converter<object, T> Unbox = Create(typeof(T));

        static UnboxT()
        {
        }

        private static Converter<object, T> Create(Type type)
        {
            if (!type.IsValueType)
                return ReferenceField;
            if (!type.IsGenericType || type.IsGenericTypeDefinition ||
                !(typeof(Nullable<>) == type.GetGenericTypeDefinition()))
                return ValueField;
            return
                (Converter<object, T>)
                    Delegate.CreateDelegate(typeof(Converter<object, T>),
                        typeof(UnboxT<T>).GetMethod("NullableField", BindingFlags.Static | BindingFlags.NonPublic)
                            .MakeGenericMethod(new[]
                            {
                                type.GetGenericArguments()[0]
                            }));
        }

        private static T ReferenceField(object value)
        {
            if (DBNull.Value != value)
                return (T)value;
            return default(T);
        }

        /// <exception cref="T:System.InvalidCastException"><c>InvalidCastException</c>.</exception>
        private static T ValueField(object value)
        {
            if (DBNull.Value != value)
                return (T)value;
            throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture,
                "Type: {0} cannot be casted to Nullable type", new object[]
                {
                    typeof (T)
                }));
        }
    }
}