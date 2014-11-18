using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Kore.Web.Configuration;
using Module = Autofac.Module;

namespace Kore.Web.Infrastructure
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSource(new SettingsSource());
        }

        public class SettingsSource : IRegistrationSource
        {
            private static readonly MethodInfo buildMethod = typeof(SettingsSource).GetMethod("BuildRegistration",
                BindingFlags.Static | BindingFlags.NonPublic);

            public IEnumerable<IComponentRegistration> RegistrationsFor(
                    Service service,
                    Func<Service, IEnumerable<IComponentRegistration>> registrations)
            {
                var ts = service as TypedService;
                if (ts != null && ts.ServiceType.IsClass && !ts.ServiceType.IsAbstract && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
                {
                    var buildGenericMethod = buildMethod.MakeGenericMethod(ts.ServiceType);
                    yield return (IComponentRegistration)buildGenericMethod.Invoke(null, null);
                }
            }

            internal static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
            {
                return RegistrationBuilder
                    .ForDelegate((c, p) => c.Resolve<ISettingService>().GetSettings<TSettings>())
                    .CreateRegistration();
            }

            public bool IsAdapterForIndividualComponents
            {
                get { return false; }
            }
        }
    }
}