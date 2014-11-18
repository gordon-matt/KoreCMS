using System.Web.Script.Serialization;
using Kore.Data;
using Newtonsoft.Json;

namespace Kore.Security.Membership
{
    public class KoreRole : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        #region IEntity Members

        [JsonIgnore]
        [ScriptIgnore]
        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}