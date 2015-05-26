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