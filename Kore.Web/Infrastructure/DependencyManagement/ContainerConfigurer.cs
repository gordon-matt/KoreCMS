using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Kore.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Web.Infrastructure.DependencyManagement
{
    /// <summary>
    /// Configures the inversion of control container with services used by Kore.
    /// </summary>
    public class ContainerConfigurer
    {
        public virtual void Configure(IEngine engine, AutofacContainerManager containerManager)
        {
            //other dependencies
            containerManager.AddComponentInstance<IEngine>(engine, "Kore.Engine");
            containerManager.AddComponentInstance<ContainerConfigurer>(this, "Kore.ContainerConfigurer");

            //type finder
            containerManager.AddComponent<ITypeFinder, WebAppTypeFinder>("Kore.TypeFinder");

            //register dependencies provided by other assemblies
            var typeFinder = containerManager.Resolve<ITypeFinder>();
            containerManager.UpdateContainer(x =>
            {
                var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar<ContainerBuilder>>();
                var drInstances = new List<IDependencyRegistrar<ContainerBuilder>>();
                foreach (var drType in drTypes)
                {
                    drInstances.Add((IDependencyRegistrar<ContainerBuilder>)Activator.CreateInstance(drType));
                }
                //sort
                drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
                foreach (var dependencyRegistrar in drInstances)
                {
                    dependencyRegistrar.Register(x, typeFinder);
                }
            });

            containerManager.RegisterWebApi();
        }
    }
}