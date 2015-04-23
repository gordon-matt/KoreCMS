using System.Web.Script.Serialization;
using Kore.Data;
using Newtonsoft.Json;

namespace Kore.Security.Membership
{
    public class KoreUser : IEntity
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool IsLockedOut { get; set; }

        #region IEntity Members

        [JsonIgnore]
        [ScriptIgnore]
        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public override string ToString()
        {
            return UserName;
        }
    }
}