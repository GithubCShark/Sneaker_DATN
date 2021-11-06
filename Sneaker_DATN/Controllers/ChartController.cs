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
        public IActionResult GetAllDateUserMem()
        {
            List<Users> list = new List<Users>();
            list = _context.Users.Where(p => p.RoleID.Equals(3)).ToList();
            var listcount = from p in list
                            where p.DateCreated.Value.Year == 2021
                            group p by p.DateCreated.Value.Month into List
                            select  new
                            {
                                Thang = List.Key,
                                SoLuong = List.Count()
                            };
            List<ViewChar> dataChart = new List<ViewChar>();
            foreach (var item in listcount)
            {
                dataChart.Add(new ViewChar
                {
                    Thang = item.Thang,
                    SoLuong = item.SoLuong
                });
            }
            //return View(dataChart);
            return View(JsonConvert.SerializeObject(dataChart));
            //return Json(listcount, JsonRequestBehavior.AllowGet);
        }
    }
}
