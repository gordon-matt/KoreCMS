using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Tenants.Domain;

namespace Kore.Tenants
{
    public static class TenantExtensions
    {
        public static IEnumerable<string> ParseHostValues(this Tenant store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            var parsedValues = new List<string>();
            if (!string.IsNullOrEmpty(store.Hosts))
            {
                string[] hosts = store.Hosts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string host in hosts)
                {
                    var tmp = host.Trim();
                    if (!string.IsNullOrEmpty(tmp))
                    {
                        parsedValues.Add(tmp);
                    }
                }
            }
            return parsedValues;
        }

        public static bool ContainsHostValue(this Tenant store, string host)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }
            if (string.IsNullOrEmpty(host))
            {
                return false;
            }

            return store.ParseHostValues().Any(x => x.Equals(host, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}