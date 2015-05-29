using System.ComponentModel.DataAnnotations;
using Kore.ComponentModel;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Models
{
    public class AddressModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AddressModel.FamilyName)]
        //[Required]
        //[StringLength(128)]
        public string FamilyName { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AddressModel.GivenNames)]
        //[Required]
        //[StringLength(128)]
        public string GivenNames { get; set; }

        //[EmailAddress]
        [LocalizedDisplayName(LocalizableStrings.AddressModel.Email)]
        //[Required]
        //[StringLength(255)]
        public string Email { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AddressModel.AddressLine1)]
        //[Required]
        //[StringLength(128)]
        public string AddressLine1 { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AddressModel.AddressLine2)]
        //[StringLength(128)]
        public string AddressLine2 { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AddressModel.AddressLine3)]
        //[StringLength(128)]
        public string AddressLine3 { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AddressModel.City)]
        //[Required]
        //[StringLength(128)]
        public string City { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AddressModel.PostalCode)]
        //[Required]
        //[StringLength(10)]
        public string PostalCode { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AddressModel.Country)]
        //[Required]
        public int CountryId { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AddressModel.PhoneNumber)]
        //[Phone]
        //[Required]
        //[StringLength(25)]
        public string PhoneNumber { get; set; }

        public static implicit operator AddressModel(Address other)
        {
            return new AddressModel
            {
                Id = other.Id,
                UserId = other.UserId,
                FamilyName = other.FamilyName,
                GivenNames = other.GivenNames,
                Email = other.Email,
                AddressLine1 = other.AddressLine1,
                AddressLine2 = other.AddressLine2,
                AddressLine3 = other.AddressLine3,
                City = other.City,
                PostalCode = other.PostalCode,
                CountryId = other.CountryId,
                PhoneNumber = other.PhoneNumber
            };
        }
    }
}