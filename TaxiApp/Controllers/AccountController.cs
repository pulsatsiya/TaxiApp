using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Controllers
{
    public class AccountController : Controller
    {
        private UserContext db;


        public AccountController(UserContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);

                // добавляем пользователя
                if (user == null)
                {
                    user = new User() { Login = model.Login, Password = model.Password };
                    Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == model.Role.Name);
                    if (userRole != null)
                    {
                        user.Role = userRole;
                    }
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    await Authenticate(user);
                    if(user.RoleId == 1)
                    return RedirectToAction("Dispetcher", "Home");
                    else
                    return RedirectToAction("Driver", "Home", user);

                }
            }
            else ModelState.AddModelError("", "Некорректные данные");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    if (user.RoleId == 1)
                        return RedirectToAction("Dispetcher", "Home");
                    else
                        
                        return RedirectToAction("Driver", "Home", user);
                }
            }
            else ModelState.AddModelError("", "Некорректные данные");
                return View(model);
        }


        // Метод аутентификации
        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}
