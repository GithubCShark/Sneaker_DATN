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
using System.Text;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IAdminSvc _adminSvc;
        protected ISendMailService _sendGmail;
        private readonly DataContext _context;
        private readonly IUserMemSvc _userMemSvc;

        public AdminController(IWebHostEnvironment webHostEnvironment, IAdminSvc adminSvc, ISendMailService sendGmail, 
            DataContext dataContext, IUserMemSvc userMemSvc)
        {
            _webHostEnviroment = webHostEnvironment;
            _adminSvc = adminSvc;
            _sendGmail = sendGmail;
            _context = dataContext;
            _userMemSvc = userMemSvc;
        }

        public IActionResult Index()
        {
            string userName = HttpContext.Session.GetString(SessionKey.User.UserName);
            if (userName != null && userName != "")
            {
                //Tổng hợp nhanh
                var today = DateTime.Now;
                var lsorderday = from x in _context.Orders
                               where x.DateCreate.Date == today.Date
                               select x;
                var lsorderm = from x in _context.Orders.Where(x => x.Status == "Đang xử lý")
                               where x.DateCreate.Year == today.Year && x.DateCreate.Month == today.Month
                               select x;
                var lsordernb = from x in _context.Users.Where(x => x.RoleID == 3)
                                where x.DateCreated.Value.Year == today.Year && x.DateCreated.Value.Month == today.Month
                                select x;
                var lsorderdob = from x in _context.Users.Where(x => x.RoleID == 3)
                                where x.DOB.Value.Month == today.Month && x.DOB.Value.Day == today.Day
                                select x;

                ViewData["LsOrderDay"] = lsorderday.Count();
                ViewData["LsOrderM"] = lsorderm.Count();
                ViewData["LsOrderNb"] = lsordernb.Count();
                ViewData["LsOrderDOB"] = lsorderdob.Count();

                //Danh sách
                List<ViewDashBoard> lsdb = new List<ViewDashBoard>();
                List<ViewDashBoard> lsDashb = new List<ViewDashBoard>();

                var lsBrands = from z in _context.Brands
                               select z;
                var lsOrderDetails = from y in _context.OrderDetails
                                     select y;

                var lsorderd = from y in lsOrderDetails
                               select y.ProductID;
                var lsprod = _context.Products.Where(x => lsorderd.Contains(x.ProductID)).ToList();
                var lsOrders = _context.Orders.OrderByDescending(x => x.DateCreate).Where(x => x.Status == "Đang xử lý").Take(5).ToList();

                List<ViewDashBoard> temp = new List<ViewDashBoard>();
                for (int i = 0; i < lsprod.Count; i++)
                {
                    var tempbrand = "";
                    var lsquantity = from x in lsOrderDetails
                                     where x.ProductID == lsprod[i].ProductID
                                     select x.Quantity;
                    foreach (var item in lsBrands)
                    {
                        if (item.BrandID == lsprod[i].BrandID)
                        {
                            tempbrand = item.BrandName;
                        }
                    }
                    temp.Add(new ViewDashBoard
                    {
                        BrandName = tempbrand,
                        Quantity = lsquantity.Sum()
                    });
                }
                temp = temp.OrderByDescending(x => x.Quantity).ToList();
                var grtemp = temp.GroupBy(p => p.BrandName).ToList();
                for (int i = 0; i < grtemp.Count; i++)
                {
                    int grquantity = 0;
                    foreach (var item in temp)
                    {
                        if (item.BrandName == grtemp[i].Key)
                        {
                            grquantity += item.Quantity;
                        }
                    }
                    lsDashb.Add(new ViewDashBoard
                    {
                        Orders = lsOrders[i],
                        BrandName = grtemp[i].Key,
                        Quantity = grquantity
                    });
                }

                lsdb = lsDashb.ToList();
                return View(lsdb);
            }
            else
            {
                return RedirectToAction(nameof(AdminController.Login), "Admin");
            }
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
                else
                {
                    ModelState.AddModelError("loginError", "Tài khoản hoặc mật khẩu sai.");
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

        public IActionResult GiftDoB()
        {
            var today = DateTime.Now;
            var lsuserdob = from x in _context.Users.Where(x => x.RoleID == 3)
                             where x.DOB.Value.Month == today.Month && x.DOB.Value.Day == today.Day
                             select x;
            ViewBag.lsvoucher = _context.Discounts.Where(s => s.Status == false)
                .Take(lsuserdob.Count()).ToList();
            return View(lsuserdob);
        }

        public IActionResult SendVoucher()
        {
            var today = DateTime.Now;
            var lsuserdob = from x in _context.Users.Where(x => x.RoleID == 3)
                            where x.DOB.Value.Month == today.Month && x.DOB.Value.Day == today.Day
                            select x;
            var lsuser = lsuserdob.ToList();
            var lsvoucher = _context.Discounts.Where(s => s.Status == false)
                .Take(lsuserdob.Count()).ToList();
            ViewBag.lsvoucher = lsvoucher;
            for (int i = 0; i < lsuser.Count; i++)
            {
                MailContent content = new MailContent()
                {
                    To = lsuser[i].Email,
                    Subject = "Chúc mừng sinh nhật bạn",
                    Body = "<p><strong>D-ACH Shop gửi tặng bạn mã khuyến mãi nhân ngày sinh nhật của bạn: </strong></p>" + lsvoucher[i].VoucherCode
                };
                _sendGmail.SendMail(content);
            }
            return RedirectToAction(nameof(Index));
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
