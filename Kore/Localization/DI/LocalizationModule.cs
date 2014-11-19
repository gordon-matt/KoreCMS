using System;
using System.Collections.Concurrent;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Module = Autofac.Module;

namespace Kore.Localization.DI
{
    public class LocalizationModule : Module
    {
        private readonly ConcurrentDictionary<string, Localizer> localizerCache;

        public LocalizationModule()
        {
            localizerCache = new ConcurrentDictionary<string, Localizer>();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Text>().As<IText>().InstancePerDependency();
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            var userProperty = FindUserProperty(registration.Activator.LimitType);

            if (userProperty != null)
            {
                var scope = registration.Activator.LimitType.FullName;

                registration.Activated += (sender, e) =>
                {
                    var localizer = localizerCache.GetOrAdd(scope, key => LocalizationUtilities.Resolve(scope));
                    userProperty.SetValue(e.Instance, localizer, null);
                };
            }
        }

        private static PropertyInfo FindUserProperty(Type type)
        {
            return type.GetProperty("T", typeof(Localizer));
        }
    }
}