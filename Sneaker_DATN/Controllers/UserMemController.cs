using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public ActionResult Index(string sortOrder, bool status, string currentFilterSearch, string searchString, int? page, bool currentFilterStatus)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateCreateSortParm = String.IsNullOrEmpty(sortOrder) ? "datecreate_desc" : "";
            ViewBag.GenderSortParm = sortOrder == "Gender" ? "gender_desc" : "Gender";
            ViewBag.PhoneSortParm = sortOrder == "phone" ? "phone_desc" : "phone";
            ViewBag.DobSortParm = sortOrder == "dob" ? "dob_desc" : "dob";
            ViewBag.LockSortParm = sortOrder == "lock" ? "lock_desc" : "lock";


            ViewBag.Status = (from r in _userMemSvc.GetAllUserMem()
                              select r.Lock).Distinct();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilterSearch;
            }

            if (status != false)
            {
                page = 1;
            }
            else
            {
                status = currentFilterStatus;
            }

            ViewBag.CurrentFilterSearch = searchString;
            ViewBag.CurrentFilterStatus = status;

            var mem = from s in _userMemSvc.GetAllUserMem()
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                mem = mem.Where(s => s.FullName.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!String.IsNullOrEmpty(status.ToString()))
            {
                mem = mem.Where(s => s.Lock == status);
            }
            switch (sortOrder)
            {
                case "datecreate_desc":
                    mem = mem.OrderBy(s => s.DateCreated);
                    break;

                case "Gender":
                    mem = mem.OrderBy(s => s.Gender);
                    break;
                case "gender_desc":
                    mem = mem.OrderByDescending(s => s.Gender);
                    break;

                case "phone":
                    mem = mem.OrderBy(s => s.PhoneNumber);
                    break;
                case "phone_desc":
                    mem = mem.OrderByDescending(s => s.PhoneNumber);
                    break;

                case "dob":
                    mem = mem.OrderBy(s => s.DOB);
                    break;
                case "dob_desc":
                    mem = mem.OrderByDescending(s => s.DOB);
                    break;

                case "lock":
                    mem = mem.OrderBy(s => s.Lock);
                    break;
                case "lock_desc":
                    mem = mem.OrderByDescending(s => s.Lock);
                    break;

                default:  // Name ascending 
                    mem = mem.OrderByDescending(s => s.DateCreated);
                    break;
            }

            if (page == null) page = 1;
                    var user = _context.Users.Include(b => b.UserName).OrderBy(b => b.UserID);
                    int pageSize = 6;
                    int pageNumber = (page ?? 1);

                    return View(mem.ToPagedList(pageNumber, pageSize));
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            var user = _userMemSvc.GetUserMem(id);

            var roles = _userMemSvc.GetRole(user.RoleID);
            ViewData["RoleNameD"] = roles.Role;
            return PartialView(user);
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            var role = _context.Roles.ToList();
            ViewData["RoleN"] = new SelectList(role, "RoleID", "Role");

            var user = _userMemSvc.GetUserMem(id);
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
                    _userMemSvc.EditUserMem(id, user);
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
    }
}
