using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sneaker_DATN.Models;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private IUserSvc _userSvc;
        private DataContext _context;
        public UserController(IWebHostEnvironment webHostEnvironment, IUserSvc userSvc, DataContext context)
        {
            _webHostEnviroment = webHostEnvironment;
            _userSvc = userSvc;
            _context = context;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View(_userSvc.GetAllUser());
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            var user = _userSvc.GetUser(id);

            var roles = _userSvc.GetRole(user.RoleID);
            ViewData["RoleNameD"] = roles.Role;
            return View(user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            var rolename = _context.Roles.ToList();
            ViewData["RoleNameC"]= new SelectList(rolename, "RoleID", "Role");
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users user)
        {
            try
            {
                _userSvc.AddUser(user);
                return RedirectToAction(nameof(Index), new { id = user.UserID});
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            var role = _context.Roles.ToList();
            ViewData["RoleN"] = new SelectList(role, "RoleID", "Role");

            var user = _userSvc.GetUser(id);
            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userSvc.EditUser(id, user);
                }
                return RedirectToAction(nameof(Index),new { id = user.UserID });
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: UserController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: UserController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
