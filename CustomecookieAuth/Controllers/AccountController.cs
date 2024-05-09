using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CustomecookieAuth.Models;

namespace CustomecookieAuth.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult LoginAsync(string ReturnUrl)
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string user, string password, string ReturnUrl)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return NotFound();
            }
            // Validate login credentials here and get user details.
            bool isAuthenticated = user?.ToUpper() == "USER" && password == "Welcome@1";
            if (!isAuthenticated)
            {
                ViewBag.Message = "Invalid Credentials";
                return View();
            }
            
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "Amit"),
                new Claim(ClaimTypes.Role, "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, Constants.AuthenticationScheme);
            await _httpContextAccessor.HttpContext.SignInAsync(Constants.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties());
            if (string.IsNullOrEmpty(ReturnUrl))
                return Ok("Login Successfully");
            else
                return LocalRedirect(ReturnUrl);
        }

        public async Task<IActionResult> SignOut()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return NotFound();
            }
            await _httpContextAccessor.HttpContext.SignOutAsync(Constants.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
