using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.Configuration.Domain
{
    public class GenericAttribute : IEntity
    {
        public int Id { get; set; }

        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public string Property { get; set; }

        public string Value { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class GenericAttributeMap : EntityTypeConfiguration<GenericAttribute>, IEntityTypeConfiguration
    {
        public GenericAttributeMap()
        {
            ToTable("Kore_GenericAttributes");
            HasKey(m => m.Id);
            Property(x => x.EntityType).IsRequired().HasMaxLength(512).IsUnicode(false);
            Property(x => x.EntityId).IsRequired().HasMaxLength(50).IsUnicode(false);
            Property(m => m.Property).IsRequired().HasMaxLength(128).IsUnicode(false);
            Property(x => x.Value).IsMaxLength().IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}