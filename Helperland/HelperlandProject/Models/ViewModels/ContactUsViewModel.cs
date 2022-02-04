using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels
{
    public class ContactUsViewModel
    {
        [Required(ErrorMessage ="Please enter a first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number")]
        public string Mobile { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage ="Please enter your message")]
        public string Message { get; set; }

        public IFormFile? File { get; set; }

    }
}
