using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using voteStuff.Entities;
using voteStuff.Models;
using System.Threading.Tasks;

namespace voteStuff.Services
{
    public interface IVotingService
    {
        VoteDuo GetDuo(int id);
        Task<VoteDuo> VoteCast(int id, string votedDuoName, ApplicationUser currentLogedInUser);
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

        public async Task<VoteDuo> VoteCast(int id, string votedDuoName, ApplicationUser currentLogedInUser)
        {
            VoteDuo castedDuo = await _context.VotesDb.FirstOrDefaultAsync(r => r.Id == id);
            UserVotingDb userVotingData =
                await _context.UserVotingDbs.FirstOrDefaultAsync(r => r.UserID == currentLogedInUser.Id);

            if (castedDuo != null)
            {
                userVotingData.TotallCastedVotes += 1;
                userVotingData.TotallVotingRights -= 1;

                castedDuo.DuoTotalVotes = castedDuo.DuoFirstVotes + castedDuo.DuoSecondVotes + 1;

                if (votedDuoName == castedDuo.DuoFirst)
                {
                    castedDuo.DuoFirstVotes += 1;
                }
                else if (votedDuoName == castedDuo.DuoSecond)
                {
                    castedDuo.DuoSecondVotes += 1;
                }
                await _context.DuoVotedByUserDbs.AddAsync(new DuoVotedByUserDb
                {
                    DuoID = id,
                    UserVotingDbID = userVotingData.ID,
                    VotingTime = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }

            return castedDuo;
        }
    }
}
