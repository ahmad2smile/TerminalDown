using voteStuff.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using voteStuff.Models;

namespace voteStuff.Services
{
    public interface INextDuoService
    {
        int GetNextDuoAll(int previousDuoId,int nextOrPrev);
        Task<int> GetDuoOfCategory(int Category);
        Task<int> GetNextDuoOfCategory(int previousDuoId, int Category, int nextOrPrev);

        bool ChangedCategory { get; set; }
        int DefaultDuoId { get; }
    }

    public class NextDuoService : INextDuoService
    {
        private readonly VoteDbContext _context;
        public int DefaultDuoId { get; } = 1;
        public bool ChangedCategory { get; set; }

        public NextDuoService(VoteDbContext context)
        {
            _context = context;
        }


        public int GetNextDuoAll(int previousDuoId, int nextOrPrev)
        {
            return (previousDuoId + nextOrPrev) > 0 ? (previousDuoId + nextOrPrev) : previousDuoId;
        }

        public async Task<int> GetDuoOfCategory(int Category)
        {
            var nextCategoryDuo = await _context.VotesDb.FirstOrDefaultAsync(r => (int)r.Category == Category);
            if (nextCategoryDuo == null) return DefaultDuoId;
                
            return nextCategoryDuo.Id;
        }

        public async Task<int> GetNextDuoOfCategory(int previousDuoId, int Category, int nextOrPrev)
        {
            var nextCategoryDuo = new VoteDuo();
            if (nextOrPrev == 1)
            {
                nextCategoryDuo = await _context.VotesDb.FirstOrDefaultAsync(
                    r => (int)r.Category == Category && r.Id > previousDuoId
                );
            }
            else if (nextOrPrev == -1)
            {
                nextCategoryDuo = await _context.VotesDb.FirstOrDefaultAsync(
                    r => (int)r.Category == Category && r.Id < previousDuoId
                );
            }

            if (nextCategoryDuo == null) ChangedCategory = true;

            return nextCategoryDuo?.Id ?? DefaultDuoId;
        }
    }
}
