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
                    .ForDelegate((c, p) =>
                    {
                        var currentTenantId = c.Resolve<IWorkContext>().CurrentTenant.Id;
                        // Uncomment the code below if you want load settings per tenant only when you have two tenants installed.
                        //var currentTenantId = c.Resolve<IWorkContext>().GetAllTenants().Count > 1
                        //    c.Resolve<IWorkContext>().CurrentTenant.Id : 0;

                        // Although it's better to connect to your database and execute the following SQL:
                        //DELETE FROM [Setting] WHERE [TenantId] > 0
                        return c.Resolve<ISettingService>().GetSettings<TSettings>(currentTenantId);
                    })
                    .InstancePerLifetimeScope()
                    .CreateRegistration();
            }

            public bool IsAdapterForIndividualComponents => false;
        }
    }
}