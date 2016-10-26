using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Web.Plugins;

namespace Kore.Web.Areas.Admin.Plugins.Models
{
    public class EdmPluginDescriptor
    {
        public Guid Id { get; set; }

        public string Group { get; set; }

        public string FriendlyName { get; set; }

        public string SystemName { get; set; }

        public string Version { get; set; }

        public string Author { get; set; }

        public int DisplayOrder { get; set; }

        public bool Installed { get; set; }

        public IEnumerable<int> LimitedToTenants { get; set; }

        public static implicit operator EdmPluginDescriptor(PluginDescriptor other)
        {
            return new EdmPluginDescriptor
            {
                Id = Guid.NewGuid(), //To Keep OData v4 happy
                Group = other.Group,
                FriendlyName = other.FriendlyName,
                SystemName = other.SystemName,
                Version = other.Version,
                Author = other.Author,
                DisplayOrder = other.DisplayOrder,
                Installed = other.Installed,
                LimitedToTenants = other.LimitedToTenants == null ? Enumerable.Empty<int>() : other.LimitedToTenants
            };
        }
    }
}