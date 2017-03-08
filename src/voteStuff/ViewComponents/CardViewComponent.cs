using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using voteStuff.Entities;
using voteStuff.Models;
using voteStuff.Services;

namespace voteStuff.ViewComponents
{
    public class CardViewComponent: ViewComponent
    {
        private UserManager<ApplicationUser> _userManager;
        private IVotingService _votingService;


        public CardViewComponent(IVotingService votingService, UserManager<ApplicationUser> userManager)
        {
            _votingService = votingService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id, string votedName = null)
        {
            ApplicationUser currentLogedInUser = await _userManager.GetUserAsync(HttpContext.User);

            if (votedName != null)
            {
                var model = await _votingService.VoteCast(id, votedName, currentLogedInUser);
                return View(model);
            }
            return View();
        }
    }
}
