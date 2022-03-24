using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels.Admin
{
    public class EditRequestViewModel
    {

        public int ServiceRequestId { get; set; }

        [Required(ErrorMessage ="Date Required")]
        [Display(Name ="Date")]
        [DataType(DataType.Date)]
        public DateTime ServiceStartDate { get; set; }

        [Required(ErrorMessage = "Time Required")]
        [Display(Name = "Time")]
        public string ServiceStartTime    { get; set; }

        [Required]
        [Display(Name = "Street name")]
        public string StreetName { get; set; }

        [Required]
        [Display(Name = "House number")]
        public string HouseNumber { get; set; }

        [Display(Name = "Postal code")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Invalid ZipCode!!")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name ="Why do you want to reschedule service request?")]
        public string? RescheduleReason { get; set; }
    }
}
