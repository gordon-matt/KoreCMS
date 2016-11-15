using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Kore.Web.UI
{
    public static class HtmlTextWriterExtensions
    {
        public static void AddAttributes(this HtmlTextWriter writer, IDictionary<string, object> attributes)
        {
            foreach (var attribute in attributes)
            {
                writer.AddAttribute(attribute.Key, Convert.ToString(attribute.Value));
            }
        }

        public static void AddAttributeIfHave(this HtmlTextWriter writer, HtmlTextWriterAttribute key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.AddAttribute(key, value);
            }
        }

        public static void AddAttributeIfHave(this HtmlTextWriter writer, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.AddAttribute(key, value);
            }
        }

        public static void AddStyleAttributeIfHave(this HtmlTextWriter writer, HtmlTextWriterStyle key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.AddStyleAttribute(key, value);
            }
        }

        public static void AddStyleAttributeIfHave(this HtmlTextWriter writer, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.AddStyleAttribute(key, value);
            }
        }
    }
}