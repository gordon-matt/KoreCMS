using System;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Models
{
    public class ZoneModel
    {
        [RoboHidden]
        public Guid Id { get; set; }

        [RoboText(IsRequired = true, MaxLength = 255)]
        public string Name { get; set; }

        public static implicit operator ZoneModel(Zone zone)
        {
            return new ZoneModel
            {
                Id = zone.Id,
                Name = zone.Name
            };
        }
    }
}