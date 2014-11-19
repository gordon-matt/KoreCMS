using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Kore.Exceptions;

namespace Kore.Infrastructure
{
    public class AutofacDIContainerAdapter : IDIContainerAdapter
    {
        public ContainerBuilder Builder { get; private set; }

        public IContainer Container { get; private set; }

        public AutofacDIContainerAdapter(ContainerBuilder builder)
        {
            this.Builder = builder;
        }

        #region IDIContainerAdapter Members

        public string Name
        {
            get { return "Autofac DI Container Adapter"; }
        }

        #region Resolution

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public T Resolve<T>(IDictionary<string, object> ctorArgs)
        {
            var ctorParams = ctorArgs.Select(x => new NamedParameter(x.Key, x.Value)).ToArray();
            return Container.Resolve<T>(ctorParams);
        }

        public T ResolveNamed<T>(string name)
        {
            return Container.ResolveNamed<T>(name);
        }

        public object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return Container.Resolve<IEnumerable<T>>();
        }

        public IEnumerable<T> ResolveAllNamed<T>(string name)
        {
            return Container.ResolveNamed<IEnumerable<T>>(name);
        }

        public T TryResolve<T>()
        {
            T result;

            if (Container.TryResolve<T>(out result))
            {
                return result;
            }

            return default(T);
        }

        public bool TryResolve(Type type, out object instance)
        {
            return Container.TryResolve(type, out instance);
        }

        public object ResolveUnregistered(Type type)
        {
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null) throw new KoreException("Unkown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (KoreException)
                {
                }
            }
            throw new KoreException("No contructor was found that had all the dependencies satisfied.");
        }

        #endregion Resolution

        #region Registration

        //public bool IsRegistered<T>()
        //{
        //    return Container.IsRegistered<T>();
        //}

        //public IDIRegistrationBuilder RegisterType<TImplementation, TService>() where TImplementation : TService
        //{
        //    var registrationBuilder = Builder.RegisterType<TImplementation>().As<TService>();
        //    return new AutofacRegistrationBuilder<TImplementation, ConcreteReflectionActivatorData, SingleRegistrationStyle>(registrationBuilder);
        //}

        //public IDIRegistrationBuilder RegisterType(Type implementation, Type service)
        //{
        //    var registrationBuilder = Builder.RegisterType(implementation).As(service);
        //    return new AutofacRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(registrationBuilder);
        //}

        //public IDIRegistrationBuilder RegisterOpenGeneric(Type implementation, Type service)
        //{
        //    var registrationBuilder = Builder.RegisterGeneric(implementation).As(service);
        //    return new AutofacRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle>(registrationBuilder);
        //}

        //public IDIRegistrationBuilder RegisterByInterface<TService>(IEnumerable<Assembly> assemblies)
        //{
        //    var registrationBuilder = Builder.RegisterAssemblyTypes(assemblies.ToArray())
        //        .Where(t => t.GetInterfaces()
        //            .Any(i => i.IsAssignableFrom(typeof(TService))))
        //        .AsImplementedInterfaces();

        //    return new AutofacRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>(registrationBuilder);
        //}

        protected virtual void RegisterExternalDependencies()
        {
            var typeFinder = Container.Resolve<ITypeFinder>();

            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar<ContainerBuilder>>();
            var drInstances = new List<IDependencyRegistrar<ContainerBuilder>>();

            var newBuilder = new ContainerBuilder();

            foreach (var drType in drTypes)
            {
                drInstances.Add((IDependencyRegistrar<ContainerBuilder>)Activator.CreateInstance(drType));
            }

            foreach (var dependencyRegistrar in drInstances)
            {
                dependencyRegistrar.Register(newBuilder, typeFinder);
            }

            newBuilder.RegisterAssemblyModules(typeFinder.GetAssemblies().ToArray());
            newBuilder.Update(Container);
        }

        protected virtual void RegisterModules()
        {
            var typeFinder = Container.Resolve<ITypeFinder>();
            var newBuilder = new ContainerBuilder();
            newBuilder.RegisterAssemblyModules(typeFinder.GetAssemblies().ToArray());
            newBuilder.Update(Container);
        }

        public void Build()
        {
            Container = Builder.Build();
            RegisterExternalDependencies();
            RegisterModules();
        }

        #endregion Registration

        #endregion IDIContainerAdapter Members
    }
}