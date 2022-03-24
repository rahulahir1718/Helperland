using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels.Customer
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name ="Old Password")]
        [DataType(DataType.Password)]
        [StringLength(14, MinimumLength = 6, ErrorMessage = "Maximum 14 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,14}$", ErrorMessage = "Password must be between 6 and 14 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [StringLength(14, MinimumLength = 6, ErrorMessage = "Maximum 14 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,14}$", ErrorMessage = "Password must be between 6 and 14 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }
    }
}
