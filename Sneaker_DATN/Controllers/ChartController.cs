using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sneaker_DATN.Models;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sneaker_DATN.Controllers
{
    public class ChartController : BaseController
    {
        private DataContext _context;
        public ChartController(DataContext dataContext)
        {
            _context = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChartWeek()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetAllDateUserMem(int year)
        {
            var lsmember = from p in _context.Users.Where(p => p.RoleID.Equals(3)).ToList()
                            where p.DateCreated.Value.Year == year
                           orderby p.DateCreated
                           group p by p.DateCreated.Value.Month into List
                            select  new
                            {
                                Thang = List.Key.ToString(),
                                SoLuong = List.Count()
                            };
            var lsorder = from p in _context.Orders.Where(p => p.Status == "Đã nhận").ToList()
                          where p.DateCreate.Year == year
                          orderby p.DateCreate
                          group p by p.DateCreate.Month into List
                          select new
                          {
                              Thang = List.Key.ToString(),
                              SoLuong = List.Count()
                          };
            var lstotal = from p in _context.Orders.Where(p => p.Status == "Đã nhận").ToList()
                          where p.DateCreate.Year == year
                          orderby p.DateCreate
                          group p by p.DateCreate.Month into List
                          select List;
            List<ViewChar> dataChart = new List<ViewChar>();
            for (int i = 0; i < 12; i++)
            {
                try
                {
                    dataChart.Add(new ViewChar
                    {
                        ThangOrder = lsorder.ToList()[i].Thang,
                        ThangMem = lsmember.ToList()[i].Thang,
                        SoLuongOrder = lsorder.ToList()[i].SoLuong,
                        SoLuongMem = lsmember.ToList()[i].SoLuong,
                        Total = lstotal.ToList()[i].Sum(x => x.Total)
                    });
                }
                catch (Exception)
                {
                    dataChart.Add(new ViewChar
                    {
                        ThangOrder = "0",
                        ThangMem = "0",
                        SoLuongOrder = 0,
                        SoLuongMem = 0,
                        Total = 0
                    });
                }
            }
            ViewData["DataList"] = dataChart.ToList();
            return Json(JsonConvert.SerializeObject(dataChart));
        }

        [HttpPost]
        public IActionResult GetChartMonth(int month)
        {
            var dt = DateTime.Now.Year;
            var lsorder = from p in _context.Orders.ToList()
                          where p.DateCreate.Month == month && p.DateCreate.Year == dt
                          orderby p.DateCreate
                          select p;

            var lsDTT = from p in lsorder.Where(p => p.Status == "Đã nhận").ToList()
                        select p;
            var lsCTT = from p in lsorder.Where(p => p.Status != "Đã nhận" && p.Status != "Đã hủy").ToList()
                        select p;

            var lsname = lsorder.GroupBy(x => x.FullName).ToList();
            List<ViewChar> dataChart = new List<ViewChar>();
            //Theo tên khách
            for (int i = 0; i < lsname.Count; i++)
            {
                double total = 0;
                double totalCTT = 0;
                foreach (var x in lsDTT)
                {
                    if (lsname.ToList()[i].Key == x.FullName)
                    {
                        total += x.Total;
                    }
                }
                foreach (var y in lsCTT)
                {
                    if (lsname.ToList()[i].Key == y.FullName)
                    {
                        totalCTT += y.Total;
                    }
                }
                dataChart.Add(new ViewChar
                {
                    Name = lsname[i].Key,
                    Total = total,
                    TotalCTT = totalCTT
                });
            }

            ViewData["DataList"] = dataChart.ToList();
            return Json(JsonConvert.SerializeObject(dataChart));
        }
    }
}
