using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using voteStuff.Services;
using voteStuff.ViewModels;

namespace voteStuff.Controllers
{
    public class HomeController: Controller
    {
        private INextDuoService _nextDuoService;

        public HomeController(INextDuoService nextDuoService)
        {
            _nextDuoService = nextDuoService;
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
        public async Task<IActionResult> Duos(int previousDuoId, int category,int nextOrPrev)
        {
            //when selcting category pin on index page with category -1 & id = 0  (just call the 1 Duo of Db)
            int nextDuoId = 1;

            //calling Next or Prev Duo from Duo page with Category -1 & id > 0
            if (previousDuoId > 0 && category == -1) nextDuoId = _nextDuoService.GetNextDuoAll(previousDuoId, nextOrPrev);

            //when selecting category pin on index page with categories 0 1 2 3 & id = 0
            //also the defaul of ~/Home/Duos id=0 & category=0
            if (previousDuoId == 0 && category > -1) nextDuoId = await _nextDuoService.GetDuoOfCategory(category);

            //calling Next or Prev Duo from Duo page with Categories 0 1 2 3 & id > 0
            if (previousDuoId > 0 && category > -1) nextDuoId = await _nextDuoService.GetNextDuoOfCategory(previousDuoId, category, nextOrPrev);

            return View(nextDuoId);
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

        [HttpGet]
        [Authorize]
        public IActionResult ProfileComponentUpdate()
        {
            return ViewComponent("Profile");
        }
    }
}
