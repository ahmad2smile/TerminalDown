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
        public DuoVotedByUserDb _duoVotedByCurrentUser;
        private INextDuoService _nextDuoService;

        public VotingService(VoteDbContext context, INextDuoService nextDuoService)
        {
            _context = context;
            _nextDuoService = nextDuoService;
        }

        public async Task<bool> didUserVotedThisDuo(int id, ApplicationUser currentLogedInUser)
        {
            bool _didUserVotedThisDuo = false;

            var userVotingData =
                await _context.UserVotingDbs.Where(r => r.UserID == currentLogedInUser.Id).FirstOrDefaultAsync();

            if (userVotingData != null)
            {
                _duoVotedByCurrentUser =
                    await _context.DuoVotedByUserDbs.FirstOrDefaultAsync(
                        r => r.DuoID == id && r.UserVotingDbID == userVotingData.ID);
                if (_duoVotedByCurrentUser != null)
                {
                    _didUserVotedThisDuo = true;
                }
            }

            return _didUserVotedThisDuo;
        }


        public async Task<VoteDuoViewModel> GetDuo(int id, ApplicationUser currentLogedInUser)
        {
            var VoteDuoData = await _context.VotesDb.FirstOrDefaultAsync(r => r.Id == id) ??
                              await _context.VotesDb.FirstOrDefaultAsync(r => r.Id == _nextDuoService.DefaultDuoId);
            bool duoIsAlreadyVotedByUser = await didUserVotedThisDuo(VoteDuoData.Id, currentLogedInUser);

            var model = new VoteDuoViewModel
            {
                Id = VoteDuoData.Id,
                DuoFirst = VoteDuoData.DuoFirst,
                DuoFirstVotes = VoteDuoData.DuoFirstVotes,
                DuoSecond = VoteDuoData.DuoSecond,
                DuoSecondVotes = VoteDuoData.DuoSecondVotes,
                DuoTotalVotes = VoteDuoData.DuoTotalVotes,
                DuoIsAlreadyVotedByUser = duoIsAlreadyVotedByUser
            };

            if (duoIsAlreadyVotedByUser)
            {
                model = new VoteDuoViewModel
                {
                    Id = VoteDuoData.Id,
                    DuoFirst = VoteDuoData.DuoFirst,
                    DuoFirstVotes = VoteDuoData.DuoFirstVotes,
                    DuoSecond = VoteDuoData.DuoSecond,
                    DuoSecondVotes = VoteDuoData.DuoSecondVotes,
                    DuoTotalVotes = VoteDuoData.DuoTotalVotes,
                    DuoIsAlreadyVotedByUser = true,
                    DuoVotedByCurrentUserDb = _duoVotedByCurrentUser
                };
                return model;
            }

            return model;
        }

        public async Task<VoteDuoViewModel> VoteCast(int id, string votedDuoName, ApplicationUser currentLogedInUser)
        {
            var VoteDuoData = await _context.VotesDb.FirstOrDefaultAsync(r => r.Id == id);

            var userVotingData =
                await _context.UserVotingDbs.FirstOrDefaultAsync(r => r.UserID == currentLogedInUser.Id);
            bool duoIsAlreadyVotedByUser = await didUserVotedThisDuo(id, currentLogedInUser);
            var model = new VoteDuoViewModel();

            if (VoteDuoData != null)
            {
                if (!duoIsAlreadyVotedByUser)
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

                    var duoVotedByCurrentUserData = new DuoVotedByUserDb
                    {
                        DuoID = id,
                        UserVotingDbID = userVotingData.ID,
                        VotingTime = DateTime.UtcNow
                    };

                    await _context.DuoVotedByUserDbs.AddAsync(duoVotedByCurrentUserData);

                    await _context.SaveChangesAsync();

                    model = new VoteDuoViewModel
                    {
                        Id = VoteDuoData.Id,
                        DuoFirst = VoteDuoData.DuoFirst,
                        DuoFirstVotes = VoteDuoData.DuoFirstVotes,
                        DuoSecond = VoteDuoData.DuoSecond,
                        DuoSecondVotes = VoteDuoData.DuoSecondVotes,
                        DuoTotalVotes = VoteDuoData.DuoTotalVotes,
                        DuoIsAlreadyVotedByUser = true,
                        DuoVotedByCurrentUserDb = duoVotedByCurrentUserData
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
                    DuoIsAlreadyVotedByUser = true,
                    DuoVotedByCurrentUserDb = _duoVotedByCurrentUser
                };
            }
            return model;
        }
    }
}
