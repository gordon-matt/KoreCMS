using System;
using System.Collections.Generic;

namespace Kore.Infrastructure
{
    public interface IDIContainerAdapter
    {
        string Name { get; }

        #region Resolution

        T Resolve<T>();

        T Resolve<T>(IDictionary<string, object> ctorArgs);

        T ResolveNamed<T>(string name);

        object Resolve(Type type);

        IEnumerable<T> ResolveAll<T>();

        IEnumerable<T> ResolveAllNamed<T>(string name);

        T TryResolve<T>();

        bool TryResolve(Type type, out object instance);

        object ResolveUnregistered(Type type);

        #endregion Resolution

        #region Registration

        //bool IsRegistered<T>();

        //IDIRegistrationBuilder RegisterType<TImplementation, TService>() where TImplementation : TService;

        //IDIRegistrationBuilder RegisterType(Type implementation, Type service);

        //IDIRegistrationBuilder RegisterOpenGeneric(Type implementation, Type service);

        //IDIRegistrationBuilder RegisterByInterface<TService>(IEnumerable<Assembly> assemblies);

        //void RegisterModules();

        void Build();

        #endregion Registration
    }
}