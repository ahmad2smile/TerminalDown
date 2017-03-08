using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using voteStuff.Models;

namespace voteStuff.ViewModels
{
    public class VoteDuoViewModel
    {
        public int Id { get; set; }
        public string DuoFirst { get; set; }
        public string DuoSecond { get; set; }
        public int DuoFirstVotes { get; set; }
        public int DuoSecondVotes { get; set; }
        public int DuoTotalVotes { get; set; }

        public DuoVotedByUserDb duoVotedByUser_Data { get; set; }
    }
}
