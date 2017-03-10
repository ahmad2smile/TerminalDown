using System;
using System.Linq;
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
        Task<bool> didUserVotedThisDuo(int id, ApplicationUser currentLogedInUser);
        Task<VoteDuoViewModel> VoteCast(int id, string votedDuoName, ApplicationUser currentLogedInUser);
    }

    public class VotingService : IVotingService
    {
        private readonly VoteDbContext _context;

        public VotingService(VoteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> didUserVotedThisDuo(int id, ApplicationUser currentLogedInUser)
        {
            var userVotingData =
                await _context.UserVotingDbs.FirstOrDefaultAsync(r => r.UserID == currentLogedInUser.Id);
            bool _didUserVotedThisDuo = false;
            if (userVotingData != null)
                _didUserVotedThisDuo = _context.DuoVotedByUserDbs.Any(r => r.DuoID == id && r.UserVotingDbID == userVotingData.ID);

            return _didUserVotedThisDuo;
        }


        public async Task<VoteDuoViewModel> GetDuo(int id, ApplicationUser currentLogedInUser)
        {
            var VoteDuoData = await _context.VotesDb.FirstOrDefaultAsync(r => r.Id == id);
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

        public async Task<VoteDuoViewModel> VoteCast(int id, string votedDuoName, ApplicationUser currentLogedInUser)
        {
            var VoteDuoData = await _context.VotesDb.FirstOrDefaultAsync(r => r.Id == id);

            var userVotingData =
                await _context.UserVotingDbs.FirstOrDefaultAsync(r => r.UserID == currentLogedInUser.Id);

            var _duoVotedByUser_Data = await didUserVotedThisDuo(id, currentLogedInUser);

            var model = new VoteDuoViewModel();

            if (VoteDuoData != null)
            {
                if (!_duoVotedByUser_Data)
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
                    DuoTotalVotes = VoteDuoData.DuoTotalVotes
                };
            }
            return model;
        }
    }
}
