using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Kore.Web
{
    public static class ObjectExtensions
    {
        public static string ToJson<T>(this T item)
        {
            return new JavaScriptSerializer().Serialize(item);
        }

        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            return HtmlHelper.AnonymousObjectToHtmlAttributes(obj);
        }

        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
            {
                expando.Add(item);
            }
            return (ExpandoObject)expando;
        }
    }
}