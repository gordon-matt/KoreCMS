using System;
using Kore.Configuration.Domain;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Areas.Admin.Configuration.Models
{
    public class SettingModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public dynamic Value { get; set; }

        public static implicit operator SettingModel(Setting other)
        {
            return new SettingModel
            {
                Id = other.Id,
                Name = other.Name,
                Type = other.Type,
                Value = JObject.Parse(other.Value)
            };
        }
    }
}