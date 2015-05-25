using System.ComponentModel.DataAnnotations;
using Kore.Web.Common.Areas.Admin.Regions.Domain;

namespace Kore.Web.Common.Areas.Admin.Regions.Models
{
    public class RegionModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Region Type. 0 = Other, 1 = Continent, 2 = Country, 3 = State, 4 = City
        /// </summary>
        public RegionType RegionType { get; set; }

        [StringLength(10)]
        public string CountryCode { get; set; }

        public bool? HasStates { get; set; }

        [StringLength(10)]
        public string StateCode { get; set; }

        public int? ParentId { get; set; }

        public static implicit operator RegionModel(Region entity)
        {
            return new RegionModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CountryCode = entity.CountryCode,
                HasStates = entity.HasStates,
                ParentId = entity.ParentId,
                RegionType = entity.RegionType,
                StateCode = entity.StateCode
            };
        }
    }
}