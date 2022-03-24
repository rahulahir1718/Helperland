using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels.Customer
{
    public class EditAddressViewModel
    {
        public int? AddressId { get; set; }

        [Required]
        [Display(Name ="Street name")]
        public string StreetName { get; set; }

        [Required]
        [Display(Name ="House number")]
        public string HouseNumber { get; set; }

        [Display(Name ="Postal code")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Invalid ZipCode!!")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name ="City")]
        public string City { get; set; }

        [Required]
        [Display(Name ="Phone number")]
        public string PhoneNumber { get; set; }
    }
}
