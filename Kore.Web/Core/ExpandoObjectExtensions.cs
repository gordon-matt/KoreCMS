using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Web.Script.Serialization;

namespace Kore.Web
{
    public static class ExpandoObjectExtensions
    {
        public static string ToJson(this ExpandoObject expando)
        {
            var serializer = new JavaScriptSerializer();
            var json = new StringBuilder();
            var keyPairs = new List<string>();
            IDictionary<string, object> dictionary = expando;
            json.Append("{");

            foreach (var pair in dictionary)
            {
                if (pair.Value is ExpandoObject)
                {
                    keyPairs.Add(String.Format(@"""{0}"": {1}", pair.Key, (pair.Value as ExpandoObject).ToJson()));
                }
                else
                {
                    keyPairs.Add(String.Format(@"""{0}"": {1}", pair.Key, serializer.Serialize(pair.Value)));
                }
            }

            json.Append(String.Join(",", keyPairs.ToArray()));
            json.Append("}");

            return json.ToString();
        }
    }
}