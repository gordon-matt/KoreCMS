using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Kore.IO;

namespace Kore.WatchdogService
{
    [XmlRoot("watchdog")]
    public class Settings
    {
        public Settings()
        {
            Services = new List<string>();
        }

        [XmlArray("services")]
        [XmlArrayItem("service")]
        public List<string> Services { get; set; }

        public static Settings Load(string fileName)
        {
            return new FileInfo(fileName).XmlDeserialize<Settings>();
        }

        public void Save(string fileName)
        {
            this.XmlSerialize(fileName);
        }
    }
}