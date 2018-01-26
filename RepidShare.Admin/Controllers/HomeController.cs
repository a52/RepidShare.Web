using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepidShare.Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //EmailFormModel model = new EmailFormModel();
            //RepidShare.Utility.CommonUtils.Contact(model);



            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            UserLogin objUserLogin = RepidShare.Utility.CommonUtils.UserLogin;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// LogOff 
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            Request.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1d);
            Request.Cookies["ASP.NET_SessionId"].Value = "";
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            //Disable back button In all browsers.
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            Session.Abandon();
            Session.RemoveAll();

            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
            return RedirectToAction("Login", "User");

        }
    }
}
