using System;
using System.Web.Mvc;

namespace Kore.Web.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string AbsoluteAction(this UrlHelper url, string actionName, string controllerName, object routeValues)
        {
            var requestUrl = url.RequestContext.HttpContext.Request.Url;

            if (requestUrl == null)
            {
                return null;
            }

            var absoluteAction = string.Format("{0}{1}",
                    requestUrl.GetLeftPart(UriPartial.Authority),
                    url.Action(actionName, controllerName, routeValues));

            return absoluteAction;
        }

        public static string AbsoluteContent(this UrlHelper url, string contentUrl)
        {
            var requestUrl = url.RequestContext.HttpContext.Request.Url;

            if (requestUrl == null)
            {
                return null;
            }

            return string.Format("{0}{1}",
                requestUrl.GetLeftPart(UriPartial.Authority),
                url.Content(contentUrl));
        }
    }
}