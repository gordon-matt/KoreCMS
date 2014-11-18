using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    internal static class TypeExtensions
    {
        internal static bool IsCompatibleWith(Type source, Type target)
        {
            if (source == target)
                return true;
            if (!target.IsValueType)
                return target.IsAssignableFrom(source);
            Type nonNullableType1 = GetNonNullableType(source);
            Type nonNullableType2 = GetNonNullableType(target);
            if (nonNullableType1 != source && nonNullableType2 == target)
                return false;
            TypeCode typeCode1 = nonNullableType1.IsEnum ? TypeCode.Object : Type.GetTypeCode(nonNullableType1);
            TypeCode typeCode2 = nonNullableType2.IsEnum ? TypeCode.Object : Type.GetTypeCode(nonNullableType2);
            switch (typeCode1)
            {
                case TypeCode.SByte:
                    switch (typeCode2)
                    {
                        case TypeCode.SByte:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;

                case TypeCode.Byte:
                    switch (typeCode2)
                    {
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;

                case TypeCode.Int16:
                    switch (typeCode2)
                    {
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;

                case TypeCode.UInt16:
                    switch (typeCode2)
                    {
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;

                case TypeCode.Int32:
                    switch (typeCode2)
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;

                case TypeCode.UInt32:
                    switch (typeCode2)
                    {
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;

                case TypeCode.Int64:
                    switch (typeCode2)
                    {
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;

                case TypeCode.UInt64:
                    switch (typeCode2)
                    {
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;

                case TypeCode.Single:
                    switch (typeCode2)
                    {
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                    }
                    break;

                default:
                    if (nonNullableType1 == nonNullableType2)
                        return true;
                    break;
            }
            return false;
        }

        internal static bool IsNullableType(Type type)
        {
            if (type.IsGenericType)
                return type.GetGenericTypeDefinition() == typeof(Nullable<>);
            return false;
        }

        internal static Type GetNonNullableType(Type type)
        {
            if (!IsNullableType(type))
                return type;
            return type.GetGenericArguments()[0];
        }

        internal static MemberInfo FindPropertyOrField(Type type, string memberName)
        {
            MemberInfo propertyOrField = FindPropertyOrField(type, memberName, false);
            if (propertyOrField == null)
                propertyOrField = FindPropertyOrField(type, memberName, true);
            return propertyOrField;
        }

        internal static MemberInfo FindPropertyOrField(Type type, string memberName, bool staticAccess)
        {
            var bindingAttr = (BindingFlags)(18 | (staticAccess ? 8 : 4));
            foreach (Type type1 in type.SelfAndBaseTypes())
            {
                MemberInfo[] members = type1.FindMembers(MemberTypes.Field | MemberTypes.Property, bindingAttr,
                    Type.FilterNameIgnoreCase, memberName);
                if (members.Length != 0)
                    return members[0];
            }
            return null;
        }

        internal static IEnumerable<Type> SelfAndBaseTypes(this Type type)
        {
            if (!type.IsInterface)
                return type.SelfAndBaseClasses();
            var types = new List<Type>();
            AddInterface(types, type);
            return types;
        }

        internal static IEnumerable<Type> SelfAndBaseClasses(this Type type)
        {
            for (; type != (Type)null; type = type.BaseType)
                yield return type;
        }

        internal static PropertyInfo GetIndexerPropertyInfo(Type type, params Type[] indexerArguments)
        {
            return type.GetProperties().FirstOrDefault(p => AreArgumentsApplicable(indexerArguments, p.GetIndexParameters()));
        }

        internal static string GetTypeName(Type type)
        {
            Type nonNullableType = GetNonNullableType(type);
            string str = nonNullableType.Name;
            if (type != nonNullableType)
                str = str + '?';
            return str;
        }

        internal static object DefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }

        internal static bool IsEnumType(Type type)
        {
            return GetNonNullableType(type).IsEnum;
        }

        private static bool AreArgumentsApplicable(IEnumerable<Type> arguments, IEnumerable<ParameterInfo> parameters)
        {
            List<Type> list1 = arguments.ToList();
            List<ParameterInfo> list2 = parameters.ToList();
            if (list1.Count != list2.Count)
                return false;
            for (int index = 0; index < list1.Count; ++index)
            {
                if (list2[index].ParameterType != list1[index])
                    return false;
            }
            return true;
        }

        private static void AddInterface(List<Type> types, Type type)
        {
            if (types.Contains(type))
                return;
            types.Add(type);
            foreach (Type type1 in type.GetInterfaces())
                AddInterface(types, type1);
        }
    }
}