using System.Web.Http;
using System.Web.Http.Cors;
using Kore.Infrastructure;
using Kore.Web.Infrastructure;

namespace KoreCMS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors(new EnableCorsAttribute("*", "*", "*", "DataServiceVersion, MaxDataServiceVersion") { SupportsCredentials = true });
            //config.Services.Replace(typeof(IAssembliesResolver), new KoreAssembliesResolver());

            // Web API configuration and services

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

            //OData
            //ODataModelBuilder builder = new ODataConventionModelBuilder();
            //builder.EntitySet<Language>("Languages");
            //config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

            //var containerBuilder = new ContainerBuilder();
            //var container = containerBuilder.Build();
            //config.DependencyResolver = new KoreDependencyResolver(container);
        }
    }
}