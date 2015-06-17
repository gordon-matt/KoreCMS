using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Common.Areas.Admin.Regions.Domain;
using Kore.Web.Plugins;

namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public class Address : IEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string FamilyName { get; set; }

        public string GivenNames { get; set; }

        public string Email { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public int CountryId { get; set; }

        public int CityId { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public virtual Region Country { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class AddressMap : EntityTypeConfiguration<Address>, IEntityTypeConfiguration
    {
        public AddressMap()
        {
            ToTable(Constants.Tables.Addresses);
            HasKey(x => x.Id);
            Property(x => x.FamilyName).IsRequired().HasMaxLength(128);
            Property(x => x.GivenNames).IsRequired().HasMaxLength(128);
            Property(x => x.Email).IsRequired().HasMaxLength(255);
            Property(x => x.AddressLine1).IsRequired().HasMaxLength(128);
            Property(x => x.AddressLine2).HasMaxLength(128);
            Property(x => x.AddressLine3).HasMaxLength(128);
            Property(x => x.CountryId).IsRequired();
            Property(x => x.CityId).IsRequired();
            Property(x => x.PostalCode).IsRequired().HasMaxLength(10);
            Property(x => x.PhoneNumber).IsRequired().HasMaxLength(25);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}