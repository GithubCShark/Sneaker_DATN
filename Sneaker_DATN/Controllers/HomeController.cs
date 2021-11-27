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
using MailKit.Net.Smtp;
using MimeKit;
using static Sneaker_DATN.Filters.AuthenticationFilterAttribute;
using System.Text;
using Microsoft.Extensions.Configuration;
using PayPal.Core;
using PayPal.v1.Payments;
using BraintreeHttp;

namespace Sneaker_DATN.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IUserMemSvc _userMemSvc;
        private readonly IProductSvc _productSvc;
        private readonly IUploadHelper _uploadHelper;
        private readonly IOrderSvc _orderSvc;
        private readonly IOrderDetailSvc _orderDetailSvc;
        private readonly IDiscountSvc _discountSvc;
        private readonly DataContext _context;
        private readonly string _clientId;
        private readonly string _secretKey;
        protected IEncodeHelper _encodeHelper;
        protected ISendMailService _sendGmail;
        public double tyGiaUSD = 23200;
        public HomeController(IUserMemSvc userMemSvc, IProductSvc productSvc, IWebHostEnvironment webHostEnvironment,
             IUploadHelper uploadHelper, IOrderSvc orderSvc, IOrderDetailSvc orderDetailSvc, DataContext context,
             IEncodeHelper encodeHelper, IDiscountSvc discountSvc, ISendMailService sendGmail, IConfiguration config)
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
            _sendGmail = sendGmail;
            _clientId = config["PayPalSettings:ClientId"];
            _secretKey = config["PayPalSettings:SecretKey"];
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
                else
                {
                    ModelState.AddModelError("loginError", "Tài khoản hoặc mật khẩu sai.");
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

        public IActionResult Products(int? page, string sortOrder, string searchString, string brands,
            int sizes, int colors, string currentFilterSearch, string currentFilterBrand, int currentFilterSize,
            int currentFilterColor)
        {
            ViewBag.BrandName = (from r in _context.Brands
                                 select r.BrandName).Distinct();
            ViewBag.Color = (from r in _context.Colors
                             select r.Color).Distinct();
            ViewBag.Size = (from r in _context.Sizes
                            select r.Size).Distinct();

            if (brands != null || colors != 0 || sizes != 0)
            {
                page = 1;
            }
            else
            {
                brands = currentFilterBrand;
                colors = currentFilterColor;
                sizes = currentFilterSize;
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilterSearch;
            }
            ViewBag.currentFilterBrand = brands;
            ViewBag.currentFilterColor = colors;
            ViewBag.currentFilterSize = sizes;
            ViewBag.currentFilterSearch = searchString;

            var productFilters = from r in _context.Products
                                 select r;

            if (!String.IsNullOrEmpty(brands))
            {
                productFilters = productFilters.Where(s => s.Brands.BrandName == brands);
            }
            if (colors != 0)
            {
                var lspro = from r in _context.ProductColors
                            where r.ColorID == colors
                            select r.Products.ProductID;

                productFilters = productFilters.Where(s => lspro.Contains(s.ProductID));
            }
            if (sizes != 0)
            {
                var lssiz = from r in _context.ProductSizes
                            where r.IdSize == sizes
                            select r.Products.ProductID;

                productFilters = productFilters.Where(s => lssiz.Contains(s.ProductID));
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                productFilters = productFilters.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper()));
            }

            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            // 1. Thêm biến NameSortParm để biết trạng thái sắp xếp tăng, giảm ở View
            ViewBag.ProductSortParm = String.IsNullOrEmpty(sortOrder) ? "product_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";


            // 3. Thứ tự sắp xếp theo thuộc tính LinkName
            switch (sortOrder)
            {
                // 3.1 Nếu biến sortOrder sắp giảm thì sắp giảm theo LinkName
                case "product_desc":
                    productFilters = productFilters.OrderByDescending(s => s.Status);
                    break;

                case "price":
                    productFilters = productFilters.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    productFilters = productFilters.OrderByDescending(s => s.Price);
                    break;

                // 3.2 Mặc định thì sẽ sắp tăng
                default:
                    productFilters = productFilters.OrderBy(s => s.Status);
                    break;
            }


            ViewData["brand"] = _context.Brands.ToList();
            ViewData["size"] = _context.Sizes.ToList();
            ViewData["color"] = _context.Colors.ToList();

            return View(productFilters.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Index(int? page, string sortOrder)
        {
            if (page == null) page = 1;
            var products = _context.Products.Include(b => b.ProductName)
                .OrderBy(b => b.ProductID);
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var productFilters = from r in _context.Products
                                 select r;

            ViewBag.ProductSortParm = String.IsNullOrEmpty(sortOrder) ? "product_desc" : "";

            switch (sortOrder)
            {
                case "product_desc":
                    productFilters = productFilters.OrderByDescending(s => s.Status);
                    break;
                    
                default:
                    productFilters = productFilters.OrderBy(s => s.Status);
                    break;
            }

            return View(productFilters.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult AddCart(int id, int size, int color, int quantity)
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
                        Quantity = quantity,
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
                        Quantity = quantity,
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

        public IActionResult OrderCart(string voucherCode, string Note)
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
                    Note = Note,
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
                    if (dataCart[i].Products.Sale > 0)
                    {
                        OrderDetails details = new OrderDetails()
                        {
                            OrderID = orderID,
                            ProductID = dataCart[i].Products.ProductID,
                            Quantity = dataCart[i].Quantity,
                            Price = (dataCart[i].Products.Price - dataCart[i].Products.Sale) * dataCart[i].Quantity,
                            ColorID = dataCart[i].Color,
                            SizeID = dataCart[i].Size
                        };
                        _context.OrderDetails.Add(details);
                        _context.SaveChanges();
                    }
                    else
                    {
                        OrderDetails details = new OrderDetails()
                        {
                            OrderID = orderID,
                            ProductID = dataCart[i].Products.ProductID,
                            Quantity = dataCart[i].Quantity,
                            Price = dataCart[i].Products.Price * dataCart[i].Quantity,
                            ColorID = dataCart[i].Color,
                            SizeID = dataCart[i].Size
                        };
                        _context.OrderDetails.Add(details);
                        _context.SaveChanges();
                    }
                }
                return Ok();

            }
            return RedirectToAction(nameof(OrderComplete), "Home");
        }

        public async Task<IActionResult> PaypalCheckOut()
        {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);
            var cart = HttpContext.Session.GetString("cart");

            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };

            var total = Math.Round(Tongtien() / tyGiaUSD, 2);
            List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
            foreach (var item in dataCart)
            {
                if (item.Products.Sale > 0)
                {
                    itemList.Items.Add(new Item()
                    {
                        Name = item.Products.ProductName,
                        Currency = "USD",
                        Price = Math.Round(((item.Products.Price - item.Products.Sale) * item.Quantity) / tyGiaUSD, 2).ToString(),
                        Quantity = item.Quantity.ToString(),
                        Sku = "Sku",
                        Tax = "0"
                    });
                }
                else
                {
                    itemList.Items.Add(new Item()
                    {
                        Name = item.Products.ProductName,
                        Currency = "USD",
                        Price = Math.Round((item.Products.Price * item.Quantity) / tyGiaUSD, 2).ToString(),
                        Quantity = item.Quantity.ToString(),
                        Sku = "Sku",
                        Tax = "0"
                    });
                }
            }

            var paypalOrderId = DateTime.Now.Ticks;
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
            {
                new Transaction()
                {
                    Amount = new Amount()
                    {
                        Total = total.ToString(),
                        Currency = "USD",
                        Details = new AmountDetails
                        {
                            Tax = "0",
                            Shipping = "0",
                            Subtotal = total.ToString()
                        }
                    },
                    ItemList = itemList,
                    Description = $"Invoice #{paypalOrderId}",
                    InvoiceNumber = paypalOrderId.ToString()
                }
            },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"{hostname}/Home/CheckoutFailed",
                    ReturnUrl = $"{hostname}/Home/CheckoutComplete"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return Redirect("/Home/CheckOutFailed");
            }
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
                    if (dataCart[i].Products.Sale > 0)
                    {
                        total += ((dataCart[i].Products.Price - dataCart[i].Products.Sale) * dataCart[i].Quantity);
                    }
                    else
                    {
                        total += (dataCart[i].Products.Price * dataCart[i].Quantity);
                    }
                }
            }
            return total;
        }

        public IActionResult History(int id, int? page, string sortOrder)
        {
            string OrGuest = HttpContext.Session.GetString(SessionKey.Guest.Guest_FullName);
            if (OrGuest == null || OrGuest == "")
            {
                return RedirectToAction("Index", "Home");
            }

            if (page == null) page = 1;
            var products = _context.Orders.Include(b => b.UserID)
                .OrderBy(b => b.OrderID);
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var orderSorters = from r in _orderSvc.GetOrderByGuest(id)
                                 select r;

            ViewBag.DateCreateSortParm = String.IsNullOrEmpty(sortOrder) ? "datecreate_desc" : "";
            ViewBag.TotalSortParm = sortOrder == "total" ? "total_desc" : "total";
            ViewBag.PaymentSortParm = sortOrder == "payment" ? "payment_desc" : "payment";

            switch (sortOrder)
            {
                case "datecreate_desc":
                    orderSorters = orderSorters.OrderBy(s => s.DateCreate);
                    break;

                case "total":
                    orderSorters = orderSorters.OrderBy(s => s.Total);
                    break;
                case "total_desc":
                    orderSorters = orderSorters.OrderByDescending(s => s.Total);
                    break;

                case "payment":
                    orderSorters = orderSorters.OrderBy(s => s.PaymentAmount);
                    break;
                case "payment_desc":
                    orderSorters = orderSorters.OrderByDescending(s => s.PaymentAmount);
                    break;

                default:
                    orderSorters = orderSorters.OrderByDescending(s => s.DateCreate);
                    break;
            }

            return View(orderSorters.ToPagedList(pageNumber, pageSize));
            //return View(_orderSvc.GetOrderByGuest(id));
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

        public IActionResult FlashSale(int? page, string sortOrder, string searchString, string brands,
            int sizes, int colors, string currentFilterSearch, string currentFilterBrand, int currentFilterSize,
            int currentFilterColor)
        {
            ViewBag.BrandName = (from r in _context.Brands
                                 select r.BrandName).Distinct();
            ViewBag.Color = (from r in _context.Colors
                             select r.Color).Distinct();
            ViewBag.Size = (from r in _context.Sizes
                            select r.Size).Distinct();

            if (brands != null || colors != 0 || sizes != 0)
            {
                page = 1;
            }
            else
            {
                brands = currentFilterBrand;
                colors = currentFilterColor;
                sizes = currentFilterSize;
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilterSearch;
            }
            ViewBag.currentFilterBrand = brands;
            ViewBag.currentFilterColor = colors;
            ViewBag.currentFilterSize = sizes;
            ViewBag.currentFilterSearch = searchString;

            var productFilters = from r in _context.Products
                                  where r.Status == false && r.Sale > 0
                                 select r;

            if (!String.IsNullOrEmpty(brands))
            {
                productFilters = productFilters.Where(s => s.Brands.BrandName == brands);
            }
            if (colors != 0)
            {
                var lspro = from r in _context.ProductColors
                            where r.ColorID == colors
                            select r.Products.ProductID;

                productFilters = productFilters.Where(s => lspro.Contains(s.ProductID));
            }
            if (sizes != 0)
            {
                var lssiz = from r in _context.ProductSizes
                            where r.IdSize == sizes
                            select r.Products.ProductID;

                productFilters = productFilters.Where(s => lssiz.Contains(s.ProductID));
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                productFilters = productFilters.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper()));
            }

            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            // 1. Thêm biến NameSortParm để biết trạng thái sắp xếp tăng, giảm ở View
            ViewBag.ProductSortParm = String.IsNullOrEmpty(sortOrder) ? "product_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";


            // 3. Thứ tự sắp xếp theo thuộc tính LinkName
            switch (sortOrder)
            {
                // 3.1 Nếu biến sortOrder sắp giảm thì sắp giảm theo LinkName
                case "product_desc":
                    productFilters = productFilters.OrderBy(s => s.ProductID);
                    break;

                case "price":
                    productFilters = productFilters.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    productFilters = productFilters.OrderByDescending(s => s.Price);
                    break;

                // 3.2 Mặc định thì sẽ sắp tăng
                default:
                    productFilters = productFilters.OrderByDescending(s => s.ProductID);
                    break;
            }


            ViewData["brand"] = _context.Brands.ToList();
            ViewData["size"] = _context.Sizes.ToList();
            ViewData["color"] = _context.Colors.ToList();

            return View(productFilters.ToPagedList(pageNumber, pageSize));
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

        public IActionResult CheckoutComplete()
        {
            // Add OrderDetails
            var GuestContext = HttpContext.Session.GetString(SessionKey.Guest.GuestContext);
            var GuestID = JsonConvert.DeserializeObject<Users>(GuestContext).UserID;
            var user = _context.Users.Find(GuestID);
            var cart = HttpContext.Session.GetString("cart");
            double tong = Tongtien();

            var order = new Orders()
            {
                UserID = GuestID,
                DateCreate = DateTime.Now,
                Total = tong,
                PaymentAmount = tong,
                Address = user.Address,
                Status = "Đang xử lý",
                Note = "PayPal",
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Add Orders
            int orderID = order.OrderID;
            List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
            for (int i = 0; i < dataCart.Count; i++)
            {
                if (dataCart[i].Products.Sale > 0)
                {
                    OrderDetails details = new OrderDetails()
                    {
                        OrderID = orderID,
                        ProductID = dataCart[i].Products.ProductID,
                        Quantity = dataCart[i].Quantity,
                        Price = (dataCart[i].Products.Price - dataCart[i].Products.Sale) * dataCart[i].Quantity,
                        ColorID = dataCart[i].Color,
                        SizeID = dataCart[i].Size
                    };
                    _context.OrderDetails.Add(details);
                    _context.SaveChanges();
                }
                else
                {
                    OrderDetails details = new OrderDetails()
                    {
                        OrderID = orderID,
                        ProductID = dataCart[i].Products.ProductID,
                        Quantity = dataCart[i].Quantity,
                        Price = dataCart[i].Products.Price * dataCart[i].Quantity,
                        ColorID = dataCart[i].Color,
                        SizeID = dataCart[i].Size
                    };
                    _context.OrderDetails.Add(details);
                    _context.SaveChanges();
                }
            }

            HttpContext.Session.Remove("cart");
            return View();
        }

        public IActionResult CheckoutFailed()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult OrderComplete()
        {
            HttpContext.Session.Remove("cart");
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
                    if (user.ImageUser != null && user.ImageUser.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnviroment.WebRootPath, "images");
                        _uploadHelper.UploadImage(user.ImageUser, rootPath, thumuccon);
                        user.ImgUser = user.ImageUser.FileName;
                    }
                    _userMemSvc.EditUserMem(id, user);
                    return RedirectToAction(nameof(InfoMenu), new { id = user.UserID });
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

        [NonAction]
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
                var user = _userMemSvc.GetAllUserMem().Where(u => u.Email == email).FirstOrDefault();
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomString(4, true));
                builder.Append(RandomNumber(1000, 9999));
                builder.Append(RandomString(2, false));

                _userMemSvc.ChangePass(user.UserID, builder.ToString());
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
