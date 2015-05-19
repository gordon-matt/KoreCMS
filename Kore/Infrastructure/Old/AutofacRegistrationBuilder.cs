//using System;
//using Autofac.Builder;

//namespace Kore.Infrastructure
//{
//    public class AutofacRegistrationBuilder<TImplementation, TActivatorData, TRegistrationStyle> : IDIRegistrationBuilder
//    {
//        private IRegistrationBuilder<TImplementation, TActivatorData, TRegistrationStyle> builder;

//        public AutofacRegistrationBuilder(IRegistrationBuilder<TImplementation, TActivatorData, TRegistrationStyle> builder)
//        {
//            this.builder = builder;
//        }

//        #region IDIRegistrationBuilder Members

//        public IDIRegistrationBuilder InstancePerDependency()
//        {
//            builder = builder.InstancePerDependency();
//            return this;
//        }

//        public IDIRegistrationBuilder InstancePerLifetimeScope()
//        {
//            builder = builder.InstancePerLifetimeScope();
//            return this;
//        }

//        public IDIRegistrationBuilder InstancePerMatchingLifetimeScope(string lifetimeScopeTag)
//        {
//            builder = builder.InstancePerMatchingLifetimeScope(lifetimeScopeTag);
//            return this;
//        }

//        public IDIRegistrationBuilder SingleInstance()
//        {
//            builder = builder.SingleInstance();
//            return this;
//        }

//        public IDIRegistrationBuilder Named<TService>(string name)
//        {
//            builder = builder.Named<TService>(name);
//            return this;
//        }

//        public IDIRegistrationBuilder Named(string name, Type serviceType)
//        {
//            builder = builder.Named(name, serviceType);
//            return this;
//        }

//        public void Build()
//        {
//            //Do Nothing
//        }

//        #endregion IDIRegistrationBuilder Members
//    }
//}