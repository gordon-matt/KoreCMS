//using System;

//namespace Kore.Infrastructure
//{
//    public interface IDIRegistrationBuilder
//    {
//        IDIRegistrationBuilder InstancePerDependency();

//        IDIRegistrationBuilder InstancePerLifetimeScope();

//        IDIRegistrationBuilder InstancePerMatchingLifetimeScope(string lifetimeScopeTag);

//        IDIRegistrationBuilder SingleInstance();

//        IDIRegistrationBuilder Named<TService>(string name);

//        IDIRegistrationBuilder Named(string name, Type serviceType);

//        void Build();
//    }
//}