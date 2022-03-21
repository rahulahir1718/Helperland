using HelperlandProject.Models.Data;

namespace HelperlandProject.Models.ViewModels.Customer
{
    public class FavouriteProvidersViewModel
    {
        public IEnumerable<User> ServiceProviders { get; set; }
        public List<int> FavouriteSpIds { get; set; }
        public List<int> BlockedSpIds { get; set; }

        public int totalCleanings { get; set; }
    }
}
