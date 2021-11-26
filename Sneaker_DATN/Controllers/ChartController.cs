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
    }
}
