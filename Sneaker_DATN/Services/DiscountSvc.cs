using Sneaker_DATN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Services
{
    public interface IDiscountSvc
    {
        List<Discounts> GetDiscountAll();
        Discounts GetDiscount(int id);
        Discounts GetDiscount(string voucherCode);
        int AddDiscount(Discounts discounts);
        int EditDiscount(int id, Discounts discounts);
    }
    public class DiscountSvc : IDiscountSvc
    {
        protected DataContext _context;
        public DiscountSvc(DataContext context)
        {
            _context = context;
        }
        public List<Discounts> GetDiscountAll()
        {
            List<Discounts> list = new List<Discounts>();

            list = _context.Discounts.OrderByDescending(x => x.DayEnd).ToList();
            return list;
        }

        public Discounts GetDiscount(string voucherCode)
        {

            var item = _context.Discounts.Where(x => x.VoucherCode == voucherCode).FirstOrDefault();

            return item;
        }
        public Discounts GetDiscount(int id)
        {
            Discounts discounts = null;
            discounts = _context.Discounts.Where(x => x.VoucherId == id).FirstOrDefault();

            //product = _context.Products.Where(e => e.Id==id).FirstOrDefault(); //cách tổng quát
            return discounts;
        }
        public int AddDiscount(Discounts discounts)
        {
            int ret = 0;
            try
            {
                _context.Add(discounts);
                _context.SaveChanges();
                ret = discounts.VoucherId;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public int EditDiscount(int id, Discounts discounts)
        {
            int ret = 0;
            try
            {
                Discounts _discounts = null;
                _discounts = _context.Discounts.Find(id); //cách này chỉ dùng cho Khóa chính

                _discounts.VoucherId = discounts.VoucherId;
                _discounts.DayStart = discounts.DayStart;
                _discounts.DayEnd = discounts.DayEnd;
                _discounts.CustomerId = discounts.CustomerId;
                _discounts.DateUse = discounts.DateUse;
                _discounts.Classify = discounts.Classify;
                _discounts.Percent = discounts.Percent;
                _discounts.Price = discounts.Price;

                _context.Update(_discounts);
                _context.SaveChanges();
                ret = discounts.VoucherId;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
    }
}
