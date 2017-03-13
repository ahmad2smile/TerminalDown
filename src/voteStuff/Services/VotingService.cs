using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        Task<ICollection<VoteDuoViewModel>> GetAllDuosVotedByCurrentUser(string currentLogedInUser);
        Task<bool> didUserVotedThisDuo(int id, ApplicationUser currentLogedInUser);
        Task<VoteDuoViewModel> VoteCast(int id, string votedDuoName, ApplicationUser currentLogedInUser);
    }

    public class VotingService : IVotingService
    {
        private readonly VoteDbContext _context;
        private DuoVotedByUserDb _duoVotedByCurrentUser;
        private readonly INextDuoService _nextDuoService;

        public VotingService(VoteDbContext context, INextDuoService nextDuoService)
        {
            _context = context;
            _nextDuoService = nextDuoService;
        }

        public async Task<bool> didUserVotedThisDuo(int id, ApplicationUser currentLogedInUser)
        {
            var userVotingData =
                await _context.UserVotingDbs.Where(r => r.UserID == currentLogedInUser.Id).FirstOrDefaultAsync();

            if (userVotingData == null) return false;
            _duoVotedByCurrentUser =
                    await _context.DuoVotedByUserDbs.FirstOrDefaultAsync(
                        r => r.DuoID == id && r.UserVotingDbID == userVotingData.ID);
            if (_duoVotedByCurrentUser != null)
                return true;
            return false;
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
                if (!duoIsAlreadyVotedByUser && userVotingData.TotallVotingRights != 0)
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


                if (userVotingData.TotallVotingRights == 0)
                {
                    duoIsAlreadyVotedByUser = false;
                    _duoVotedByCurrentUser = new DuoVotedByUserDb();
                }

                model = new VoteDuoViewModel
                {
                    Id = VoteDuoData.Id,
                    DuoFirst = VoteDuoData.DuoFirst,
                    DuoFirstVotes = VoteDuoData.DuoFirstVotes,
                    DuoSecond = VoteDuoData.DuoSecond,
                    DuoSecondVotes = VoteDuoData.DuoSecondVotes,
                    DuoTotalVotes = VoteDuoData.DuoTotalVotes,
                    DuoIsAlreadyVotedByUser = duoIsAlreadyVotedByUser,
                    DuoVotedByCurrentUserDb = _duoVotedByCurrentUser
                };
            }
            return model;
        }

        public async Task<ICollection<VoteDuoViewModel>> GetAllDuosVotedByCurrentUser(string id)
        {
            var currentUserVotingDb = await _context.UserVotingDbs.FirstOrDefaultAsync(r => r.UserID == id);
            if (currentUserVotingDb == null) return new Collection<VoteDuoViewModel>();

            //            this is how it should've been !! current implementation is a massive load on Db
            //            ICollection<DuoVotedByUserDb> dumyCollection = currentUserVotingDb.DuoVotedByUserDbList;

            int currentUserVotingDbId = currentUserVotingDb.ID;
            ICollection<DuoVotedByUserDb> duoVotedByCurrentUserDb = await
                _context.DuoVotedByUserDbs.Where(r => r.UserVotingDbID == currentUserVotingDbId).ToListAsync();
            IOrderedEnumerable<DuoVotedByUserDb> orderedList = duoVotedByCurrentUserDb.OrderByDescending(r => r.VotingTime);

            ICollection<VoteDuoViewModel> duosVotedByCurrentUserCollection = new Collection<VoteDuoViewModel>();

            foreach (var duoVotedByCurrentUser in orderedList)
            {
                int votedDuoId = duoVotedByCurrentUser.DuoID;
                var currentDuo = await _context.VotesDb.FirstOrDefaultAsync(r => r.Id == votedDuoId);
                duosVotedByCurrentUserCollection.Add(new VoteDuoViewModel
                {
                    DuoFirst = currentDuo.DuoFirst,
                    DuoFirstVotes = currentDuo.DuoFirstVotes,
                    DuoSecond = currentDuo.DuoSecond,
                    DuoSecondVotes = currentDuo.DuoSecondVotes,
                    DuoTotalVotes = currentDuo.DuoTotalVotes,
                    Category = currentDuo.Category,

                    DuoVotedByCurrentUserDb = duoVotedByCurrentUser
                });
            }

            return duosVotedByCurrentUserCollection;
        }
    }
}
