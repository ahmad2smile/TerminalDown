using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using voteStuff.ViewModels;

namespace voteStuff.Controllers
{
    public class HomeController: Controller
    {

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
            return View(id);
        }

        [HttpPost]
        [Authorize]
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
