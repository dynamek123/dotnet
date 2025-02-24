using Microsoft.AspNetCore.Mvc;
using ProjektSklepGryWideo.Data;
using ProjektSklepGryWideo.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;

namespace ProjektSklepGryWideo.Controllers
{
    public class AccountController : Controller
    {
        private readonly GameStoreContext _context;

        public AccountController(GameStoreContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if (user != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
                if (_context.Users.Any(ur => ur.Email == user.Email && ur.IsAdmin == true))
                {
                    // Add the Admin role claim
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }


        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("FirstName, LastName, Email, Password, IsAdmin")] User user, string AdminKey)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists in the database
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                    return View(user); 
                }

                if (AdminKey == "2025")
                {
                    user.IsAdmin = true; 
                }

              
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login"); 
            }

            return View(user); 
        }

        [Authorize]
        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
        
            var user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }

             [Authorize(Roles = "Admin")] 
            public IActionResult Admin()
            {
                return View(); 
            }

    }
}
