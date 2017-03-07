using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using voteStuff.Entities;
using voteStuff.Models;
using voteStuff.Services;

namespace voteStuff.ViewComponents
{
    public class ProfileViewComponent: ViewComponent
    {
        private SignInManager<ApplicationUser> _singInManager;
        private UserManager<ApplicationUser> _userManager;

        public ProfileViewComponent(SignInManager<ApplicationUser> singInManager, UserManager<ApplicationUser> userManager)
        {
            _singInManager = singInManager;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_singInManager.IsSignedIn(HttpContext.User))
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                string firstName = user.FirstName;
                var info = await _singInManager.GetExternalLoginInfoAsync();
                var identifier = user.FbUserId;
                var picture = $"https://graph.facebook.com/{identifier}/picture";
                var model = new ProfileModel {firstName = firstName, picture = picture};
                return View("Default", model);
            }
            return View("Default", new ProfileModel { firstName = "", picture = ""});
        }
    }
}
