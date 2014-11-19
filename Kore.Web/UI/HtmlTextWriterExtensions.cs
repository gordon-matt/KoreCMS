using System.Web.UI;

namespace Kore.Web.UI
{
    public static class HtmlTextWriterExtensions
    {
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