using Microsoft.EntityFrameworkCore;
using Sneaker_DATN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Services
{
    public interface IOrderSvc
    {
        List<Orders> GetOrderByGuest(int id);

        Orders GetOrder(int id);

        int AddOrder(Orders orders);

        int EditOrder(int id, Orders orders);

        int CancelOrder(int id);

    }
    public class OrderSvc : IOrderSvc
    {
        protected DataContext _context;
        public OrderSvc(DataContext context)
        {
            _context = context;
        }

        public Orders GetOrder(int id)
        {
            Orders order = null;
            order = _context.Orders.Find(id);
            return order;
        }

        public List<Orders> GetOrderByGuest(int id)
        {
            List<Orders> list = new List<Orders>();

            list = _context.Orders.Where(x => x.UserID == id).ToList();
            return list;
        }

        public int AddOrder(Orders orders)
        {
            int ret = 0;
            try
            {
                orders.DateCreate = DateTime.Now;
                _context.Add(orders);
                _context.SaveChanges();
                ret = orders.OrderID;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public int EditOrder(int id, Orders orders)
        {
            int ret = 0;
            try
            {
                Orders _orders = null;
                _orders = _context.Orders.Find(id);
                _orders.Status = orders.Status;

                _context.Update(_orders);
                _context.SaveChanges();
                ret = orders.OrderID;
            }
            catch (Exception)
            {
                ret = 0;
            }
            return ret;
        }

        public int CancelOrder(int id)
        {
            int ret = 0;
            try
            {
                var order = _context.Orders.Find(id);
                order.Status = "Đã hủy";
                _context.Update(order);
                _context.SaveChanges();
                ret = id;
            }
            catch (Exception)
            {
                ret = 0;
            }
            return ret;
        }
    }
}
