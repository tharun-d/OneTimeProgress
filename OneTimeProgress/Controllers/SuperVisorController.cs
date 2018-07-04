using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneTimeProgress.BussinessLayer;
using OneTimeProgress.BussinessEntity;

namespace OneTimeProgress.Controllers
{
    public class SuperVisorController : Controller
    {
        // GET: SuperVisor
        Bussines bussines = new Bussines();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FlightsPage()
        {
            string superVisorName = Session["SuperVisorName"].ToString();
            ViewBag.SuperVisorName = superVisorName;
            string superVisorDepartment = Session["SuperVisorDepartment"].ToString();
            ViewBag.Department = superVisorDepartment;
            List<FlightDetails> flightDetails = bussines.GetAllFlightDetails();
            ViewBag.FlightDetails = flightDetails;
            return View();
        }
        [HttpPost]
        public ActionResult FlightsPage(string flightNumber)
        {
            Session["SflightNumber"] = flightNumber;
            return RedirectToAction("ALLTaskDetailsTest");
        }
        //public ActionResult ALLTaskDetails()
        //{
        //    string superVisorName = Session["SuperVisorName"].ToString();
        //    string superVisorDepartment=Session["SuperVisorDepartment"].ToString();
        //    string flightNumber = Session["SflightNumber"].ToString();
        //    FlightDetails flightDetails = bussines.GetDetailsForOneFlight(flightNumber);
        //    ViewBag.FlightNumber = flightDetails.FlightNumber;
        //    ViewBag.DepartureTime = "17:00";
        //    ViewBag.CurrentStation = flightDetails.CurrentStation;
        //    List<ALLTaskLists> taskLists = bussines.GetStatusOfAllTasks(flightNumber,superVisorDepartment);
        //    ViewBag.TaskLists = taskLists;
        //    return View();
        //}
        public ActionResult ALLTaskDetailsTest()
        {
            string superVisorName = Session["SuperVisorName"].ToString();
            ViewBag.SuperVisorName = superVisorName;
            string superVisorDepartment = Session["SuperVisorDepartment"].ToString();
            ViewBag.Department = superVisorDepartment;
            string flightNumber = Session["SflightNumber"].ToString();
            FlightDetails flightDetails = bussines.GetDetailsForOneFlight(flightNumber);
            ViewBag.FlightNumber = flightDetails.FlightNumber;
            ViewBag.DepartureTime = flightDetails.Departure;
            ViewBag.CurrentStation = flightDetails.CurrentStation;
            List<ALLTaskLists> taskLists = bussines.GetStatusOfAllTasks(flightNumber,superVisorDepartment);
            ViewBag.TaskLists = taskLists;
            return View();
        }
    }
}