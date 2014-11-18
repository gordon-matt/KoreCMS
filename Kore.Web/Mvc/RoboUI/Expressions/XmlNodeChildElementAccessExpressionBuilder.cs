using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public class XmlNodeChildElementAccessExpressionBuilder : MemberAccessExpressionBuilderBase
    {
        private static readonly MethodInfo childElementInnerTextMethod =
            typeof(XmlNodeExtensions).GetMethod("ChildElementInnerText", new[]
            {
                typeof (XmlNode),
                typeof (string)
            });

        static XmlNodeChildElementAccessExpressionBuilder()
        {
        }

        public XmlNodeChildElementAccessExpressionBuilder(string memberName)
            : base(typeof(XmlNode), memberName)
        {
        }

        public override Expression CreateMemberAccessExpression()
        {
            ConstantExpression constantExpression = Expression.Constant(MemberName);
            return Expression.Call(childElementInnerTextMethod, ParameterExpression, constantExpression);
        }
    }
}