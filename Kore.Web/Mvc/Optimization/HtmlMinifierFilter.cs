using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Kore.Web.Mvc.Optimization
{
    public class HtmlMinifierFilter : MemoryStream
    {
        private static readonly Regex regexRemoveWhitespace = new Regex(">[\r\n][ \r\n\t]*<", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex regexRemoveWhitespace2 = new Regex(">[ \r\n\t]+<", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly Stream response;

        public HtmlMinifierFilter(Stream response)
        {
            this.response = response;
        }

        private static string ReplaceTags(string html)
        {
            return regexRemoveWhitespace2.Replace(regexRemoveWhitespace.Replace(html, "><"), "> <");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string s = ReplaceTags(Encoding.UTF8.GetString(buffer));
            buffer = Encoding.UTF8.GetBytes(s);
            response.Write(buffer, offset, buffer.Length);
        }
    }
}