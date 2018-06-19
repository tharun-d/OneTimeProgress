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
            List<FlightDetails> flightDetails = bussines.GetAllFlightDetails();
            ViewBag.FlightDetails = flightDetails;
            return View();
        }
        [HttpPost]
        public ActionResult FlightsPage(string flightNumber)
        {
            Session["SflightNumber"] = flightNumber;
            return RedirectToAction("ALLTaskDetails");
        }
        public ActionResult ALLTaskDetails()
        {
            string flightNumber = Session["SflightNumber"].ToString();
            FlightDetails flightDetails = bussines.GetDetailsForOneFlight(flightNumber);
            ViewBag.FlightNumber = flightDetails.FlightNumber;
            ViewBag.DepartureTime = DateTime.Now.AddHours(2).ToShortTimeString();
            ViewBag.CurrentStation = flightDetails.CurrentStation;
            List<ALLTaskLists> taskLists = bussines.GetStatusOfAllTasks(flightNumber);
            ViewBag.TaskLists = taskLists;
            return View();
        }
    }
}