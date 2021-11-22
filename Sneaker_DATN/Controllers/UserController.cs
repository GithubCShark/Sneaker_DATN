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
using X.PagedList;

namespace Sneaker_DATN.Controllers
{
    public class UserController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private IUserSvc _userSvc;
        private DataContext _context;
        private IUploadHelper _uploadHelper;
        public UserController(IWebHostEnvironment webHostEnvironment, IUserSvc userSvc, DataContext context, IUploadHelper uploadHelper)
        {
            _webHostEnviroment = webHostEnvironment;
            _userSvc = userSvc;
            _context = context;
            _uploadHelper = uploadHelper;
        }
        // GET: UserController
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var students = from s in _userSvc.GetAllUser() 
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.UserName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FullName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.UserName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.DOB);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.DOB);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.UserName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            ViewData["role"] = _context.Roles.ToList();

            return View(students.ToPagedList(pageNumber, pageSize));

        
            //return View(_userSvc.GetAllUser());
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            var user = _userSvc.GetUser(id);

            var roles = _userSvc.GetRole(user.RoleID);
            ViewData["RoleNameD"] = roles.Role;
            return PartialView(user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            var rolename = _context.Roles.ToList();
            ViewData["RoleNameC"]= new SelectList(rolename, "RoleID", "Role");
            return PartialView();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (user.ImageUser != null && user.ImageUser.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnviroment.WebRootPath, "images");
                        _uploadHelper.UploadImage(user.ImageUser, rootPath, "avatar");
                        user.ImgUser = user.ImageUser.FileName;
                    }
                    _userSvc.AddUser(user);
                    return RedirectToAction(nameof(Index), new { id = user.UserID });
                }
                else
                {
                    var rolename = _context.Roles.ToList();
                    ViewData["RoleNameC"] = new SelectList(rolename, "RoleID", "Role");
                    return View();
                }
            }
            catch
            {
                return PartialView();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            var role = _context.Roles.ToList();
            ViewData["RoleN"] = new SelectList(role, "RoleID", "Role");

            var user = _userSvc.GetUser(id);
            return PartialView(user);
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
                    user.Password = null;
                    _userSvc.EditUser(id, user);
                    return RedirectToAction(nameof(Index), new { id = user.UserID });
                }
                else
                {
                    var role = _context.Roles.ToList();
                    ViewData["RoleN"] = new SelectList(role, "RoleID", "Role");

                    return PartialView(user);
                }
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Info(int id)
        {
            var user = _userSvc.GetInfo(id);

            var roles = _userSvc.GetRole(user.RoleID);
            ViewData["RoleNameInfo"] = roles.Role;

            return View(user);
        }

        public ActionResult BotChat()
        {
            return View();
        }
    }
}
