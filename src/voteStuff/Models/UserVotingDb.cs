using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using voteStuff.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace voteStuff.Models
{
    public class UserVotingDb
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int TotallVotingRights { get; set; }
        public int TotallCastedVotes { get; set; }
        public DateTime LastTimeVotesGifted { get; set; }
        public int LastTotallVotesGifted { get; set; }

        [ForeignKey(nameof(UserID))]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<DuoVotedByUserDb> DuoVotedByUserDbList { get; set; }
    }
}
