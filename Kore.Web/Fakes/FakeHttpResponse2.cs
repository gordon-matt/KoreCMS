using System;
using System.IO;
using System.Text;
using System.Web;

namespace Kore.Web.Fakes
{
    public class FakeHttpResponse2 : HttpResponseBase
    {
        private readonly HttpCookieCollection cookies;
        private readonly TextWriter stringWriter;
        private readonly StringBuilder stringBuilder;

        public FakeHttpResponse2()
        {
            stringBuilder = new StringBuilder();
            stringWriter = new StringWriter(stringBuilder);
            cookies = new HttpCookieCollection();
        }

        public override int StatusCode { get; set; }

        public override string RedirectLocation { get; set; }

        public override HttpCookieCollection Cookies
        {
            get { return cookies; }
        }

        public override TextWriter Output
        {
            get { return stringWriter; }
            set { throw new NotSupportedException(); }
        }

        public override Stream OutputStream
        {
            get { return new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString())); }
        }

        public override void Write(string s)
        {
            stringWriter.Write(s);
        }

        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }
    }
}