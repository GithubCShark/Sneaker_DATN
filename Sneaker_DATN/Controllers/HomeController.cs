using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
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
using X.PagedList;
using static Sneaker_DATN.Filters.AuthenticationFilterAttribute;

namespace Sneaker_DATN.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private IUserMemSvc _userMemSvc;
        private IProductSvc _productSvc;
        private IUploadHelper _uploadHelper;
        private IOrderSvc _orderSvc;
        private IOrderDetailSvc _orderDetailSvc;
        private IDiscountSvc _discountSvc;
        private readonly DataContext _context;
        protected IEncodeHelper _encodeHelper;
        public HomeController(IUserMemSvc userMemSvc, IProductSvc productSvc, IWebHostEnvironment webHostEnvironment, 
            IUploadHelper uploadHelper, IOrderSvc orderSvc, IOrderDetailSvc orderDetailSvc, DataContext context, 
            IEncodeHelper encodeHelper, IDiscountSvc discountSvc)
        {
            _userMemSvc = userMemSvc;
            _productSvc = productSvc;
            _webHostEnviroment = webHostEnvironment;
            _uploadHelper = uploadHelper;
            _orderSvc = orderSvc;
            _orderDetailSvc = orderDetailSvc;
            _context = context;
            _encodeHelper = encodeHelper;
            _discountSvc = discountSvc;
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
                    HttpContext.Session.SetInt32(SessionKey.Guest.Guest_ID.ToString(), user.UserID);
                    HttpContext.Session.SetString(SessionKey.Guest.GuestContext, JsonConvert.SerializeObject(user));
                    if (user.ImgUser == null || user.ImgUser == "")
                    {
                        HttpContext.Session.SetString(SessionKey.Guest.Guest_Avatar, "");
                    }
                    else
                    {
                        HttpContext.Session.SetString(SessionKey.Guest.Guest_Avatar, user.ImgUser);
                    }
                    return RedirectToAction(nameof(Index), "Home", ViewData["checklog"] = true);
                }
            }
            return PartialView(viewWebLogin);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            {
                HttpContext.Session.Remove(SessionKey.Guest.Guest_UserName);
                HttpContext.Session.Remove(SessionKey.Guest.Guest_FullName);
                HttpContext.Session.Remove(SessionKey.Guest.Guest_ID.ToString());
                HttpContext.Session.Remove(SessionKey.Guest.Guest_Avatar);
                HttpContext.Session.Remove(SessionKey.Guest.GuestContext);

                return RedirectToAction(nameof(Index), "Home");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.RoleID = 3;
                    user.Lock = false;
                    _userMemSvc.AddUserMem(user);

                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    return View(user);
                }
            }
            catch
            {
                return View(user);
            }
        }

        public IActionResult Products(int? page)
        {
            if (page == null) page = 1;
            var sizes = _context.Products.Include(b => b.ProductName)
                .OrderBy(b => b.ProductID);
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            ViewData["brand"] = _context.Brands.ToList();
            ViewData["size"] = _context.Sizes.ToList();
            ViewData["color"] = _context.Colors.ToList();
            return View(_context.Products.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var products = _context.Products.Include(b => b.ProductName)
                .OrderBy(b => b.ProductID);
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            return View(_context.Products.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult AddCart(int id, int size, int color)
        {
            var cart = HttpContext.Session.GetString("cart");
            
            if (cart == null)
            {
                var product = _productSvc.GetProduct(id);
                List<ViewCart> listCart = new List<ViewCart>()
                {
                    new ViewCart
                    {
                        Products = product,
                        Quantity = 1,
                        Size = size,
                        Color =  color
                    }
                };
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));
            }
            else
            {
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                bool check = true;
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Products.ProductID == id)
                    {
                        dataCart[i].Quantity++;
                        check = false;
                    }
                }
                if (check)
                {
                    var product = _productSvc.GetProduct(id);
                    dataCart.Add(new ViewCart
                    {
                        Products = product,
                        Quantity = 1,
                        Size = size,
                        Color = color
                    });
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
            }
            return Ok();
        }

        public IActionResult Cart()
        {
            List<ViewCart> dataCart = new List<ViewCart>();
            var cart = HttpContext.Session.GetString("cart");
            ViewBag.sz = _context.Sizes.ToList();
            ViewBag.col = _context.Colors.ToList();
            ViewBag.Prosz = _context.ProductSizes.ToList();
            ViewBag.Procol = _context.ProductColors.ToList();
            if (cart != null)
            {
                dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
            }
            return View(dataCart);
        }

        public IActionResult UpdateCart(int id, int soluong, int size, int color)
        {
            var cart = HttpContext.Session.GetString("cart");
            double total = 0;
            if (cart != null)
            {
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Products.ProductID == id)
                    {
                        dataCart[i].Quantity = soluong;
                        dataCart[i].Size = size;
                        dataCart[i].Color = color;
                        break;
                    }
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                total = Tongtien();
                return Ok(total);
            }
            return BadRequest();
        }

        public IActionResult DeleteCart(int id)
        {
            double total = 0;
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);

                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Products.ProductID == id)
                    {
                        dataCart.RemoveAt(i);
                    }
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                total = Tongtien();
                return Ok(total);
            }
            return BadRequest();
        }

        public IActionResult OrderCart(string voucherCode)
        {
            var itemDiscount = _discountSvc.GetDiscount(voucherCode);

            string guest_Name = HttpContext.Session.GetString(SessionKey.Guest.Guest_FullName);
            if (guest_Name == null || guest_Name == "")  // đã có session
            {
                return BadRequest();
            }
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null && cart.Count() > 0)
            {
                var GuestContext = HttpContext.Session.GetString(SessionKey.Guest.GuestContext);
                var GuestID = JsonConvert.DeserializeObject<Users>(GuestContext).UserID;
                var user = _context.Users.Find(GuestID);
                double total = Tongtien();

                var order = new Orders()
                {
                    UserID = GuestID,
                    DateCreate = DateTime.Now,
                    Total = total,
                    PaymentAmount = total,
                    Address = user.Address,
                    Status = "Đang xử lý",
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                if (itemDiscount != null && !itemDiscount.Status) // chua su dung
                {
                    itemDiscount.Status = true; //  su dung
                    itemDiscount.DateUse = DateTime.Now; //  su dung
                    itemDiscount.CustomerId = order.UserID;

                    _discountSvc.EditDiscount(itemDiscount.VoucherId, itemDiscount);

                    order.VoucherCode = itemDiscount.VoucherCode;
                    order.ExpDiscount = true;
                    order.VoucherId = itemDiscount.VoucherId;
                    if (itemDiscount.Classify)
                    {
                        if (itemDiscount.Price >= order.Total)
                        {
                            order.PaymentAmount = 0;
                        }
                        else
                        {
                            order.PaymentAmount = order.Total - itemDiscount.Price;
                        }
                    }
                    else
                    {
                        order.PaymentAmount = order.Total - (order.Total * itemDiscount.Percent) / 100;
                    }
                }

                _context.Orders.Add(order);
                _context.SaveChanges();

                int orderID = order.OrderID;
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                for (int i = 0; i < dataCart.Count; i++)
                {
                    OrderDetails details = new OrderDetails()
                    {
                        OrderID = orderID,
                        ProductID = dataCart[i].Products.ProductID,
                        Quantity = dataCart[i].Quantity,
                        Price = dataCart[i].Products.Price,
                        ColorID = dataCart[i].Color,
                        SizeID = dataCart[i].Size
                    };
                    _context.OrderDetails.Add(details);
                    _context.SaveChanges();
                }
                HttpContext.Session.Remove("cart");
                return Ok();

            }
            return RedirectToAction(nameof(OrderComplete), "Home");
        }

        public IActionResult CheckVoucher(string voucherCode)
        {
            var item = _discountSvc.GetDiscount(voucherCode);
            if (item != null && !item.Status) // chua su dung
            {
                return Ok(1);
            }
            return BadRequest();
        }

        public IActionResult MiniCart()
        {
            List<ViewCart> dataCart = new List<ViewCart>();
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
            }
            return PartialView(dataCart);
        }

        [NonAction]
        private double Tongtien()
        {
            double total = 0;
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                for (int i = 0; i < dataCart.Count; i++)
                {
                    total += (dataCart[i].Products.Price * dataCart[i].Quantity);
                }
            }
            return total;
        }

        public IActionResult History(int id)
        {
            string OrGuest = HttpContext.Session.GetString(SessionKey.Guest.Guest_FullName);
            if (OrGuest == null || OrGuest == "")
            {
                return RedirectToAction("Index", "Home");
            }
            return View(_orderSvc.GetOrderByGuest(id));
        }

        public IActionResult CancelOrder(int id)
        {
            _orderSvc.CancelOrder(id);
            return Ok();
        }

        public IActionResult OrderDetails(int id)
        {
            var ordetails = _orderDetailSvc.GetOrderDetails(id);
            var order = _context.Orders.Find(id);

            ViewBag.order = order;

            ViewData["Product"] = _context.Products.ToList();
            ViewData["Color"] = _context.Colors.ToList();
            ViewData["Size"] = _context.Sizes.ToList();

            return View(ordetails);
        }

        public IActionResult Details(int id)
        {
            var product = _productSvc.GetProduct(id);

            var brd = _productSvc.GetBrand(product.BrandID);
            ViewData["brdetails"] = brd.BrandName;

            ViewBag.prosz = _context.ProductSizes.ToList();
            ViewBag.sz = _context.Sizes.ToList();

            ViewBag.procol = _context.ProductColors.ToList();
            ViewBag.col = _context.Colors.ToList();

            return View(product);
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            string guest = HttpContext.Session.GetString(SessionKey.Guest.Guest_FullName);
            if (guest != null && guest != "")
            {
                int id = (int)HttpContext.Session.GetInt32(SessionKey.Guest.Guest_ID.ToString());
                ViewBag.InfoUser = _context.Users.Find(id);
            }

            List<ViewCart> dataCart = new List<ViewCart>();
            var cart = HttpContext.Session.GetString("cart");

            ViewBag.sz = _context.Sizes.ToList();
            ViewBag.col = _context.Colors.ToList();

            if (cart != null)
            {
                dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
            }
            return View(dataCart);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult OrderComplete()
        {
            return View();
        }

        public IActionResult ChangePass()
        {
            var user = _userMemSvc.GetUserMem((int)HttpContext.Session.GetInt32(SessionKey.Guest.Guest_ID.ToString()));
            return PartialView(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePass(Users user)
        {
            try
            {
                int id = (int)HttpContext.Session.GetInt32(SessionKey.Guest.Guest_ID.ToString());
                var _user = _userMemSvc.GetUserMem(id);

                if (_encodeHelper.Encode(user.ConfirmPassword) == _user.Password)
                {
                    user.FullName = _user.FullName;
                    user.Gender = _user.Gender;
                    user.Email = _user.Email;
                    user.PhoneNumber = _user.PhoneNumber;
                    user.Address = _user.Address;
                    user.DOB = _user.DOB;
                    user.ImgUser = _user.ImgUser;
                    user.Lock = _user.Lock;
                    user.RoleID = _user.RoleID;
                    user.ConfirmPassword = user.Password;

                    _userMemSvc.EditUserMem(id, user);
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        public IActionResult Info()
        {
            var role = _context.Roles.ToList();
            ViewData["RoleN"] = new SelectList(role, "RoleID", "Role");

            int id = (int)HttpContext.Session.GetInt32(SessionKey.Guest.Guest_ID.ToString());
            var _user = _userMemSvc.GetUserMem(id);

            return View(_user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Info(Users user)
        {
            string thumuccon = "avatar";
            int id = (int)HttpContext.Session.GetInt32(SessionKey.Guest.Guest_ID.ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                    if (user.ImageUser != null && user.ImageUser.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnviroment.WebRootPath, "images");
                        _uploadHelper.UploadImage(user.ImageUser, rootPath, thumuccon);
                        user.ImgUser = user.ImageUser.FileName;
                    }
                    _userMemSvc.EditUserMem(id, user);
                    return RedirectToAction(nameof(InfoMenu), new { id = user.UserID });
                //}
                //else
                //{
                //    return View(user);
                //}
            }
            catch
            {
                return RedirectToAction(nameof(Info));
            }
        }

        public IActionResult InfoMenu()
        {
            var role = _context.Roles.ToList();
            ViewData["RoleN"] = new SelectList(role, "RoleID", "Role");

            int id = (int)HttpContext.Session.GetInt32(SessionKey.Guest.Guest_ID.ToString());
            var _user = _userMemSvc.GetUserMem(id);

            return View(_user);
        }

        public IActionResult Search(int? page, string searchString)
        {
            if (page == null) page = 1;
            var sizes = _context.Products.Include(b => b.ProductName)
                .OrderBy(b => b.ProductID);
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            ViewData["brand"] = _context.Brands.ToList();

            return View(_productSvc.SneakerSearchString(searchString).ToPagedList(pageNumber, pageSize));
        }
    }

}
