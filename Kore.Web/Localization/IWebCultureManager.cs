using System.Web;
using Kore.Localization;

namespace Kore.Web.Localization
{
    public interface IWebCultureManager : ICultureManager
    {
        string GetCurrentCulture(HttpContextBase requestContext);
    }
}