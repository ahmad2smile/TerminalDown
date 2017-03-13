using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using voteStuff.Entities;
using voteStuff.Models;
using voteStuff.Services;
using voteStuff.ViewModels;

namespace voteStuff.Controllers
{
    public class AccountController: Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VoteDbContext _context;
        private IVotingService _votingService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            VoteDbContext context,
            IVotingService votingService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _votingService = votingService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties,provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty,errorMessage: $"Error: {remoteError}");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var extLoginResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false);
            if (extLoginResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            if (extLoginResult.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                var indentifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                var userName = string.Concat(indentifier, lastName, firstName);

                
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    FbUserId = indentifier,
                    FirstName = firstName,
                    LastName = lastName
                };

                var extUserCreatResult = await _userManager.CreateAsync(user);

                //Gifted Votes at time of Registration
                const int numberOfGiftedVotes = 5;

                if (extUserCreatResult.Succeeded)
                {
                    var extRegisterLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (extRegisterLoginResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        await _context.UserVotingDbs.AddAsync(new UserVotingDb
                        {
                            UserID = user.Id,
                            LastTimeVotesGifted = DateTime.UtcNow,
                            LastTotallVotesGifted = numberOfGiftedVotes,
                            TotallCastedVotes = 0,
                            TotallVotingRights = numberOfGiftedVotes
                        });
                        await _context.SaveChangesAsync();
                        _context.Dispose();

                        return LocalRedirect(returnUrl);
                    }
                }
                return RedirectToAction("Login", "Account");
            }

        }

        [Authorize]
        public async Task<IActionResult> ProfileOverview()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var allDuosVotedByCurrentUser = await _votingService.GetAllDuosVotedByCurrentUser(currentUser.Id);

            var model = new ProfileOverviewViewModel
            {
                FbUserId = currentUser.FbUserId,
                UserName = currentUser.FirstName+ " " + currentUser.LastName,
                AllDuosVotedByUser = allDuosVotedByCurrentUser
            };
            return View(model);
        }
    }
}
