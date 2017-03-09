using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using voteStuff.Entities;
using voteStuff.Models;

namespace voteStuff.ViewComponents
{
    public class ProfileViewComponent: ViewComponent
    {
        private VoteDbContext _context;
        private SignInManager<ApplicationUser> _singInManager;
        private UserManager<ApplicationUser> _userManager;

        public ProfileViewComponent(
            SignInManager<ApplicationUser> singInManager, 
            UserManager<ApplicationUser> userManager,
            VoteDbContext context
            )
        {
            _singInManager = singInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_singInManager.IsSignedIn(HttpContext.User))
            {
                var currentLogedInUser = await _userManager.GetUserAsync(HttpContext.User);
                var currentUserVotingData =
                   await _context.UserVotingDbs.FirstOrDefaultAsync(r => r.UserID == currentLogedInUser.Id);

                string firstName = currentLogedInUser.FirstName;
                var identifier = currentLogedInUser.FbUserId;
                var picture = $"https://graph.facebook.com/{identifier}/picture";
                var totallVotingRights = currentUserVotingData.TotallVotingRights;

                var model = new ProfileModel
                {
                    FirstName = firstName,
                    ProfilePicture = picture,
                    ProfileVotingRight = totallVotingRights
                };
                return View("Default", model);
            }

            return View("Default", new ProfileModel
            {
                FirstName = "",
                ProfilePicture = "",
                ProfileVotingRight = 0
            });
        }
    }
}
