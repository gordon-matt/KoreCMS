using System.Web;

namespace Kore.Web.Fakes
{
    public class FakeHttpContext2 : HttpContextWrapper
    {
        private readonly HttpResponseBase response;

        public FakeHttpContext2()
            : base(new HttpContext(new FakeHttpWorkerRequest()))
        {
        }

        public FakeHttpContext2(HttpContextBase httpContext)
            : base(httpContext.ApplicationInstance.Context)
        {
            response = new FakeHttpResponse2();
        }

        public override HttpResponseBase Response
        {
            get { return response ?? base.Response; }
        }
    }
}