using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaxiApp.Models;

namespace TaxiApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserContext db;
       


        public HomeController(ILogger<HomeController> logger, UserContext _db)
        {
            _logger = logger;
            db = _db;
        }


        #region Действия пользователя

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(RequestClient request)
        {
           
            if (ModelState.IsValid)
            {
              
                db.RequestClients.Add(request);
                await db.SaveChangesAsync();
                return RedirectToAction("Done", request);
            }
            else
            return BadRequest("Ошибка в оформлении заявки.");

            
        }

        [HttpGet]
        public async Task<IActionResult> Done(int? id)
        {
            if (id != null)
            {
                RequestClient request = await db.RequestClients.FirstOrDefaultAsync(r => r.Id == id);
                if (request != null)
                    return View(request);
            }
            return NotFound();
        }


        [HttpGet]
        public IActionResult Status()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Status(int? id)
        {

            if (id != null)
            {
                RequestClient request = await db.RequestClients.FirstOrDefaultAsync(r => r.Id == id);
                   
       

                if (request != null)
                return RedirectToAction("Result", request);
            }
            return Content("Заявка не найдена");
        }

        public IActionResult Result(RequestClient request)
        {
            if (request != null)
            {
               var result = db.RequestClients.Include(u => u.User).FirstOrDefault(r => r.Id == request.Id);
                return View(result);
            }
            else
                return BadRequest();
            
        }

        #endregion

        #region Действия диспетчера

        [Authorize(Roles = "Диспетчер")]
        [HttpGet]
        public IActionResult Dispetcher()
        { 
            

            var request = db.RequestClients.Include(u => u.User);
           
            return View(request.ToList());
          

            
        }

        [Authorize(Roles = "Диспетчер")]
        public async Task<IActionResult> Edit(int? id)
        {
                 var users = db.Users.Where(u => u.RoleId == 2);

                var drivers = new SelectList(users, "Id", "Login");
                ViewBag.Drivers = drivers;
            



            if(id != null)
            {
                RequestClient request = await db.RequestClients.FirstOrDefaultAsync(r => r.Id == id );
                if (request != null)
                    return View(request);
            }
            return NotFound();
        }
        [Authorize(Roles = "Диспетчер")]
        [HttpPost]
        public async Task<IActionResult> Edit(RequestClient client)
        {
            db.RequestClients.Update(client);
            await db.SaveChangesAsync();
            return RedirectToAction("Dispetcher");
        }

        [Authorize(Roles = "Диспетчер")]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            var users = db.Users.Where(u => u.RoleId == 2);

            var drivers = new SelectList(users, "Id", "Login");
            ViewBag.Drivers = drivers;




            if (id != null)
            {
                RequestClient request = await db.RequestClients.FirstOrDefaultAsync(r => r.Id == id);
                if (request != null)
                    return View(request);
            }
            return NotFound();
        }

        [Authorize(Roles = "Диспетчер")]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id != null)
            {
                RequestClient request = await db.RequestClients.FirstOrDefaultAsync(r => r.Id == id);
                if (request != null)
                {
                    db.RequestClients.Remove(request);
                   await db.SaveChangesAsync();
                    return RedirectToAction("Dispetcher");
                }
            }
            return NotFound();
        }

        #endregion


        #region Действия водителя
        [Authorize(Roles = "Водитель")]
        [HttpGet]
        public IActionResult Driver(User user)
        {

            var request = db.RequestClients.Include(u => u.User).Where(u => u.UserId == user.Id && u.Status == Models.Status.Edit || u.Status == Models.Status.Start);

            return View(request.ToList());
        }


        [Authorize(Roles = "Водитель")]
        [HttpGet]
        public async Task<IActionResult> DriverEdit(int? id)
        {
            var users = db.Users.Where(u => u.RoleId == 2);

            var drivers = new SelectList(users, "Id", "Login");
            ViewBag.Drivers = drivers;

            if (id != null)
            {
                var request = await db.RequestClients.FirstOrDefaultAsync(r => r.Id == id);

                if (request != null)
                    return View(request);
            }
            return Content("Не найдено");
        }




        [Authorize(Roles = "Водитель")]
        [HttpPost]
        public async Task<IActionResult> DriverEdit(RequestClient request)
        {
            User user = db.Users.FirstOrDefault(r => r.Id == request.UserId);


 
            db.RequestClients.Update(request);
            await db.SaveChangesAsync();
            return RedirectToAction("Driver", user);
        }

        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




    }
}
