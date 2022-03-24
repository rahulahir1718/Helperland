using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels.ServiceProvider
{
    public class MyDetailsViewModel
    {

        public bool IsActive { get; set; }
        public string? UserProfilePicture { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        [Display(Name = "Mobile number")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number")]
        public string Mobile { get; set; }

        public string BirthDay { get; set; }
        public string BirthMonth { get; set; }
        public string BirthYear { get; set; }

        [Display(Name = "Nationality")]
        public int? Nationality { get; set; }

        [Display(Name ="Gender")]
        public int? Gender { get; set; }

        public int? AddressId { get; set; }

        [Required]
        [Display(Name = "Street name")]
        public string StreetName { get; set; }

        [Required]
        [Display(Name = "House number")]
        public string HouseNumber { get; set; }

        [Required]
        [Display(Name = "Postal code")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Invalid ZipCode!!")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

    }
}
