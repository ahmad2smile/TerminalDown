using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using voteStuff.Entities;
using voteStuff.Models;
using voteStuff.Services;

namespace voteStuff.ViewComponents
{
    public class CardViewComponent: ViewComponent
    {
        private IVotingService _votingService;


        public CardViewComponent(IVotingService votingService)
        {
            _votingService = votingService;
        }

        public IViewComponentResult Invoke(int id, string votedName = null)
        {
            if (votedName != null)
            {
                var model = _votingService.VoteCast(id, votedName);
                return View(model);
            }
            return View();
        }
    }
}
