using AutoMapper;
using BookReviews.Impl;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using User = BookReviews.Impl.Models.User;

namespace BookReviews.Web.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : BaseController
    {
        IAuthenticationManager _authenticationManager;
        IMapper _mapper;
        IUserLogic _userLogic;

        public AccountController(IAuthenticationManager authenticationManager, IMapper mapper, IUserLogic userLogic)
        {
            _authenticationManager = authenticationManager;
            _mapper = mapper;
            _userLogic = userLogic;
        }

        [HttpGet]
        [Route("register")]
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterVM();
            return View(model);
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterVM model)
        {
            if (!model.Password.Equals(model.ConfirmPassword, StringComparison.OrdinalIgnoreCase))
                ModelState.AddModelError("ConfirmPassword", "Passwords must match");

            if (!ModelState.IsValid)
                return View(model);

            User user = _mapper.Map<User>(model);

            bool result = await _userLogic.RegisterAccount(user, model.Password);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to register account";
                return View(model);
            }

            TempData["SuccessMessage"] = "Successfully registered account";

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            var model = new LoginVM();
            TempData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = await _userLogic.AuthenticateUser(model.EmailAddress, model.Password);

            if (user == null)
            {
                TempData["ErrorMessage"] = "The email address or password is incorrect";
                return View(model);
            }

            LoginUser(user);

            string returnUrl = TempData["ReturnUrl"].ToString();
            if (!String.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                Response.Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("logout")]
        public ActionResult Logout()
        {
            LogoutUser();
            TempData["SuccessMessage"] = "You are now logged out";

            return View("Login");
        }

        private void LoginUser(User user)
        {
            // clear any existing cookie for the user
            LogoutUser();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.EmailAddress),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var authSettings = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Globals.AppSettings.AuthTicketExpiration)
            };

            _authenticationManager.SignIn(authSettings, identity);
        }

        private void LogoutUser()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}