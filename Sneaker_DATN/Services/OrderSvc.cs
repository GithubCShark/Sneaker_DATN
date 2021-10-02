using Sneaker_DATN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Services
{
    public interface IOrderSvc
    {
        List<Orders> GetOrderAll();
        Orders GetOrder(int id);
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

        public List<Orders> GetOrderAll()
        {
            List<Orders> list = new List<Orders>();
            list = _context.Orders.ToList();
            return list;
        }

    }
}
