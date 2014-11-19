using System.Collections.Generic;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public abstract class JsonObject
    {
        public IDictionary<string, object> ToJson()
        {
            var dictionary = new Dictionary<string, object>();
            Serialize(dictionary);
            return dictionary;
        }

        protected abstract void Serialize(IDictionary<string, object> json);
    }
}