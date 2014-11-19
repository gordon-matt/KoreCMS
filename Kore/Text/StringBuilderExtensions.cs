using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kore.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder Append(this StringBuilder stringBuilder,
            params string[] values)
        {
            foreach (string value in values)
            {
                stringBuilder.Append(value);
            }
            return stringBuilder;
        }

        public static StringBuilder Append(this StringBuilder stringBuilder,
            params object[] values)
        {
            foreach (object value in values)
            {
                stringBuilder.Append(value);
            }
            return stringBuilder;
        }

        public static StringBuilder Append<T>(this StringBuilder stringBuilder,
            IEnumerable<T> values)
        {
            return stringBuilder.Append(string.Join("", values));
        }

        public static StringBuilder Append<T>(this StringBuilder stringBuilder,
            IEnumerable<T> values, Func<T, object> selector)
        {
            return stringBuilder.Append(string.Join("", values.Select(selector)));
        }

        public static StringBuilder Append<T>(this StringBuilder stringBuilder,
            IEnumerable<T> values, string separator)
        {
            return stringBuilder.Append(string.Join(separator, values));
        }

        public static StringBuilder Append<T>(this StringBuilder stringBuilder,
            IEnumerable<T> values, string separator, Func<T, object> selector)
        {
            return stringBuilder.Append(string.Join(separator, values.Select(selector)));
        }
    }
}