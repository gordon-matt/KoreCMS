using System.IO;
using System.Text;
using System.Web;

namespace Kore.Web.IO
{
    public class HtmlStringWriter : TextWriter, IHtmlString
    {
        private readonly TextWriter writer;

        public HtmlStringWriter()
        {
            writer = new StringWriter();
        }

        public override Encoding Encoding
        {
            get { return writer.Encoding; }
        }

        public string ToHtmlString()
        {
            return writer.ToString();
        }

        public override string ToString()
        {
            return writer.ToString();
        }

        public override void Write(string value)
        {
            writer.Write(value);
        }

        public override void Write(char value)
        {
            writer.Write(value);
        }
    }
}