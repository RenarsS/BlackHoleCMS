using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlackHoleCMS.Data;
using BlackHoleCMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlackHoleCMS.Controllers
{
    public class UserController : Controller
    {
        private readonly BlackHoleCmsContext _context;

        public UserController(BlackHoleCmsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username, Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var u = await _context.User.SingleAsync(model => model.Username == user.Username);

                if (u.Password == user.Password)
                {
                    if (u.Username != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim("user", u.Username),
                            new Claim("role", "User")
                        };
                        await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsPrincipal(new ClaimsIdentity(claims, "UserAuth", "user", "role"))));
                    }

                    return RedirectToAction(nameof(Profile), u);
                }
                    
                
                return View();
            }
            
            return View(user.Username);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

    }
}