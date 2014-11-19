using System;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Xml;
using Kore.Web.Mvc.RoboUI.Filters;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public class ExpressionBuilderFactory
    {
        public static MemberAccessExpressionBuilderBase MemberAccess(Type elementType, Type memberType,
            string memberName)
        {
            memberType = memberType ?? typeof(object);
            if (TypeExtensions.IsCompatibleWith(elementType, typeof(DataRow)))
                return new DataRowFieldAccessExpressionBuilder(memberType, memberName);
            if (TypeExtensions.IsCompatibleWith(elementType, typeof(ICustomTypeDescriptor)))
                return new CustomTypeDescriptorPropertyAccessExpressionBuilder(elementType, memberType, memberName);
            if (TypeExtensions.IsCompatibleWith(elementType, typeof(XmlNode)))
                return new XmlNodeChildElementAccessExpressionBuilder(memberName);
            if (elementType == typeof(object) ||
                TypeExtensions.IsCompatibleWith(elementType, typeof(IDynamicMetaObjectProvider)))
                return new DynamicPropertyAccessExpressionBuilder(elementType, memberName);
            return new PropertyAccessExpressionBuilder(elementType, memberName);
        }

        public static MemberAccessExpressionBuilderBase MemberAccess(Type elementType, string memberName,
            bool liftMemberAccess)
        {
            MemberAccessExpressionBuilderBase expressionBuilderBase = MemberAccess(elementType, null, memberName);
            expressionBuilderBase.Options.LiftMemberAccessToNull = liftMemberAccess;
            return expressionBuilderBase;
        }

        public static MemberAccessExpressionBuilderBase MemberAccess(IQueryable source, Type memberType,
            string memberName)
        {
            MemberAccessExpressionBuilderBase expressionBuilderBase = MemberAccess(source.ElementType, memberType,
                memberName);
            expressionBuilderBase.Options.LiftMemberAccessToNull =
                QueryProviderExtensions.IsLinqToObjectsProvider(source.Provider);
            return expressionBuilderBase;
        }
    }
}