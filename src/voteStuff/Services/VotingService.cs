using System.Linq;
using voteStuff.Entities;
using voteStuff.Models;

namespace voteStuff.Services
{
    public interface IVotingService
    {
        VoteDuo GetDuo(int id);
        VoteDuo VoteCast(int id, string votedDuoName);
    }

    public class VotingService: IVotingService
    {
        private VoteDbContext _context;

        public VotingService(VoteDbContext context)
        {
            _context = context;
        }

        public VoteDuo GetDuo(int id)
        {
            return _context.VotesDb.FirstOrDefault(r => r.Id == id);
        }

        public VoteDuo VoteCast(int id, string votedDuoName)
        {
            VoteDuo castedDuo = _context.VotesDb.FirstOrDefault(r => r.Id == id);
            if (castedDuo != null)
            {
                castedDuo.DuoTotalVotes = castedDuo.DuoFirstVotes + castedDuo.DuoSecondVotes + 1;

                if (votedDuoName == castedDuo.DuoFirst)
                {
                    castedDuo.DuoFirstVotes += 1;
                }
                else if (votedDuoName == castedDuo.DuoSecond)
                {
                    castedDuo.DuoSecondVotes += 1;
                }

                _context.SaveChanges();
            }

            return castedDuo;
        }
    }
}
