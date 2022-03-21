using HelperlandProject.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace HelperlandProject.Models.ViewModels.BookService
{
    public class YourDetailsViewModel
    {
        public IEnumerable<UserAddress> userAddresses { get; set; }=new List<UserAddress>();

        public IEnumerable<User>? favouriteSP { get; set; }

        public int? selectedFSPId { get; set; }
        
        [Required(ErrorMessage ="Please provide service address with given postalcode")]
        public int? check { get; set; }
    }
}
