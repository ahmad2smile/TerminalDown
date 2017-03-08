using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using voteStuff.Models;
using voteStuff.Services;
using voteStuff.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using voteStuff.Entities;

namespace voteStuff.Controllers
{
    public class HomeController: Controller
    {
        private IVotingService _votingService;
        private UserManager<ApplicationUser> _userManager;

        public HomeController(IVotingService votingService, UserManager<ApplicationUser> userManager)
        {
            _votingService = votingService;
            _userManager = userManager;
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
        public async Task<IActionResult> Duos()
        {
            var currentLogedInUser = await _userManager.GetUserAsync(HttpContext.User);
            int id = 1;
            VoteDuoViewModel model = await _votingService.GetDuo(id, currentLogedInUser);
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
