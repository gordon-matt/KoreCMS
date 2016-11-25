using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Demos.ConsoleApp.Data.Domain
{
    public class Person : IEntity
    {
        public int Id { get; set; }

        public string FamilyName { get; set; }

        public string GivenNames { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class PersonMap : EntityTypeConfiguration<Person>, IEntityTypeConfiguration
    {
        public PersonMap()
        {
            ToTable("People");
            HasKey(x => x.Id);
            Property(x => x.FamilyName).IsRequired().HasMaxLength(128).IsUnicode(true);
            Property(x => x.GivenNames).IsRequired().HasMaxLength(128).IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}