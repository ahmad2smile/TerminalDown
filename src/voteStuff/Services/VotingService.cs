using System;
using Microsoft.EntityFrameworkCore;
using voteStuff.Entities;
using voteStuff.Models;
using System.Threading.Tasks;
using voteStuff.ViewModels;

namespace voteStuff.Services
{
    public interface IVotingService
    {
        Task<VoteDuoViewModel> GetDuo(int id, ApplicationUser currentLogedInUser);
        Task<DuoVotedByUserDb> didUserVotedThisDuo(int id, ApplicationUser currentLogedInUser);
        Task<VoteDuoViewModel> VoteCast(int id, string votedDuoName, ApplicationUser currentLogedInUser);
    }

    public class VotingService : IVotingService
    {
        private VoteDbContext _context;

        public VotingService(VoteDbContext context)
        {
            _context = context;
        }

        public async Task<DuoVotedByUserDb> didUserVotedThisDuo(int id, ApplicationUser currentLogedInUser)
        {
            var userVotingData =
                await _context.UserVotingDbs.FirstOrDefaultAsync(r => r.UserID == currentLogedInUser.Id);
            var _didUserVotedThisDuo = new DuoVotedByUserDb();
            if (userVotingData != null)
            {
                _didUserVotedThisDuo = await _context.DuoVotedByUserDbs.FirstOrDefaultAsync(r => r.DuoID == id && r.UserVotingDbID == userVotingData.ID);
            }

            return _didUserVotedThisDuo;
        }


        public async Task<VoteDuoViewModel> GetDuo(int id, ApplicationUser currentLogedInUser)
        {
            var _duoVotedByUser_Data = await didUserVotedThisDuo(id, currentLogedInUser);

            var VoteDuoData = await _context.VotesDb.FirstOrDefaultAsync(r => r.Id == id);

            if (_duoVotedByUser_Data == null)
            {
                var model = new VoteDuoViewModel
                {
                    Id = VoteDuoData.Id,
                    DuoFirst = VoteDuoData.DuoFirst,
                    DuoFirstVotes = VoteDuoData.DuoFirstVotes,
                    DuoSecond = VoteDuoData.DuoSecond,
                    DuoSecondVotes = VoteDuoData.DuoSecondVotes,
                    DuoTotalVotes = VoteDuoData.DuoTotalVotes
                };

                return model;
            }
            else
            {

                var model = new VoteDuoViewModel
                {
                    Id = VoteDuoData.Id,
                    DuoFirst = VoteDuoData.DuoFirst,
                    DuoFirstVotes = VoteDuoData.DuoFirstVotes,
                    DuoSecond = VoteDuoData.DuoSecond,
                    DuoSecondVotes = VoteDuoData.DuoSecondVotes,
                    DuoTotalVotes = VoteDuoData.DuoTotalVotes,
                    duoVotedByUser_Data = _duoVotedByUser_Data
                };

                return model;
            }
        }

        public async Task<VoteDuoViewModel> VoteCast(int id, string votedDuoName, ApplicationUser currentLogedInUser)
        {
            var VoteDuoData = await _context.VotesDb.FirstOrDefaultAsync(r => r.Id == id);

            var userVotingData =
                await _context.UserVotingDbs.FirstOrDefaultAsync(r => r.UserID == currentLogedInUser.Id);

            var _duoVotedByUser_Data = await didUserVotedThisDuo(id, currentLogedInUser);

            var model = new VoteDuoViewModel();

            if (VoteDuoData != null)
            {
                if (_duoVotedByUser_Data == null)
                {
                    userVotingData.TotallCastedVotes += 1;
                    userVotingData.TotallVotingRights -= 1;

                    VoteDuoData.DuoTotalVotes = VoteDuoData.DuoFirstVotes + VoteDuoData.DuoSecondVotes + 1;

                    if (votedDuoName == VoteDuoData.DuoFirst)
                    {
                        VoteDuoData.DuoFirstVotes += 1;
                    }
                    else if (votedDuoName == VoteDuoData.DuoSecond)
                    {
                        VoteDuoData.DuoSecondVotes += 1;
                    }

                    await _context.DuoVotedByUserDbs.AddAsync(new DuoVotedByUserDb
                    {
                        DuoID = id,
                        UserVotingDbID = userVotingData.ID,
                        VotingTime = DateTime.UtcNow
                    });

                    await _context.SaveChangesAsync();

                    model = new VoteDuoViewModel
                    {
                        Id = VoteDuoData.Id,
                        DuoFirst = VoteDuoData.DuoFirst,
                        DuoFirstVotes = VoteDuoData.DuoFirstVotes,
                        DuoSecond = VoteDuoData.DuoSecond,
                        DuoSecondVotes = VoteDuoData.DuoSecondVotes,
                        DuoTotalVotes = VoteDuoData.DuoTotalVotes
                    };

                    return model;
                }
                model = new VoteDuoViewModel
                {
                    Id = VoteDuoData.Id,
                    DuoFirst = VoteDuoData.DuoFirst,
                    DuoFirstVotes = VoteDuoData.DuoFirstVotes,
                    DuoSecond = VoteDuoData.DuoSecond,
                    DuoSecondVotes = VoteDuoData.DuoSecondVotes,
                    DuoTotalVotes = VoteDuoData.DuoTotalVotes,
                    duoVotedByUser_Data = _duoVotedByUser_Data
                };
            }
            return model;
        }
    }
}
