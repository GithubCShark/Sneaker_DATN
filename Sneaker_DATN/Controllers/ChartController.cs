using Microsoft.AspNetCore.Http;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Sneaker_DATN.Models;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;

namespace Sneaker_DATN.Controllers
{
    public class ChartController : BaseController
    {
        private DataContext _context;
        public ChartController(DataContext dataContext)
        {
            _context = dataContext;
        }
        public ActionResult Index()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public Microsoft.AspNetCore.Mvc.JsonResult GetAllDateUserMem()
        {
            List<Users> list = new List<Users>();
            list = _context.Users.Where(p => p.RoleID.Equals(3)).ToList();
            var listcount = from p in list
                            where p.DateCreated.Value.Year == 2021
                            group p by p.DateCreated.Value.Month into List
                            select new
                            {
                                Thang = List.Key,
                                SoLuong = List.Count()
                            };
            return Json(listcount, JsonRequestBehavior.AllowGet);
        }
    }
}
