using System;
using Autofac;

namespace Kore.Demos.ConsoleApp.Infrastructure
{
    public static class AutofacRequestLifetimeHelper
    {
        private static ILifetimeScope lifetimeScope;
        private static readonly object RequestTag = "AutofacConsoleRequest";

        public static ILifetimeScope GetLifetimeScope(ILifetimeScope container, Action<ContainerBuilder> configurationAction)
        {
            return lifetimeScope ?? (lifetimeScope = InitializeLifetimeScope(configurationAction, container));
        }

        private static ILifetimeScope LifetimeScope
        {
            get { return lifetimeScope; }
            set { lifetimeScope = value; }
        }

        private static ILifetimeScope InitializeLifetimeScope(Action<ContainerBuilder> configurationAction, ILifetimeScope container)
        {
            return (configurationAction == null)
                ? container.BeginLifetimeScope(RequestTag)
                : container.BeginLifetimeScope(RequestTag, configurationAction);
        }

        public static void Dispose()
        {
            if (lifetimeScope != null)
            {
                lifetimeScope.Dispose();
            }
        }
    }
}