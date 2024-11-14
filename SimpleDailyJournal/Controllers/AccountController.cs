using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SimpleDailyJournal.Controllers

{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            Console.WriteLine("ActionCtrl -> Login, Reached here!!!");
            var authProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("AuthCallback", "Account", null, Request.Scheme)
            };
            
            Console.WriteLine($"Login Redirect URI: {authProperties.RedirectUri}");

            // Challenge the Auth0 authentication scheme and have it call "AuthCallback" action if all good
            return Challenge(authProperties, "Auth0");
        }
        
        public IActionResult AuthCallback()
        {
            // After processing the Auth0 response, redirect to the main journal entries page.
            return RedirectToAction("Index", "JournalEntries"); 
        }

        public IActionResult Register()
        {
            // Add screen_hint=signup to show the sign-up screen by default
            var authProperties = new AuthenticationProperties
            {
                RedirectUri = "/",
                Items = { { "screen_hint", "signup" } }
            };

            return Challenge(authProperties, "Auth0");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to a page that informs the user they have logged out and suggests they log in again.
            return RedirectToAction("LoggedOutInfo", "Account");
        }

        public IActionResult LoggedOutInfo()
        {
            ViewBag.Message = "You have been logged out. Please log in to access your journal entries.";

            return View();
        }
    }
}