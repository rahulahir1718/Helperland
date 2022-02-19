using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
