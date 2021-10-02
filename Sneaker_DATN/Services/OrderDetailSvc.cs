using Sneaker_DATN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Services
{
    public interface IOrderDetailSvc
    {
        List<OrderDetails> GetOrderDetailsAll();
    }
    public class OrderDetailSvc : IOrderDetailSvc
    {
        protected DataContext _context;
        public OrderDetailSvc(DataContext context)
        {
            _context = context;
        }
        public List<OrderDetails> GetOrderDetailsAll()
        {
            List<OrderDetails> list = new List<OrderDetails>();
            list = _context.OrderDetails.ToList();
            return list;
        }
    }
}
