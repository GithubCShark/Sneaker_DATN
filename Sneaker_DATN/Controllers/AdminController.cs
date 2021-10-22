using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sneaker_DATN.Constant;
using Sneaker_DATN.Models;
using Sneaker_DATN.Models.ViewModels;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private IAdminSvc _adminSvc;

        public AdminController(IWebHostEnvironment webHostEnvironment, IAdminSvc adminSvc)
        {
            _webHostEnviroment = webHostEnvironment;
            _adminSvc = adminSvc;
        }

        public IActionResult Index()
        {
            string userName = HttpContext.Session.GetString(SessionKey.User.UserName);
            if (userName != null && userName != "")
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(AdminController.Login), "Admin");
            }
            //return View();
        }

        public IActionResult Login(string returnUrl)
        {
            string userName = HttpContext.Session.GetString(SessionKey.User.UserName);
            if (userName != null && userName != "")
            {
                return RedirectToAction(nameof(UserController.Index), "User");
            }

            ViewLogin login = new ViewLogin();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(ViewLogin viewLogin)
        {
            if (ModelState.IsValid)
            {
                Users user = _adminSvc.Login(viewLogin);
                if (user != null)
                {
                    HttpContext.Session.SetString(SessionKey.User.UserName, user.UserName);
                    HttpContext.Session.SetString(SessionKey.User.FullName, user.FullName);
                    HttpContext.Session.SetInt32(SessionKey.User.ID.ToString(), user.UserID);
                    HttpContext.Session.SetString(SessionKey.User.UserContext, JsonConvert.SerializeObject(user));
                    if (user.ImgUser == null || user.ImgUser == "")
                    {
                        HttpContext.Session.SetString(SessionKey.User.Avatar, "");
                    }
                    else
                    {
                        HttpContext.Session.SetString(SessionKey.User.Avatar, user.ImgUser);
                    }
                    return RedirectToAction(nameof(Index), "Admin");
                }
            }
            return View(viewLogin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            {
                HttpContext.Session.Remove(SessionKey.User.UserName);
                HttpContext.Session.Remove(SessionKey.User.FullName);
                HttpContext.Session.Remove(SessionKey.User.Avatar);
                HttpContext.Session.Remove(SessionKey.User.ID.ToString());
                HttpContext.Session.Remove(SessionKey.User.UserContext);

                return RedirectToAction(nameof(Login), "Admin");
            }
        }
    }
}
