using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KoreCMS.Models
{
    public class InstallationModel
    {
        [Required]
        [EmailAddress]
        [DisplayName("Admin Email")]
        public string AdminEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Admin Password")]
        public string AdminPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("AdminPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Enter Connection String")]
        public bool EnterConnectionString { get; set; }

        [DisplayName("Connection String")]
        public string ConnectionString { get; set; }

        [DisplayName("Database Server")]
        public string DatabaseServer { get; set; }

        [DisplayName("Database Name")]
        public string DatabaseName { get; set; }

        [DisplayName("Use Windows Authentication")]
        public bool UseWindowsAuthentication { get; set; }

        [DisplayName("Database Username")]
        public string DatabaseUsername { get; set; }

        [DisplayName("Database Password")]
        public string DatabasePassword { get; set; }
    }
}