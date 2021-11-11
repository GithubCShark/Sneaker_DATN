using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sneaker_DATN.Models;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    public class DiscountController : Controller
    {
        // GET:KhachhangController
        private IDiscountSvc _discountSvc;
        DataContext _context;

        public DiscountController(IDiscountSvc discountSvc, DataContext context)
        {
            _discountSvc = discountSvc;
            _context = context;
        }

        // GET: MonAnController
        public ActionResult Index()
        {
            ViewBag.user = _context.Users.ToList();
            return View(_discountSvc.GetDiscountAll());
        }

        // GET: KhachhangController/Details/5du
        public ActionResult Details(int id)
        {
            return View(_discountSvc.GetDiscount(id));
        }
        public ActionResult Create()
        {
            //System.Guid guid = System.Guid.NewGuid();
            var item = new Discounts();
            //item.VoucherCode = guid.ToString();
            return View(item);
        }

        // POST: MonAnController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Discounts discount)
        {
            try
            {
                _discountSvc.AddDiscount(discount);
                return RedirectToAction(nameof(Details), new { id = discount.VoucherId });
            }
            catch
            {
                return View();
            }
        }



        // GET: KhachhangController/Edit/5
        public ActionResult Edit(int id)
        {
            var discount = _discountSvc.GetDiscount(id);
            return View(discount);

        }

        // POST: KhachhangController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Discounts discount)
        {
            try
            {
                _discountSvc.EditDiscount(id, discount);
                return RedirectToAction(nameof(Details), new { id = discount.VoucherId });
            }
            catch
            {
                return View();
            }
        }
    }
}
