using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;

namespace KoreCMS.Data.Domain
{
    public class UserProfileEntry : ITenantEntity
    {
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string UserId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class UserProfileEntryMap : EntityTypeConfiguration<UserProfileEntry>, IEntityTypeConfiguration
    {
        public UserProfileEntryMap()
        {
            ToTable(Constants.Tables.UserProfiles);
            HasKey(x => x.Id);
            Property(x => x.UserId).IsRequired().HasMaxLength(128).IsUnicode(true);
            Property(x => x.Key).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.Value).IsRequired().IsMaxLength().IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}