using System;
using Kore.Web.Environment;

namespace Kore.Web
{
    public class HttpContextStateProvider : IWorkContextStateProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HttpContextStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        #region IWorkContextStateProvider Members

        public Func<IWebWorkContext, T> Get<T>(string name)
        {
            if (name == "HttpContext")
            {
                var result = (T)(object)httpContextAccessor.Current();
                return ctx => result;
            }
            return null;
        }

        #endregion IWorkContextStateProvider Members
    }
}