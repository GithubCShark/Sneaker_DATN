﻿using Microsoft.AspNetCore.Hosting;
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
using System.Text;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private IAdminSvc _adminSvc;
        protected ISendMailService _sendGmail;

        public AdminController(IWebHostEnvironment webHostEnvironment, IAdminSvc adminSvc, ISendMailService sendGmail)
        {
            _webHostEnviroment = webHostEnvironment;
            _adminSvc = adminSvc;
            _sendGmail = sendGmail;
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

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
            {
                return builder.ToString().ToLower();
            }
            return builder.ToString();
        }

        [NonAction]
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public IActionResult ForgotPassword(string email)
        {
            if (email != null)
            {
                var user = _adminSvc.GetAllUser().Where(u => u.Email == email).FirstOrDefault();
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomString(4, true));
                builder.Append(RandomNumber(1000, 9999));
                builder.Append(RandomString(2, false));

                _adminSvc.ChangePass(user.UserID, builder.ToString());
                MailContent content = new MailContent()
                {
                    To = email,
                    Subject = "Quên mật khẩu",
                    Body = "<p><strong>Mật khẩu sau khi reset: </strong></p>" + builder.ToString()
                };

                _sendGmail.SendMail(content);
            }
            return View();
        }
    }
}
