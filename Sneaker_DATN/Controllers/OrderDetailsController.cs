using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sneaker_DATN.Models;
using Sneaker_DATN.Services;

namespace Sneaker_DATN.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly DataContext _context;
        private IOrderDetailSvc _orderDetailSvc;
        private IOrderSvc _orderSvc;

        public OrderDetailsController(DataContext context, IOrderDetailSvc orderDetailSvc, IOrderSvc orderSvc)
        {
            _orderSvc = orderSvc;
            _context = context;
            _orderDetailSvc = orderDetailSvc;
        }

        // GET: OrderDetails
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ordetails = _orderDetailSvc.GetOrderDetails(id);
            var order = _context.Orders.Find(id);

            ViewBag.order = order;
            

            //var order = _orderSvc.GetOrder(id);
        
            //if (id == null)
            //{
            //    return NotFound();
            //}
            //var orderDetails = await _context.OrderDetails
            //    .Include(o => o.Colors)
            //    .Include(o => o.Orders)
            //    .Include(o => o.Products)
            //    .Include(o => o.Sizes)
            //    .FirstOrDefaultAsync(m => m.OrderID == id);
            //if (orderDetails == null)
            //{
            //    return NotFound();
            //}

            return View(ordetails);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "ColorID");
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "Address");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName");
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeID");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,ProductID,Quantity,Price,SizeID,ColorID")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "ColorID", orderDetails.ColorID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "Address", orderDetails.OrderID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", orderDetails.ProductID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeID", orderDetails.SizeID);
            return View(orderDetails);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails.FindAsync(id);
            if (orderDetails == null)
            {
                return NotFound();
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "ColorID", orderDetails.ColorID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "Address", orderDetails.OrderID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", orderDetails.ProductID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeID", orderDetails.SizeID);
            return View(orderDetails);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,ProductID,Quantity,Price,SizeID,ColorID")] OrderDetails orderDetails)
        {
            if (id != orderDetails.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailsExists(orderDetails.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "ColorID", orderDetails.ColorID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "Address", orderDetails.OrderID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", orderDetails.ProductID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeID", orderDetails.SizeID);
            return View(orderDetails);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails
                .Include(o => o.Colors)
                .Include(o => o.Orders)
                .Include(o => o.Products)
                .Include(o => o.Sizes)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetails = await _context.OrderDetails.FindAsync(id);
            _context.OrderDetails.Remove(orderDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailsExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderID == id);
        }
    }
}
