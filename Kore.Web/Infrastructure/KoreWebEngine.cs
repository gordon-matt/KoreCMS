using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Kore.Configuration;
using Kore.Infrastructure.DependencyManagement;
using Kore.Web.Infrastructure.DependencyManagement;

namespace Kore.Infrastructure
{
    public class KoreWebEngine : IEngine
    {
        #region Fields

        private AutofacContainerManager _containerManager;

        #endregion Fields

        #region Ctor

        /// <summary>
        /// Creates an instance of the content engine using default settings and configuration.
        /// </summary>
        public KoreWebEngine()
            : this(new ContainerConfigurer())
        {
        }

        public KoreWebEngine(ContainerConfigurer configurer)
        {
            InitializeContainer(configurer);
        }

        #endregion Ctor

        #region Utilities

        private void RunStartupTasks()
        {
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
            {
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            }
            //sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
            {
                startUpTask.Execute();
            }
        }

        private void InitializeContainer(ContainerConfigurer configurer)
        {
            var builder = new ContainerBuilder();

            var container = builder.Build();

            _containerManager = new AutofacContainerManager(container);
            configurer.Configure(this, _containerManager);

            //_containerManager.NotifyCompleted(container);
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Initialize components and plugins in the Kore environment.
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize()
        {
            //startup tasks
            if (!KoreConfigurationSection.Instance.IgnoreStartupTasks)
            {
                RunStartupTasks();
            }
        }

        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public T Resolve<T>(IDictionary<string, object> ctorArgs) where T : class
        {
            return ContainerManager.Resolve<T>(ctorArgs);
        }

        public T ResolveNamed<T>(string name) where T : class
        {
            return ContainerManager.ResolveNamed<T>(name);
        }

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        public IEnumerable<T> ResolveAllNamed<T>(string name)
        {
            return ContainerManager.ResolveAllNamed<T>(name);
        }

        public bool TryResolve<T>(out T instance)
        {
            return ContainerManager.TryResolve<T>(out instance);
        }

        public bool TryResolve(Type serviceType, out object instance)
        {
            return ContainerManager.TryResolve(serviceType, out instance);
        }

        #endregion Methods

        #region Properties

        public IContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion Properties
    }
}