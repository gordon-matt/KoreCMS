﻿using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

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

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string PhoneNumber { get; set; }

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
            Property(x => x.City).IsRequired().HasMaxLength(128);
            Property(x => x.PostalCode).IsRequired().HasMaxLength(10);
            Property(x => x.Country).IsRequired().HasMaxLength(50);
            Property(x => x.PhoneNumber).IsRequired().HasMaxLength(25);
        }
    }
}