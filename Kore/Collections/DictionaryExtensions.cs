using System.Collections.Generic;
using System.Collections.Specialized;

namespace Kore.Collections
{
    public static class DictionaryExtensions
    {
        public static NameValueCollection ToNameValueCollection(this Dictionary<string, string> dictionary)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var value in dictionary)
            {
                nameValueCollection.Add(value.Key, value.Value);
            }

            return nameValueCollection;
        }

        public static NameValueCollection ToNameValueCollection<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var value in dictionary)
            {
                nameValueCollection.Add(value.Key.ToString(), value.Value.ToString());
            }

            return nameValueCollection;
        }
    }
}