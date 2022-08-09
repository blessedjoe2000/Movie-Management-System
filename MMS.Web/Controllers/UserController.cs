using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MMS.Data.Services;
using MMS.Data.Models;
using MMS.Web.Models;

namespace MMS.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IMovieService _svc;

        public UserController()
        {
            _svc = new MovieServiceDb();
        }

        //GET /Login
        public IActionResult Login()
        {
            return View();
        }

        //POST /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] UserLoginViewModel m)
        {
            // call service to Authenticate User
            var user = _svc.Authenticate(m.Email, m.Password);
            if(user == null)
            {
                ModelState.AddModelError("Email", "Invalid login details");
                ModelState.AddModelError("Password", "Invalid login details");
                return View(m);
            }
            // sign user in using cookie authentication to store principal
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                BuildClaimsPrincipal(user)
            );
            return RedirectToAction("Index", "Movie");
        }

        //GET /Register
        public IActionResult Register()
        {
            return View();
        }

        //POST /Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Username,Email,Password,PasswordConfirm,Role")] UserRegisterViewModel m)
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            var user = _svc.Register(m.Username, m.Email, m.Password, m.Role);

            // check if emailaddress is unique
            if (user == null)
            {
                ModelState.AddModelError("Email", "email has already been used. Use another");
                return View(m);
            }

            // registration successful now redirect to login page
            Alert("Registration Successful - Now Login", AlertType.info);
            return RedirectToAction("Login");
        }

        //POST /Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
            (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult ErrorNotAuthorised()
        {
            Alert("Not Authorized", AlertType.warning);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorNotAuthenticated()
        {
            Alert("Not Authenticated", AlertType.warning);
            return RedirectToAction("Login, User");
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail(string email)
        {

            if (_svc.GetUserByEmail(email) != null)
            {
                return Json($"Email Address {email} is already in use. Please choose another");
            }
            return Json(true);
        }

        //******************build claim principle****************

        private ClaimsPrincipal BuildClaimsPrincipal(User user)
        {
            var claims = new ClaimsIdentity(new[]{
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            //build principal using claims
            return new ClaimsPrincipal(claims);
        }
    }
}