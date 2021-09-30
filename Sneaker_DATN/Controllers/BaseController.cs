using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
