using System.Web.Script.Serialization;
using Kore.Web.Mvc.JQGrid.Utility;

namespace Kore.Web.Mvc.JQGrid.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj, int? recursionDepth = null)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new NullPropertiesConverter() });

            if (recursionDepth.HasValue)
            {
                serializer.RecursionLimit = recursionDepth.Value;
            }

            return serializer.Serialize(obj);
        }
    }
}