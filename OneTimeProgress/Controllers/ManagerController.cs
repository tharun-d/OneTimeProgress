using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneTimeProgress.BussinessEntity;
using OneTimeProgress.BussinessLayer;

namespace OneTimeProgress.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        Bussines bussines = new Bussines();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DepartmentStatus()
        {
            Session["flightNumber"] = "1001";
            string flightNumber = Session["flightNumber"].ToString();
            FlightDetails flightDetails = bussines.GetDetailsForOneFlight(flightNumber);
            ViewBag.FlightNumber = flightDetails.FlightNumber;
            ViewBag.Bay = flightDetails.Bay;
            ViewBag.CurrentStation = flightDetails.CurrentStation;
            ViewBag.Departments = bussines.GetAllDepartmentsStatus(flightNumber);
            return View();
        }
    }
}