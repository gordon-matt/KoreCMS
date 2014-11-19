using System.Web;
using System.Web.Mvc;

namespace Kore.Web.Mvc.Optimization
{
    public class HtmlMinifierAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;
            response.Filter = new HtmlMinifierFilter(response.Filter);
        }
    }
}