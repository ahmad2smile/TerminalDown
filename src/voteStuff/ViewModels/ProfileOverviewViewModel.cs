using System.Collections.Generic;
using voteStuff.Models;

namespace voteStuff.ViewModels
{
    public class ProfileOverviewViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<VoteDuo> AllDuosVotedByUser { get; set; }
    }
}
