using System.Web;

namespace Kore.Web.Environment
{
    public interface IHttpContextAccessor
    {
        HttpContextBase Current();
    }

    public class HttpContextAccessor : IHttpContextAccessor
    {
        public HttpContextBase Current()
        {
            var httpContext = GetStaticProperty();
            if (httpContext == null)
                return null;
            return new HttpContextWrapper(httpContext);
        }

        private static HttpContext GetStaticProperty()
        {
            return HttpContext.Current;
        }
    }
}