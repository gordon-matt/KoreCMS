using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Kore.Configuration;

namespace Kore.Infrastructure
{
    public class EngineContext
    {
        #region Initialization Methods

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Debug.WriteLine("Constructing engine " + DateTime.Now);
                Singleton<IEngine>.Instance = CreateEngineInstance();
                Debug.WriteLine("Initializing engine " + DateTime.Now);
                Singleton<IEngine>.Instance.Initialize();
            }
            return Singleton<IEngine>.Instance;
        }

        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        public static IEngine CreateEngineInstance()
        {
            if (KoreConfigurationSection.Instance != null && !string.IsNullOrEmpty(KoreConfigurationSection.Instance.Engine.Type))
            {
                var engineType = Type.GetType(KoreConfigurationSection.Instance.Engine.Type);
                if (engineType == null)
                {
                    throw new ConfigurationErrorsException("The type '" + engineType + "' could not be found. Please check the configuration or check for missing assemblies.");
                }
                if (!typeof(IEngine).IsAssignableFrom(engineType))
                    throw new ConfigurationErrorsException("The type '" + engineType + "' doesn't implement 'Kore.Infrastructure.IEngine' and cannot be configured for that purpose.");
                return Activator.CreateInstance(engineType) as IEngine;
            }

            return Default;
        }

        #endregion Initialization Methods

        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }

        public static IEngine Default { get; set; }
    }
}