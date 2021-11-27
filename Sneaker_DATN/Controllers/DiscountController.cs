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
        public ActionResult Index(int? page, string sortOrder, string sortProperty, string searchString, string currentSearch)
        {
            if (page == null) page = 1;
            var sizes = _context.Discounts.Include(b => b.VoucherCode).OrderBy(b => b.VoucherId);
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            // 1. Thêm biến NameSortParm để biết trạng thái sắp xếp tăng, giảm ở View
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateUseSortParm = sortOrder == "dateuse" ? "dateuse_desc" : "dateuse";
            ViewBag.DayEndSortParm = sortOrder == "dayend" ? "dayend_desc" : "dayend";
            ViewBag.DayStartSortParm = sortOrder == "daystart" ? "daystart_desc" : "daystart";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.PercentSortParm = sortOrder == "percent" ? "percent_desc" : "percent";
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";

            // 2. Truy vấn lấy tất cả đường dẫn
            var links = from l in _context.Discounts
                        select l;

            // 3. Thứ tự sắp xếp theo thuộc tính LinkName
            switch (sortOrder)
            {
                // 3.1 Nếu biến sortOrder sắp giảm thì sắp giảm theo LinkName
                case "name_desc":
                    links = links.OrderBy(s => s.VoucherId);
                    break;

                case "dateuse":
                    links = links.OrderBy(s => s.DateUse);
                    break;
                case "dateuse_desc":
                    links = links.OrderByDescending(s => s.DateUse);
                    break;

                case "dayend":
                    links = links.OrderBy(s => s.DayEnd);
                    break;
                case "dayend_desc":
                    links = links.OrderByDescending(s => s.DayEnd);
                    break;

                case "daystart":
                    links = links.OrderBy(s => s.DayStart);
                    break;
                case "daystart_desc":
                    links = links.OrderByDescending(s => s.DayStart);
                    break;

                case "price":
                    links = links.OrderBy(s => s.DayStart);
                    break;
                case "price_desc":
                    links = links.OrderByDescending(s => s.DayStart);
                    break;

                case "percent":
                    links = links.OrderBy(s => s.DayStart);
                    break;
                case "percent_desc":
                    links = links.OrderByDescending(s => s.DayStart);
                    break;

                case "status":
                    links = links.OrderBy(s => s.Status);
                    break;
                case "status_desc":
                    links = links.OrderByDescending(s => s.Status);
                    break;

                // 3.2 Mặc định thì sẽ sắp tăng
                default:
                    links = links.OrderByDescending(s => s.VoucherId);
                    break;
            }
            // 4.Search
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentSearch;
            }
            ViewBag.currentSearch = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                links = links.Where(s => s.VoucherCode.ToUpper().Contains(searchString.ToUpper()));
            }

            ViewBag.user = _context.Users.ToList();
            return View(links.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: KhachhangController/Details/5du
        public ActionResult Details(int id)
        {
            return PartialView(_discountSvc.GetDiscount(id));
        }
        public ActionResult Create()
        {
            var item = new Discounts();
            return PartialView(item);
        }

        // POST: MonAnController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Discounts discount)
        {
            try
            {
                _discountSvc.AddDiscount(discount);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return PartialView();
            }
        }



        // GET: KhachhangController/Edit/5
        public ActionResult Edit(int id)
        {
            var discount = _discountSvc.GetDiscount(id);
            return PartialView(discount);

        }

        // POST: KhachhangController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Discounts discount)
        {
            try
            {
                _discountSvc.EditDiscount(id, discount);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return PartialView();
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.VoucherId == id);
            if (discount == null)
            {
                return NotFound();
            }

            return PartialView(discount);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if( discount.CustomerId == 0)
            {
                _context.Discounts.Remove(discount);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountExists(int id)
        {
            return _context.Discounts.Any(e => e.VoucherId == id);
        }
    }
}
