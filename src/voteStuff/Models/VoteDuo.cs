namespace voteStuff.Models
{

    public class VoteDuo
    {
        public int Id { get; set; }
        public string DuoFirst { get; set; }
        public string DuoSecond { get; set; }
        public int DuoFirstVotes { get; set; }
        public int DuoSecondVotes { get; set; }
        public int DuoTotalVotes { get; set; }

        public DuoCategoriesEnum Category { get; set; }

    }
}
