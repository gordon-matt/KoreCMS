using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Models
{
    public class AddressModel
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

        public int CountryId { get; set; }

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