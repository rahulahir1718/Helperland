using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels.Customer
{
    public class RescheduleServiceViewModel
    {
        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime ServiceTime { get; set; }

        public int ServiceRequestId { get; set; }

        public List<SelectListItem> timeList = new List<SelectListItem>{
                             new SelectListItem{Text="8:00", Value="8:00:00"},
                             new SelectListItem{Text="8:30", Value="8:30:00"},
                             new SelectListItem{Text="9:00", Value="9:00:00"},
                             new SelectListItem{Text="9:30", Value="9:30:00"},
                             new SelectListItem{Text="10:00",Value="10:00:00"},
                             new SelectListItem{Text="10:30",Value="10:30:00"},
                             new SelectListItem{Text="11:00",Value="11:00:00"},
                             new SelectListItem{Text="11:30",Value="11:30:00"},
                             new SelectListItem{Text="12:00",Value="12:00:00"},
                             new SelectListItem{Text="12:30",Value="12:30:00"},
                             new SelectListItem{Text="13:00",Value="13:00:00"},
                             new SelectListItem{Text="13:30",Value="13:30:00"},
                             new SelectListItem{Text="14:00",Value="14:00:00"},
                             new SelectListItem{Text="14:30",Value="14:30:00"},
                             new SelectListItem{Text="15:00",Value="15:00:00"},
                             new SelectListItem{Text="15:30",Value="15:30:00"},
                             new SelectListItem{Text="16:00",Value="16:00:00"},
                             new SelectListItem{Text="16:30",Value="16:30:00"},
                             new SelectListItem{Text="17:00",Value="17:00:00"},
                             new SelectListItem{Text="17:30",Value="17:30:00"},
                             new SelectListItem{Text="18:00",Value="18:00:00"},
                             new SelectListItem{Text="18:30",Value="18:30:00"}};
    }
}
