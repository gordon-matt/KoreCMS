using Autofac;
using Castle.Core.Logging;
using Kore.Infrastructure;

namespace Kore.Logging
{
    public static class LoggingUtilities
    {
        public static ILogger Resolve()
        {
            try
            {
                // Because this code may get run before "Application_Start()"..
                if (EngineContext.Default == null)
                {
                    // .. then we need to check if that's the case. If so, return a null logger.
                    return NullLogger.Instance;
                }

                var componentContext = EngineContext.Current.Resolve<IComponentContext>();
                return componentContext.Resolve<ILogger>(new NamedParameter("name", "defaultLogger"));
            }
            catch
            {
                return NullLogger.Instance;
            }
        }
    }
}