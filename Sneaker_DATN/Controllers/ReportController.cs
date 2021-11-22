using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sneaker_DATN.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            List<ViewReport> lsrp = new List<ViewReport>();
            List<ViewReport> lsReport = new List<ViewReport>();
            var lsBrands = from z in _context.Brands
                           select z;
            //1
            var lsProduct = from x in _context.Products
                            select x;
            var lsOrderDetails = from y in _context.OrderDetails
                                 select y;
            //2
            var lsUser = from x in _context.Users
                            where x.RoleID == 3
                            select x;
            var lsOrders = from y in _context.Orders
                                 select y;

            if (search == 1)
            {
                //Báo cáo doanh thu theo sản phẩm
                lsReport = new List<ViewReport>();
                var lsorderd = from y in lsOrderDetails
                               select y.ProductID;
                var lsprod = lsProduct.Where(x => lsorderd.Contains(x.ProductID)).ToList();

                for (int i = 0; i < lsprod.Count; i++)
                {
                    string tempbrand = "";
                    var lsquantity = from x in lsOrderDetails
                                     where x.ProductID == lsprod[i].ProductID
                                     select x.Quantity;
                    var lstotal = from x in lsOrderDetails
                                  where x.ProductID == lsprod[i].ProductID
                                  select x.Price;
                    foreach (var item in lsBrands)
                    {
                        if (item.BrandID == lsprod[i].BrandID)
                        {
                            tempbrand = item.BrandName;
                        }
                    }
                    lsReport.Add(new ViewReport
                    {
                        Products = lsprod[i],
                        Users = null,
                        BrandName = tempbrand,
                        Quantity = lsquantity.Sum(),
                        Total = lstotal.Sum()
                    });
                }
                lsrp = lsReport.OrderByDescending(x => x.Quantity).ToList();
            }
            else if (search == 2)
            {
                //Báo cáo xếp hạng đơn hàng khách
                lsReport = new List<ViewReport>();
                var lsorder = from y in lsOrders
                               select y.UserID;
                var lsur = lsUser.Where(x => lsorder.Contains(x.UserID)).ToList();

                for (int i = 0; i < lsur.Count; i++)
                {
                    var lsquantity = from x in lsOrders
                                     where x.UserID == lsur[i].UserID
                                     select x.UserID;
                    var lstotal = from x in lsOrders
                                  where x.UserID == lsur[i].UserID
                                  select x.Total;
                    lsReport.Add(new ViewReport
                    {
                        Users = lsur[i],
                        Products = null,
                        Quantity = lsquantity.Count(),
                        Total = lstotal.Sum()
                    });
                }
                lsrp = lsReport.OrderByDescending(x => x.Quantity).ToList();
            }
            else if (search == 3)
            {
                //Báo cáo doanh thu theo thương hiệu
                lsReport = new List<ViewReport>();
                var lsorderd = from y in lsOrderDetails
                               select y.ProductID;
                var lsprod = lsProduct.Where(x => lsorderd.Contains(x.ProductID)).ToList();

                List<ViewReport> temp = new List<ViewReport>();
                for (int i = 0; i < lsprod.Count; i++)
                {
                    var tempbrand = "";
                    var lsquantity = from x in lsOrderDetails
                                     where x.ProductID == lsprod[i].ProductID
                                     select x.Quantity;
                    var lstotal = from x in lsOrderDetails
                                  where x.ProductID == lsprod[i].ProductID
                                  select x.Price;
                    foreach (var item in lsBrands)
                    {
                        if (item.BrandID == lsprod[i].BrandID)
                        {
                            tempbrand = item.BrandName;
                        }
                    }
                    temp.Add(new ViewReport
                    {
                        BrandName = tempbrand,
                        Quantity = lsquantity.Sum(),
                        Total = lstotal.Sum()
                    });
                }
                var grtemp = temp.GroupBy(p => p.BrandName).ToList();
                for (int i = 0; i < grtemp.Count; i++)
                {
                    int grquantity = 0;
                    double grtotal = 0;
                    foreach (var item in temp)
                    {
                        if (item.BrandName == grtemp[i].Key)
                        {
                            grquantity += item.Quantity;
                            grtotal += item.Total;
                        }
                    }
                    lsReport.Add(new ViewReport
                    {
                        Products = null,
                        Users = null,
                        BrandName = grtemp[i].Key,
                        Quantity = grquantity,
                        Total = grtotal
                    });
                }
                lsrp = lsReport.OrderByDescending(x => x.Quantity).ToList();
            }
            else
            {
                lsReport = new List<ViewReport>();
                lsReport.Add(new ViewReport
                {
                    Products = null,
                    Users = null,
                    BrandName = "",
                    Quantity = 0,
                    Total = 0
                });
            }

            return View(lsrp);
        }
        [HttpPost]
        public FileResult Export(int check)
        {
            var day = DateTime.Now.Day;
            var month = DateTime.Now.Month;
            string report = "";
            DataTable dt = new DataTable("Report");

            //Copy
            var lsBrands = from z in _context.Brands
                           select z;
            //1
            var lsProduct = from x in _context.Products
                            select x;
            var lsOrderDetails = from y in _context.OrderDetails
                                 select y;
            //2
            var lsUser = from x in _context.Users
                         where x.RoleID == 3
                         select x;
            var lsOrders = from y in _context.Orders
                           select y;

            if (check == 1)
            {
                report = "TopProducts_";
                dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Tên sản phẩm"),
                                            new DataColumn("Thương hiệu"),
                                            new DataColumn("Số lượng"),
                                            new DataColumn("Tổng tiền") });
                //Copy code
                var lsorderd = from y in lsOrderDetails
                               select y.ProductID;
                var lsprod = lsProduct.Where(x => lsorderd.Contains(x.ProductID)).ToList();

                for (int i = 0; i < lsprod.Count; i++)
                {
                    string tempbrand = "";
                    var lsquantity = from x in lsOrderDetails
                                     where x.ProductID == lsprod[i].ProductID
                                     select x.Quantity;
                    var lstotal = from x in lsOrderDetails
                                  where x.ProductID == lsprod[i].ProductID
                                  select x.Price;
                    foreach (var item in lsBrands)
                    {
                        if (item.BrandID == lsprod[i].BrandID)
                        {
                            tempbrand = item.BrandName;
                        }
                    }
                    dt.Rows.Add(lsprod[i].ProductName, tempbrand, lsquantity.Sum(), lstotal.Sum());
                }
            }
            else if (check == 2)
            {
                report = "TopMembers_";
                dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Tên khách"),
                                            new DataColumn("Email"),
                                            new DataColumn("Ngày sinh"),
                                            new DataColumn("Số lượng"),
                                            new DataColumn("Tổng tiền") });
                //Copy code
                var lsorder = from y in lsOrders
                              select y.UserID;
                var lsur = lsUser.Where(x => lsorder.Contains(x.UserID)).ToList();

                for (int i = 0; i < lsur.Count; i++)
                {
                    var lsquantity = from x in lsOrders
                                     where x.UserID == lsur[i].UserID
                                     select x.UserID;
                    var lstotal = from x in lsOrders
                                  where x.UserID == lsur[i].UserID
                                  select x.Total;

                    dt.Rows.Add(lsur[i].FullName, lsur[i].Email, lsur[i].DOB, lsquantity.Sum(), lstotal.Sum());
                }
            }
            else if (check == 3)
            {
                report = "TopBrands_";
                dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Thương hiệu"),
                                            new DataColumn("Số lượng"),
                                            new DataColumn("Tổng tiền") });

                //Copy code
                var lsorderd = from y in lsOrderDetails
                               select y.ProductID;
                var lsprod = lsProduct.Where(x => lsorderd.Contains(x.ProductID)).ToList();

                List<ViewReport> temp = new List<ViewReport>();
                for (int i = 0; i < lsprod.Count; i++)
                {
                    var tempbrand = "";
                    var lsquantity = from x in lsOrderDetails
                                     where x.ProductID == lsprod[i].ProductID
                                     select x.Quantity;
                    var lstotal = from x in lsOrderDetails
                                  where x.ProductID == lsprod[i].ProductID
                                  select x.Price;
                    foreach (var item in lsBrands)
                    {
                        if (item.BrandID == lsprod[i].BrandID)
                        {
                            tempbrand = item.BrandName;
                        }
                    }
                    temp.Add(new ViewReport
                    {
                        BrandName = tempbrand,
                        Quantity = lsquantity.Sum(),
                        Total = lstotal.Sum()
                    });
                }
                var grtemp = temp.GroupBy(p => p.BrandName).ToList();
                for (int i = 0; i < grtemp.Count; i++)
                {
                    int grquantity = 0;
                    double grtotal = 0;
                    foreach (var item in temp)
                    {
                        if (item.BrandName == grtemp[i].Key)
                        {
                            grquantity += item.Quantity;
                            grtotal += item.Total;
                        }
                    }

                    dt.Rows.Add(grtemp[i].Key, grquantity, grtotal);
                }
            }
            else
            {
                return null;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report + day + "/" + month + ".xlsx");
                }
            }
        }
    }
}
