using System.Configuration;

namespace Kore.Configuration
{
    public class KoreConfigurationSection : ConfigurationSection
    {
        private static KoreConfigurationSection instance;

        public static KoreConfigurationSection Instance
        {
            get
            {
                return instance ??
                       (instance = (KoreConfigurationSection)ConfigurationManager.GetSection("kore"));
            }
        }

        [ConfigurationProperty("dynamicDiscovery", IsRequired = true, DefaultValue = true)]
        public bool DynamicDiscovery
        {
            get { return (bool)base["dynamicDiscovery"]; }
            set { base["dynamicDiscovery"] = value; }
        }

        [ConfigurationProperty("ignoreStartupTasks", IsRequired = true, DefaultValue = false)]
        public bool IgnoreStartupTasks
        {
            get { return (bool)base["ignoreStartupTasks"]; }
            set { base["ignoreStartupTasks"] = value; }
        }

        [ConfigurationProperty("engine", IsRequired = false)]
        public EngineConfigurationElement Engine
        {
            get { return (EngineConfigurationElement)base["engine"]; }
        }

        [ConfigurationProperty("tasks", IsRequired = false)]
        public TasksConfigurationElement Tasks
        {
            get { return (TasksConfigurationElement)base["tasks"]; }
        }
    }
}