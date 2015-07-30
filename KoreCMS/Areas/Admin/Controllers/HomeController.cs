using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc;
using Kore.Web.Security.Membership.Permissions;

namespace KoreCMS.Areas.Admin.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Admin)]
    public class HomeController : KoreController
    {
        private readonly Lazy<IEnumerable<IDurandalRouteProvider>> durandalRouteProviders;
        private readonly Lazy<IEnumerable<IRequireJSConfigProvider>> requireJSConfigProviders;

        public HomeController(
            Lazy<IEnumerable<IDurandalRouteProvider>> durandalRouteProviders,
            Lazy<IEnumerable<IRequireJSConfigProvider>> requireJSConfigProviders)
        {
            this.durandalRouteProviders = durandalRouteProviders;
            this.requireJSConfigProviders = requireJSConfigProviders;
        }

        [Route("")]
        public ActionResult Host()
        {
            return View();
        }

        [Route("dashboard")]
        public ActionResult Dashboard()
        {
            if (!CheckPermission(StandardPermissions.DashboardAccess))
            {
                return new HttpUnauthorizedResult();
            }

            ViewBag.Title = T(LocalizableStrings.Dashboard.Title);

            return View();
        }

        [Route("shell")]
        public ActionResult Shell()
        {
            return View();
        }

        [Route("get-spa-routes")]
        public JsonResult GetSpaRoutes()
        {
            var routes = durandalRouteProviders.Value.SelectMany(x => x.Routes);
            return Json(routes, JsonRequestBehavior.AllowGet);
        }

        [Route("get-requirejs-config")]
        public JsonResult GetRequireJsConfig()
        {
            var config = new RequireJsConfig
            {
                Paths = new Dictionary<string, string>(),
                Shim = new Dictionary<string, string[]>()
            };

            // Routes First
            var routes = durandalRouteProviders.Value.SelectMany(x => x.Routes);

            foreach (var route in routes)
            {
                config.Paths.Add(route.ModuleId, route.JsPath);
            }

            // Then Others
            foreach (var provider in requireJSConfigProviders.Value)
            {
                foreach (var pair in provider.Paths)
                {
                    if (!config.Paths.ContainsKey(pair.Key))
                    {
                        config.Paths.Add(pair.Key, pair.Value);
                    }
                }
                foreach (var pair in provider.Shim)
                {
                    if (!config.Shim.ContainsKey(pair.Key))
                    {
                        config.Shim.Add(pair.Key, pair.Value);
                    }
                }
            }

            return Json(config, JsonRequestBehavior.AllowGet);
        }
    }

    public struct RequireJsConfig
    {
        public Dictionary<string, string> Paths { get; set; }

        public Dictionary<string, string[]> Shim { get; set; }
    }
}