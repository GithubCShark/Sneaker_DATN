using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sneaker_DATN.Helpers;
using Sneaker_DATN.Models;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    public class UserMemController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private IUserMemSvc _userMemSvc;
        private DataContext _context;
        private IUploadHelper _uploadHelper;
        public UserMemController(IWebHostEnvironment webHostEnvironment, IUserMemSvc userMemSvc, DataContext context, IUploadHelper uploadHelper)
        {
            _webHostEnviroment = webHostEnvironment;
            _userMemSvc = userMemSvc;
            _context = context;
            _uploadHelper = uploadHelper;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View(_userMemSvc.GetAllUserMem());
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            var user = _userMemSvc.GetUserMem(id);

            var roles = _userMemSvc.GetRole(user.RoleID);
            ViewData["RoleNameD"] = roles.Role;
            return View(user);
        }

        // GET: UserController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: UserController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Users user)
        //{
        //    try
        //    {
                //    if (user.ImageUser != null && user.ImageUser.Length > 0)
                //{
                //    string rootPath = Path.Combine(_webHostEnviroment.WebRootPath, "images");
                //    _uploadHelper.UploadImage(user.ImageUser, rootPath, "avatar");
                //    user.ImgUser = user.ImageUser.FileName;
                //}
        //        _userMemSvc.AddUserMem(user);
        //        return RedirectToAction(nameof(Index), new { id = user.UserID});
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            var role = _context.Roles.ToList();
            ViewData["RoleN"] = new SelectList(role, "RoleID", "Role");

            var user = _userMemSvc.GetUserMem(id);
            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Users user)
        {
            string thumuccon = "avatar";
            try
            {
                if (ModelState.IsValid)
                {
                    if (user.ImageUser != null && user.ImageUser.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnviroment.WebRootPath, "images");
                        _uploadHelper.UploadImage(user.ImageUser, rootPath, thumuccon);
                        user.ImgUser = user.ImageUser.FileName;
                    }
                    _userMemSvc.EditUserMem(id, user);
                    return RedirectToAction(nameof(Index), new { id = user.UserID });
                }
                else
                {
                    var role = _context.Roles.ToList();
                    ViewData["RoleN"] = new SelectList(role, "RoleID", "Role");

                    return View(user);
                }
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
