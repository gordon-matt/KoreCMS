using System.Linq;
using System.Reflection;
using Autofac.Core;
using Castle.Core.Logging;

namespace Kore.Logging
{
    public class LoggingModule : Autofac.Module
    {
        private static void InjectLoggerProperties(object instance)
        {
            var instanceType = instance.GetType();

            // Get all the injectable properties to set.
            // If you wanted to ensure the properties were only UNSET properties,
            // here's where you'd do it.
            var properties = instanceType
              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
              .Where(p => p.PropertyType == typeof(ILogger) && p.CanWrite && p.GetIndexParameters().Length == 0);

            // Set the properties located.
            foreach (var propToSet in properties)
            {
                propToSet.SetValue(instance, new NLogFilteredLogger(instanceType.FullName), null);
            }
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(
                new[]
                {
                    new ResolvedParameter((p, i) => p.ParameterType == typeof (ILogger),
                        (p, i) => new NLogFilteredLogger(t.FullName))
                });
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            // Handle constructor parameters.
            registration.Preparing += OnComponentPreparing;

            // Handle properties.
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }

        //private readonly ConcurrentDictionary<string, ILogger> loggerCache;

        //public LoggingModule()
        //{
        //    loggerCache = new ConcurrentDictionary<string, ILogger>();
        //}

        //protected override void Load(ContainerBuilder moduleBuilder)
        //{
        //    // by default, use Kore's logger that delegates to Castle's logger factory
        //    moduleBuilder.RegisterType<NLogLoggerFactory>().As<ILoggerFactory>().InstancePerLifetimeScope();
        //    // call CreateLogger in response to the request for an ILogger implementation
        //    moduleBuilder.Register(CreateLogger).As<ILogger>().InstancePerDependency();
        //}

        //protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        //{
        //    var implementationType = registration.Activator.LimitType;
        //    // build an array of actions on this type to assign loggers to member properties
        //    var injectors = BuildLoggerInjectors(implementationType).ToArray();
        //    // if there are no logger properties, there's no reason to hook the activated event
        //    if (!injectors.Any())
        //        return;
        //    // otherwise, whan an instance of this component is activated, inject the loggers on the instance
        //    registration.Activated += (s, e) =>
        //    {
        //        foreach (var injector in injectors)
        //            injector(e.Context, e.Instance);
        //    };
        //}

        //private IEnumerable<Action<IComponentContext, object>> BuildLoggerInjectors(Type componentType)
        //{
        //    // Look for settable properties of type "ILogger"
        //    var loggerProperties = componentType
        //    .GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
        //    .Select(p => new
        //    {
        //        PropertyInfo = p,
        //        p.PropertyType,
        //        IndexParameters = p.GetIndexParameters(),
        //        Accessors = p.GetAccessors(false)
        //    })
        //    .Where(x => x.PropertyType == typeof(ILogger)) // must be a logger
        //    .Where(x => x.IndexParameters.Count() == 0) // must not be an indexer
        //    .Where(x => x.Accessors.Length != 1 || x.Accessors[0].ReturnType == typeof(void)); //must have get/set, or only set
        //    // Return an array of actions that resolve a logger and assign the property
        //    foreach (var entry in loggerProperties)
        //    {
        //        var propertyInfo = entry.PropertyInfo;
        //        yield return (ctx, instance) =>
        //        {
        //            string component = componentType.ToString();
        //            var logger = loggerCache.GetOrAdd(component, key => ctx.Resolve<ILogger>(new TypedParameter(typeof(Type), componentType)));
        //            propertyInfo.SetValue(instance, logger, null);
        //        };
        //    }
        //}

        //private static ILogger CreateLogger(IComponentContext context, IEnumerable<Parameter> parameters)
        //{
        //    // return an ILogger in response to Resolve<ILogger>(componentTypeParameter)
        //    var loggerFactory = context.Resolve<ILoggerFactory>();
        //    var containingType = parameters.TypedAs<Type>();
        //    return loggerFactory.Create(containingType);
        //}
    }
}