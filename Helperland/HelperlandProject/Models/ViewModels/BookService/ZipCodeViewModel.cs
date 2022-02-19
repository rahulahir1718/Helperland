using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels.BookService
{
    public class ZipCodeViewModel
    {
        [Required]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Invalid ZipCode!!")]
        public string ZipCode { get; set; }
    }
}
