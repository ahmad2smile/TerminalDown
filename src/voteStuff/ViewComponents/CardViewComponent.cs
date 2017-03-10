using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using voteStuff.Entities;
using voteStuff.Hubs;
using voteStuff.Services;

namespace voteStuff.ViewComponents
{
    public class CardViewComponent: ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVotingService _votingService;
        private readonly IConnectionManager _connectionManager;


        public CardViewComponent(
            IVotingService votingService, 
            UserManager<ApplicationUser> userManager,
            IConnectionManager connectionManager
            )
        {
            _votingService = votingService;
            _userManager = userManager;
            _connectionManager = connectionManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id, string votedName = null)
        {
            ApplicationUser currentLogedInUser = await _userManager.GetUserAsync(HttpContext.User);

            if (!string.IsNullOrEmpty(votedName))
            {
                var model = await _votingService.VoteCast(id, votedName, currentLogedInUser);
//                _connectionManager.GetHubContext<VotingHub>().Clients.All.UpdateVotedDuo(model);
                
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
