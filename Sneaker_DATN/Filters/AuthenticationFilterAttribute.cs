using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Sneaker_DATN.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Filters
{
    public class AuthenticationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Controller controller = filterContext.Controller as Controller;
            var session = filterContext.HttpContext.Session;
            string userName = filterContext.HttpContext.Session.GetString(SessionKey.User.UserName);
            var sessionStatus = ((userName != null && userName != "") ? true : false);
            if (controller != null)
            {
                if (session == null || !sessionStatus)
                {
                    filterContext.Result = 
                        new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                                { "controller", "Admin" },
                                { "action", "Login" }
                            });
                }
            }
            base.OnActionExecuting(filterContext);
        }

        public class AuthenticationFilterAttributeGuest : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filtercontext)
            {
                Controller controller = filtercontext.Controller as Controller;
                var session = filtercontext.HttpContext.Session;
                string EmailGuest = filtercontext.HttpContext.Session.GetString(SessionKey.Guest.Guest_Email);
                var sessionStatus = ((EmailGuest != null && EmailGuest != "") ? true : false);
                if (controller != null)
                {
                    if (session == null || !sessionStatus)
                    {
                        filtercontext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary
                                {
                                {"controller", "UserMem" },
                                {"action", "Login" }
                                });
                    }
                }
                base.OnActionExecuting(filtercontext);
            }
        }
    }
}
