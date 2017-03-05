using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using voteStuff.Entities;
using voteStuff.Models;

namespace voteStuff.Controllers
{
    public class AccountController: Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
                    if (returnUrl == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return LocalRedirect(returnUrl);
                }
            }
            return  View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
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
                var userName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var indentifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                
                var user = new ApplicationUser { UserName = userName, Email = email, FbUserId = indentifier};
                var extUserCreatResult = await _userManager.CreateAsync(user);

                if (extUserCreatResult.Succeeded)
                {
                    var extRegisterLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (extRegisterLoginResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                return RedirectToAction("Login", "Account");
            }

        }
    }
}
