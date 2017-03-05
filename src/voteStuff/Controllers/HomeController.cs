using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using voteStuff.Models;
using voteStuff.Services;
using voteStuff.ViewModels;

namespace voteStuff.Controllers
{
    public class HomeController: Controller
    {
        private IVotingService _votingService;

        public HomeController(IVotingService votingService)
        {
            _votingService = votingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


        [HttpGet]
        [Authorize]
        public IActionResult Duos()
        {
            int id = 1;
            VoteDuo model = _votingService.GetDuo(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Duos(CastedVote postedModel)
        {
            if (ModelState.IsValid)
            {
                return ViewComponent("Card", postedModel);
            }

            return ViewComponent("Card");
        }
    }
}
