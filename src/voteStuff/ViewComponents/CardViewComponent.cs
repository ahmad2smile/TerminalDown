using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using voteStuff.Entities;
using voteStuff.Services;

namespace voteStuff.ViewComponents
{
    public class CardViewComponent: ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVotingService _votingService;


        public CardViewComponent(IVotingService votingService, UserManager<ApplicationUser> userManager)
        {
            _votingService = votingService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id, string votedName = null)
        {
            ApplicationUser currentLogedInUser = await _userManager.GetUserAsync(HttpContext.User);

            if (!string.IsNullOrEmpty(votedName))
            {
                var model = await _votingService.VoteCast(id, votedName, currentLogedInUser);
                return View(model);
            }
            else
            {
                var model = await _votingService.GetDuo(id, currentLogedInUser);
                return View(model);
            }
        }
    }
}
