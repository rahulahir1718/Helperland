using HelperlandProject.Models.Data;

namespace HelperlandProject.Models.ViewModels.ServiceProvider
{
    public class BlockCustomerViewModel
    {
        public IEnumerable<User> allCustomers { get; set; }
        public IEnumerable<User> blockedCustomers { get; set; }
    }
}
