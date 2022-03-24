using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels.Customer
{
    public class MyDetailsViewModel
    {
        [Required]
        [Display(Name ="First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mobile number")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number")]
        public string Mobile { get; set; }

        public string BirthDay { get; set; }
        public string BirthMonth { get; set; }
        public string BirthYear { get; set; }

        [Display(Name ="My Preffered Language")]
        public int? LanguageId { get; set; }

    }
}
