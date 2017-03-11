using System.Web.Http;
using Kore.Infrastructure;
using Kore.Web.Infrastructure;

namespace KoreCMS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            var registrars = EngineContext.Current.ResolveAll<IWebApiRegistrar>();

            foreach (var registrar in registrars)
            {
                registrar.Register(config);
            }

            config.Routes.MapHttpRoute(
                name: "DefaultApiWithAction",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}