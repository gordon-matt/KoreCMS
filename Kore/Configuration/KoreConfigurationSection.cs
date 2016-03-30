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

        [ConfigurationProperty("scheduledTasks", IsRequired = false)]
        public ScheduledTasksConfigurationElement ScheduledTasks
        {
            get { return (ScheduledTasksConfigurationElement)base["scheduledTasks"]; }
        }
    }
}