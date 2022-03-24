using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels.Customer
{
    public class RescheduleServiceViewModel
    {
        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; }

        [Required(ErrorMessage = "Time Required")]
        [Display(Name = "Time")]
        public string ServiceTime { get; set; }

        public int ServiceRequestId { get; set; }
    }
}
