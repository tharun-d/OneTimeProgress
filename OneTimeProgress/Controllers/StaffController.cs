using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneTimeProgress.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            if (userName == "abc" && password == "abc")
            {
                return RedirectToAction("FlightsPage");
            }
            return RedirectToAction("Login");
        }

        public ActionResult FlightsPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FlightsPage(string flightNumber)
        {
            return RedirectToAction("TaskDetails");
        }
        public ActionResult TaskDetails()
        {
            return View();
        }
    }
}