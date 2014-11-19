using System.Web.Http;

namespace Kore.Web.Infrastructure
{
    public interface IWebApiRegistrar
    {
        void Register(HttpConfiguration config);
    }
}