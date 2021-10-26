using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly DataContext _context;
        public HomeController(IUserMemSvc userMemSvc, IProductSvc productSvc, IWebHostEnvironment webHostEnvironment, IUploadHelper uploadHelper, IOrderSvc orderSvc, IOrderDetailSvc orderDetailSvc, DataContext context)
        {
            _userMemSvc = userMemSvc;
            _productSvc = productSvc;
            _webHostEnviroment = webHostEnvironment;
            _uploadHelper = uploadHelper;
            _orderSvc = orderSvc;
            _orderDetailSvc = orderDetailSvc;
            _context = context;
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
                    //return RedirectToAction(nameof(Index), "Home");
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
            // Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.
            // Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;
            // Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo BookID mới có thể phân trang.
            var sizes = _context.Products.Include(b => b.ProductName)
                .OrderBy(b => b.ProductID);
            // Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 4;
            // Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            // Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
            //int checkTotal = (int)(sizes.ToList().Count / pageSize) + 1;
            //// Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            //if (pageNumber > checkTotal) pageNumber = checkTotal;
            ViewData["brand"] = _context.Brands.ToList();
            return View(_context.Products.ToPagedList(pageNumber, pageSize));
            //return View(await _context.Sizes.ToListAsync());

            //return View(_productSvc.GetProductAll());
        }
        public IActionResult Index()
        {
            return View(_productSvc.GetProductAll());
        }
        #region cart
        public IActionResult AddCart(int id)
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
                        Quantity = 1
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
                        Quantity = 1
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
            if (cart != null)
            {
                dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
            }
            return View(dataCart);
        }
        public IActionResult UpdateCart(int id, int soluong)
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

        public IActionResult OrderCart()
        {
            string guest_Name = HttpContext.Session.GetString(SessionKey.Guest.Guest_FullName);
            if (guest_Name == null || guest_Name == "")  // đã có session
            {
                return BadRequest();
            }
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null && cart.Count() > 0)
            {
                #region DonHang
                var G_Context = HttpContext.Session.GetString(SessionKey.Guest.GuestContext);
                var khachhangId = JsonConvert.DeserializeObject<Users>(G_Context).UserID;

                double total = Tongtien();

                var order = new Orders()
                {
                    UserID = khachhangId,
                    Total = total,
                    DateCreate = DateTime.Now,
                    Note = "",

                };

                _orderSvc.AddOrder(order);
                int orderiD = order.OrderID;

                #region Chitiet
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                for (int i = 0; i < dataCart.Count; i++)
                {
                    OrderDetails detail = new OrderDetails()
                    {
                        OrderID = orderiD,
                        ProductID = dataCart[i].Products.ProductID,
                        Quantity = dataCart[i].Quantity,
                        Price = dataCart[i].Products.Price * dataCart[i].Quantity,

                    };
                    _orderDetailSvc.AddOrderDetail(detail);
                }

                #endregion
                #endregion
                #endregion


                HttpContext.Session.Remove("cart");

                return Ok();
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
        public IActionResult Details(int id)
        {
            var product = _productSvc.GetProduct(id);

            var brd = _productSvc.GetBrand(product.BrandID);
            ViewData["brdetails"] = brd.BrandName;

            return View(product);
        }

        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
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
            return View();
        }
        public IActionResult Info()
        {
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ChangePass()
        //{
        //    return View();
        //}
    }

}
