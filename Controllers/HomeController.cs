using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TubelessServices.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string ApplicationID)
        {
            ViewBag.Title = "Home Page";


            if (ApplicationID != null)
            {
                ViewBag.ApplicationID = ApplicationID;
            }
            else
            {
                string siteID = ConfigurationManager.AppSettings["SiteID"];
                ViewBag.ApplicationID = siteID;
            }

            return View();
        }

        public ActionResult Details(string ApplicationID, string storeName)
        {

            ViewBag.ApplicationID = ApplicationID;
            ViewBag.storeName = storeName;


            bool isValid = true;
            var obj = new
            {
                valid = isValid
            };
            // return Json(obj);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
