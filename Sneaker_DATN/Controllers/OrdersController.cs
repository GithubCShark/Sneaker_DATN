using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sneaker_DATN.Models;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Sneaker_DATN.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly DataContext _context;
        private IOrderSvc _orderSvc;
        public OrdersController(IOrderSvc orderSvc, DataContext context)
        {
            _context = context;
            _orderSvc = orderSvc;
        }
        // GET: OrdersController
        public ActionResult Index(int? page, string sortOrder, string searchString, string status, string currentFilterSearch, string currentFilterStatus)
        {
            ViewBag.Status = (from r in _context.Orders
                              select r.Status).Distinct();

            if (status != null)
            {
                page = 1;
            }
            else
            {
                status = currentFilterStatus;
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilterSearch;
            }
            ViewBag.currentFilterStatus = status;
            ViewBag.currentFilterSearch = searchString;

            var sizes = from r in _context.Orders
                        select r;
            if (!String.IsNullOrEmpty(status))
            {
                sizes = sizes.Where(s => s.Status == status);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                sizes = sizes.Where(s => s.FullName.ToUpper().Contains(searchString.ToUpper()));
            }

            if (page == null) page = 1;
            //var sizes = _context.Orders.Include(b => b.FullName).OrderBy(b => b.OrderID);
            int pageSize = 6;
            int pageNumber = (page ?? 1);


            // 1. Thêm biến NameSortParm để biết trạng thái sắp xếp tăng, giảm ở View
            ViewBag.DateCreateSortParm = String.IsNullOrEmpty(sortOrder) ? "datecreate_desc" : "";
            ViewBag.TotalSortParm = sortOrder == "total" ? "total_desc" : "total";
            ViewBag.PaymentAmountSortParm = sortOrder == "paymentamount" ? "paymentamount_desc" : "paymentamount";


            // 3. Thứ tự sắp xếp theo thuộc tính LinkName
            switch (sortOrder)
            {
                // 3.1 Nếu biến sortOrder sắp giảm thì sắp giảm theo LinkName
                case "datecreate_desc":
                    sizes = sizes.OrderBy(s => s.DateCreate);
                    break;

                case "paymentamount":
                    sizes = sizes.OrderBy(s => s.PaymentAmount);
                    break;
                case "paymentamount_desc":
                    sizes = sizes.OrderByDescending(s => s.PaymentAmount);
                    break;

                case "total":
                    sizes = sizes.OrderBy(s => s.Total);
                    break;
                case "total_desc":
                    sizes = sizes.OrderByDescending(s => s.Total);
                    break;

                // 3.2 Mặc định thì sẽ sắp tăng
                default:
                    sizes = sizes.OrderByDescending(s => s.DateCreate);
                    break;
            }


            return View(sizes.ToList().ToPagedList(pageNumber, pageSize));
            //return View(_orderSvc.GetOrderAll());
        }

        // GET: OrdersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdersController/Edit/5
        public ActionResult Edit(int id)
        {
            Orders lsOrders = _orderSvc.GetOrder(id);
            return View(lsOrders);
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Orders orders)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _orderSvc.EditOrder(id, orders);
                }
                catch
                {
                    
                }
                return RedirectToAction(nameof(Index), new { id = orders.OrderID });
            }
            return PartialView(orders);

        }
    }
}
