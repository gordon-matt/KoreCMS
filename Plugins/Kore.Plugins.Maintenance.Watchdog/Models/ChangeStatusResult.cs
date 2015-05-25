using System.Runtime.Serialization;

namespace Kore.Plugins.Maintenance.Watchdog.Models
{
    [DataContract]
    public struct ChangeStatusResult
    {
        [DataMember]
        public bool Successful { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}