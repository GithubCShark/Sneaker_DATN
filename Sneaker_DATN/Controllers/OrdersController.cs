﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sneaker_DATN.Models;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    public class OrdersController : BaseController
    {
        private IOrderSvc _orderSvc;
        public OrdersController(IOrderSvc orderSvc)
        {
            _orderSvc = orderSvc;
        }
        // GET: OrdersController
        public ActionResult Index()
        {
            return View(_orderSvc.GetOrderAll());
        }

        // GET: OrdersController/Details/5
        public ActionResult Details(int id)
        {
            Orders lsOrders = _orderSvc.GetOrder(id);
            return View(lsOrders);
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
        public ActionResult Edit(int id, Orders orders)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _orderSvc.EditOrder(id, orders);

                    return RedirectToAction(nameof(Index), new { id = orders.OrderID });
                }
                else
                {
                    return View(orders);
                }
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: OrdersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
