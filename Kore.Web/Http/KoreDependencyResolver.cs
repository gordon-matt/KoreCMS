//using System;
//using System.Collections.Generic;
//using System.Security;
//using System.Web.Http.Dependencies;
//using Autofac;
//using Autofac.Core.Lifetime;
//using Autofac.Integration.WebApi;
//using Kore.Infrastructure;

//namespace Kore.Web.Http.Dispatcher
//{
//    public class KoreDependencyResolver : IDependencyResolver
//    {
//        private bool _disposed;
//        private readonly ILifetimeScope _container;
//        private readonly IDependencyScope _rootDependencyScope;

//        public ILifetimeScope Container
//        {
//            get { return this._container; }
//        }

//        public KoreDependencyResolver(ILifetimeScope container)
//        {
//            if (container == null)
//                throw new ArgumentNullException("container");
//            this._container = container;
//            this._rootDependencyScope = (IDependencyScope)new AutofacWebApiDependencyScope(container);
//        }

//        [SecuritySafeCritical]
//        ~KoreDependencyResolver()
//        {
//            this.Dispose(false);
//        }

//        #region IDependencyResolver Members

//        [SecurityCritical]
//        public IDependencyScope BeginScope()
//        {
//            return (IDependencyScope)new AutofacWebApiDependencyScope(
//                this._container.BeginLifetimeScope((object)MatchingScopeLifetimeTags.RequestLifetimeScopeTag));
//        }

//        #endregion IDependencyResolver Members

//        #region IDependencyScope Members

//        [SecurityCritical]
//        public object GetService(Type serviceType)
//        {
//            return EngineContext.Current.ContainerManager.ResolveOptional(serviceType);
//        }

//        [SecurityCritical]
//        public IEnumerable<object> GetServices(Type serviceType)
//        {
//            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
//            return (IEnumerable<object>)EngineContext.Current.Resolve(type);
//        }

//        #endregion IDependencyScope Members

//        #region IDisposable Members

//        public void Dispose()
//        {
//        }

//        private void Dispose(bool disposing)
//        {
//            if (this._disposed)
//                return;
//            if (disposing && this._rootDependencyScope != null)
//                ((IDisposable)this._rootDependencyScope).Dispose();
//            this._disposed = true;
//        }

//        #endregion IDisposable Members
//    }
//}