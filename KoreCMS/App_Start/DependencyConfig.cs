namespace KoreCMS
{
    public static class DependencyConfig
    {
        public static void Register()
        {
            //    var builder = new ContainerBuilder();
            //    builder.RegisterControllers(typeof(MvcApplication).Assembly);
            //    builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            //builder.RegisterType<WebAppTypeFinder>()
            //    .As<ITypeFinder>()
            //    .SingleInstance()
            //    .Keyed<ITypeFinder>("Kore.TypeFinder");

            //var adapter = new AutofacDIContainerAdapter(builder);
            //EngineContext.Current = adapter;
            //EngineContext.Current.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(adapter.Container));

            //var typeFinder = EngineContext.Current.Resolve<ITypeFinder>();
            //var assemblies = typeFinder.GetAssemblies().ToArray();
            //builder.RegisterControllers(assemblies);
            //builder.RegisterApiControllers(assemblies);
        }
    }
}