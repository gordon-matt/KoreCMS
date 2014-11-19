using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Kore.Data;

namespace Kore.Reflection
{
    public static class TypeExtensions
    {
        public static object GetDefaultValue(this Type type)
        {
            if (type == typeof(Boolean)) return default(Boolean);
            if (type == typeof(Byte)) return default(Byte);
            if (type == typeof(Char)) return default(Char);
            if (type == typeof(Int16)) return default(Int16);
            if (type == typeof(Int32)) return default(Int32);
            if (type == typeof(Int64)) return default(Int64);
            if (type == typeof(Decimal)) return default(Decimal);
            if (type == typeof(Double)) return default(Double);
            if (type == typeof(DateTime)) return default(DateTime);
            if (type == typeof(Guid)) return default(Guid);
            if (type == typeof(Single)) return default(Single);
            if (type == typeof(String)) return default(String);
            if (type == typeof(SByte)) return default(SByte);
            if (type == typeof(TimeSpan)) return default(TimeSpan);
            if (type == typeof(UInt16)) return default(UInt16);
            if (type == typeof(UInt32)) return default(UInt32);
            if (type == typeof(UInt64)) return default(UInt64);
            if (type == typeof(Uri)) return default(Uri);
            if (type.IsEnum) return 0;
            return null;
        }

        public static IEnumerable<MethodInfo> GetExtensionMethods(this Type type, Assembly extensionsAssembly)
        {
            var query = from t in extensionsAssembly.GetTypes()
                        where !t.IsGenericType && !t.IsNested
                        from m in t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                        where m.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false)
                        where m.GetParameters()[0].ParameterType == type
                        select m;

            return query;
        }

        public static MethodInfo GetExtensionMethod(this Type type, Assembly extensionsAssembly, string name)
        {
            return type.GetExtensionMethods(extensionsAssembly).FirstOrDefault(m => m.Name == name);
        }

        public static MethodInfo GetExtensionMethod(this Type type, Assembly extensionsAssembly, string name, Type[] types)
        {
            var methods = (from m in type.GetExtensionMethods(extensionsAssembly)
                           where m.Name == name
                           && m.GetParameters().Count() == types.Length + 1 // + 1 because extension method parameter (this)
                           select m).ToList();

            if (!methods.Any())
            {
                return default(MethodInfo);
            }

            if (methods.Count() == 1)
            {
                return methods.First();
            }

            foreach (var methodInfo in methods)
            {
                var parameters = methodInfo.GetParameters();

                bool found = true;
                for (byte b = 0; b < types.Length; b++)
                {
                    found = true;
                    if (parameters[b].GetType() != types[b])
                    {
                        found = false;
                    }
                }

                if (found)
                {
                    return methodInfo;
                }
            }

            return default(MethodInfo);
        }

        public static bool IsCollection(this Type type)
        {
            // string implements IEnumerable, but for our purposes we don't consider it a collection.
            if (type == typeof(string)) return false;

            var interfaces = from inf in type.GetInterfaces()
                             where inf == typeof(IEnumerable) ||
                                 (inf.IsGenericType && inf.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                             select inf;
            return interfaces.Count() != 0;
        }

        public static bool IsNullable(this Type type)
        {
            if (type == null)
            {
                return false;
            }
            if (!type.IsValueType)
            {
                return true;
            }

            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Nullable<>);

            //return Nullable.GetUnderlyingType(type) != null; //faster than above version? Needs testing
        }

        public static bool IsNumeric(this Type type)
        {
            if (type.In(typeof(Byte), typeof(Decimal), typeof(Double),
                typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte), typeof(Single),
                typeof(UInt16), typeof(UInt32), typeof(UInt64)))
            {
                return true;
            }
            return false;
        }

        public static DbType ToDbType(this Type type)
        {
            return DataTypeConvertor.GetDbType(type);
        }

        public static SqlDbType ToSqlDbType(this Type type)
        {
            return DataTypeConvertor.GetSqlDbType(type);
        }

        public static Type ToNullable(this Type type)
        {
            if (type == null)
            {
                return null;
            }
            if (type.IsNullable())
            {
                return type;
            }
            if (type.IsValueType && type != typeof(void))
            {
                return typeof(Nullable<>).MakeGenericType(type);
            }
            return null;
        }
    }
}