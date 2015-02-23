using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace KoreCMS.Data.Domain
{
    public class UserProfileEntry : IEntity
    {
        public int Id { get; set; }

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
            ToTable("Kore_UserProfiles");
            HasKey(x => x.Id);
            Property(x => x.UserId).IsRequired();
            Property(x => x.Key).HasMaxLength(255).IsRequired();
        }
    }
}