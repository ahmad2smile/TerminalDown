using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace voteStuff.Models
{
    public class DuoVotedByUserDb
    {
        public int ID { get; set; }
        public int DuoID { get; set; }
        public DateTime VotingTime { get; set; }
        public int UserVotingDbID { get; set; }

        [ForeignKey(nameof(DuoID))]
        public VoteDuo VoteDuo { get; set; }

        [ForeignKey(nameof(UserVotingDbID))]
        public UserVotingDb UserVotingDb { get; set; }
    }
}
