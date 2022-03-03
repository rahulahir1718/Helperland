using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels.BookService
{
    public class ServiceScheduleViewModel
    {

        public DateTime ServiceStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime ServiceTime { get; set; }
        public double ServiceHours { get; set; }

        public string? Comments { get; set; }

        [Display(Name = "I have pets at home")]
        public bool HasPets { get; set; }
    }
}
