using System.Collections.Generic;
using voteStuff.Models;

namespace voteStuff.ViewModels
{
    public class ProfileOverviewViewModel
    {
        public string FbUserId { get; set; }
        public string UserName { get; set; }
        public DuoCategoriesEnum Category { get; set; }
        public ICollection<VoteDuoViewModel> AllDuosVotedByUser { get; set; }
    }
}
