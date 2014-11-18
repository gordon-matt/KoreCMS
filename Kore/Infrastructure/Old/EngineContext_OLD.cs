//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Web;
//using Autofac;
//using Kore.Configuration;

//namespace Kore.Infrastructure
//{
//    public static class EngineContext
//    {
//        //private static IDIContainerAdapter adapter;

//        //public static IDIContainerAdapter Current
//        //{
//        //    get { return adapter; }
//        //    set
//        //    {
//        //        adapter = value;
//        //        adapter.Build();
//        //    }
//        //}

//        public static IDIContainerAdapter Current
//        {
//            get
//            {
//                if (Singleton<IDIContainerAdapter>.Instance == null)
//                {
//                    Initialize(false);
//                }
//                return Singleton<IDIContainerAdapter>.Instance;
//            }
//            set
//            {
//                Replace(value);
//            }
//        }

//        [MethodImpl(MethodImplOptions.Synchronized)]
//        public static IDIContainerAdapter Initialize(bool forceRecreate)
//        {
//            if (Singleton<IDIContainerAdapter>.Instance == null || forceRecreate)
//            {
//                Debug.WriteLine("Constructing engine " + DateTime.Now);
//                Singleton<IDIContainerAdapter>.Instance = CreateEngineInstance();
//                Debug.WriteLine("Initializing engine " + DateTime.Now);
//                //Singleton<IDIContainerAdapter>.Instance.Initialize(); //TODO
//            }
//            return Singleton<IDIContainerAdapter>.Instance;
//        }

//        public static void Replace(IDIContainerAdapter engine)
//        {
//            //engine.Build();
//            Singleton<IDIContainerAdapter>.Instance = engine;
//        }

//        public static IDIContainerAdapter CreateEngineInstance()
//        {
//            if (KoreConfigurationSection.Instance != null && !string.IsNullOrEmpty(KoreConfigurationSection.Instance.Engine.Type))
//            {
//                var engineType = Type.GetType(KoreConfigurationSection.Instance.Engine.Type);
//                if (engineType == null)
//                {
//                    throw new ConfigurationErrorsException("The type '" + engineType + "' could not be found. Please check the configuration or check for missing assemblies.");
//                }
//                if (!typeof(IDIContainerAdapter).IsAssignableFrom(engineType))
//                    throw new ConfigurationErrorsException("The type '" + engineType + "' doesn't implement 'Kore.Infrastructure.IEngine' and cannot be configured for that purpose.");
//                return Activator.CreateInstance(engineType) as IDIContainerAdapter;
//            }

//            return new AutofacDIContainerAdapter(new ContainerBuilder());
//        }
//    }
//}