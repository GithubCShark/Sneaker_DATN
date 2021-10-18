using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sneaker_DATN.Constant;
using Sneaker_DATN.Helpers;
using Sneaker_DATN.Models;
using Sneaker_DATN.Models.ViewModels;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private IUserMemSvc _userMemSvc;
        private IProductSvc _productSvc;
        private IUploadHelper _uploadHelper;
        public HomeController(IUserMemSvc userMemSvc, IProductSvc productSvc, IWebHostEnvironment webHostEnvironment, IUploadHelper uploadHelper)
        {
            _userMemSvc = userMemSvc;
            _productSvc = productSvc;
            _webHostEnviroment = webHostEnvironment;
            _uploadHelper = uploadHelper;
        }
        public IActionResult Index()
        {
            return View(_productSvc.GetProductAll());
        }

        public IActionResult Login(string returnUrl)
        {
            string userName = HttpContext.Session.GetString(SessionKey.Guest.Guest_UserName);
            if (userName != null && userName != "")
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            ViewWebLogin login = new ViewWebLogin();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(ViewWebLogin viewWebLogin)
        {
            if (ModelState.IsValid)
            {
                Users user = _userMemSvc.Login(viewWebLogin);
                if (user != null)
                {
                    HttpContext.Session.SetString(SessionKey.Guest.Guest_UserName, user.UserName);
                    HttpContext.Session.SetString(SessionKey.Guest.Guest_FullName, user.FullName);
                    HttpContext.Session.SetString(SessionKey.Guest.GuestContext, JsonConvert.SerializeObject(user));

                    return RedirectToAction(nameof(Index), "Home");
                }
            }
            return View(viewWebLogin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            {
                HttpContext.Session.Remove(SessionKey.Guest.Guest_UserName);
                HttpContext.Session.Remove(SessionKey.Guest.Guest_FullName);
                HttpContext.Session.Remove(SessionKey.Guest.GuestContext);

                return RedirectToAction(nameof(Index), "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Users user)
        {
            try
            {
                if (user.ImageUser != null && user.ImageUser.Length > 0)
                {
                    string rootPath = Path.Combine(_webHostEnviroment.WebRootPath, "images");
                    _uploadHelper.UploadImage(user.ImageUser, rootPath, "avatar");
                    user.ImgUser = user.ImageUser.FileName;
                }
                _userMemSvc.AddUserMem(user);
                return RedirectToAction(nameof(Index), "Home");
            }
            catch
            {
                return View();
            }
        }
    }
}
