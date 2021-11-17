using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sneaker_DATN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    public class ReportController : Controller
    {
        private DataContext _context;
        public ReportController(DataContext dataContext)
        {
            _context = dataContext;
        }
        // GET: ReportController
        public IActionResult Index(int search)
        {
            var lsProduct = from x in _context.Products
                         select x;
            var lsOrderDetails = from y in _context.OrderDetails
                                 select y;

            var lsorderd = from y in _context.OrderDetails
                           select y.ProductID;
            var lsprod = lsProduct.Where(x => lsorderd.Contains(x.ProductID)).ToList();

            List<ViewReportProducts> lsReport = new List<ViewReportProducts>();

            for (int i = 0; i < lsprod.Count; i++)
            {
                var lsquantity = from x in lsOrderDetails
                                 where x.ProductID == lsprod[i].ProductID
                                 select x.Quantity;
                var lstotal = from x in lsOrderDetails
                              where x.ProductID == lsprod[i].ProductID
                              select x.Price;

                lsReport.Add(new ViewReportProducts
                {
                    Products = lsprod[i],
                    Quantity = lsquantity.Sum(),
                    Total = lstotal.Sum()
                });
            }
            //foreach (var item in lsprod)
            //{
            //    var lsquantity = from x in lsOrderDetails
            //                     where x.ProductID == item.ProductID
            //                     select x.Quantity;
            //    var lstotal = from x in lsOrderDetails
            //                  where x.ProductID == item.ProductID
            //                  select x.Price;

            //    lsReport.Add(new ViewReportProducts
            //    {
            //        Products = item,
            //        Quantity = lsquantity.Sum(),
            //        Total = lstotal.Sum()
            //    });
            //}
            var lsrp = lsReport.OrderBy(x => x.Products.ProductID).ToList();

            return View(lsrp);
        }
    }
}
