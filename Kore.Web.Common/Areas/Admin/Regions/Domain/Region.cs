using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.Common.Areas.Admin.Regions.Domain
{
    public class Region : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Region Type. 0 = Other, 1 = Continent, 2 = Country, 3 = State, 4 = City
        /// </summary>
        public RegionType RegionType { get; set; }

        public string CountryCode { get; set; }

        public bool HasStates { get; set; }

        public string StateCode { get; set; }

        public int? ParentId { get; set; }

        public short? Order { get; set; }

        public virtual Region Parent { get; set; }

        public virtual ICollection<Region> Children { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class RegionMap : EntityTypeConfiguration<Region>, IEntityTypeConfiguration
    {
        public RegionMap()
        {
            ToTable(Constants.Tables.Regions);
            HasKey(m => m.Id);
            Property(m => m.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(m => m.RegionType).IsRequired();
            Property(m => m.CountryCode).HasMaxLength(2).IsUnicode(false).IsFixedLength();
            Property(m => m.HasStates).IsRequired();
            Property(m => m.StateCode).HasMaxLength(10).IsUnicode(false);
            HasOptional(p => p.Parent).WithMany(p => p.Children).HasForeignKey(x => x.ParentId).WillCascadeOnDelete(false);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}