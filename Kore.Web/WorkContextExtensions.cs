//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Http.Controllers;
//using System.Web.Mvc;
//using System.Web.Routing;

//namespace Kore.Web
//{
//    public static class WorkContextExtensions
//    {
//        public static WorkContext GetContext(this IWorkContext workContextAccessor, ControllerContext controllerContext)
//        {
//            return workContextAccessor.GetContext(controllerContext.RequestContext.HttpContext);
//        }

//        public static WorkContext GetWorkContext(this RequestContext requestContext)
//        {
//            if (requestContext == null)
//                return null;

//            var routeData = requestContext.RouteData;
//            if (routeData == null || routeData.DataTokens == null)
//                return null;

//            object workContextValue;
//            if (!routeData.DataTokens.TryGetValue("IWorkContext", out workContextValue))
//            {
//                workContextValue = FindWorkContextInParent(routeData);
//            }

//            if (!(workContextValue is IWorkContext))
//                return null;

//            var workContextAccessor = (IWorkContext)workContextValue;
//            return workContextAccessor.GetContext(requestContext.HttpContext);
//        }

//        public static WorkContext GetWorkContext(this HttpControllerContext controllerContext)
//        {
//            if (controllerContext == null)
//                return null;

//            var routeData = controllerContext.RouteData;
//            if (routeData == null || routeData.Values == null)
//                return null;

//            object workContextValue;
//            if (!routeData.Values.TryGetValue("IWorkContext", out workContextValue))
//            {
//                return null;
//            }

//            if (workContextValue == null || !(workContextValue is IWorkContext))
//                return null;

//            var workContextAccessor = (IWorkContext)workContextValue;
//            return workContextAccessor.GetContext();
//        }

//        public static WorkContext GetWorkContext(this ControllerContext controllerContext)
//        {
//            if (controllerContext == null)
//                return null;

//            return GetWorkContext(controllerContext.RequestContext);
//        }
//    }
//}