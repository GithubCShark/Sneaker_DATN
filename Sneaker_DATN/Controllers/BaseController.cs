using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sneaker_DATN.Constant;
using Sneaker_DATN.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    [AuthenticationFilter]
    public class BaseController : Controller
    {
        public BaseController() { }

        protected string GetUserName()
        {
            return HttpContext.Session.GetString(SessionKey.User.UserName);
        }

        protected string GetFullName()
        {
            return HttpContext.Session.GetString(SessionKey.User.FullName);
        }

        protected string GetEmailMem()
        {
            return HttpContext.Session.GetString(SessionKey.Guest.Guest_Email);
        }
    }
}
