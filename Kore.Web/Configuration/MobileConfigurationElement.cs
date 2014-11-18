using System.Configuration;

namespace Kore.Web.Configuration
{
    public class MobileConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("emulateMobileDevice", IsRequired = false, DefaultValue = false)]
        public bool EmulateMobileDevice
        {
            get { return (bool)base["emulateMobileDevice"]; }
            set { base["emulateMobileDevice"] = value; }
        }

        [ConfigurationProperty("mobileDevicesSupported", IsRequired = false, DefaultValue = true)]
        public bool MobileDevicesSupported
        {
            get { return (bool)base["mobileDevicesSupported"]; }
            set { base["mobileDevicesSupported"] = value; }
        }
    }
}