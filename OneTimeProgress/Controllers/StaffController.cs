using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using OneTimeProgress.BussinessEntity;
using OneTimeProgress.BussinessLayer;
using OneTimeProgress.DataAccessLayer;

namespace OneTimeProgress.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff

        Bussines bussines = new Bussines();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            ConfirmLoginModel confirmLoginModels = bussines.LoginValidator(loginModel);
            if (confirmLoginModels.userType == "Staff")
            {
                Session["StaffName"] = confirmLoginModels.userName;
                Session["StaffDepartment"] = confirmLoginModels.userDepartment;
                return RedirectToAction("TaskDetailsTest");
            }
            else if (confirmLoginModels.userType == "Super Visor")
            {
                Session["SuperVisorName"] = confirmLoginModels.userName;
                Session["SuperVisorDepartment"] = confirmLoginModels.userDepartment;
                return RedirectToAction("FlightsPage","SuperVisor");
            }
            else if (confirmLoginModels.userType == "Manager")
            {
                Session["ManagerName"] = confirmLoginModels.userName;
                return RedirectToAction("FlightsPage", "Manager");
            }
            else
            {
                ViewBag.Message = "Sorry we dont find you";
                return View();
            }
            
        }

        //public ActionResult FlightsPage()
        //{
        //    List<FlightDetails> flightDetails = bussines.GetAllFlightDetails();
        //    ViewBag.FlightDetails = flightDetails;
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult FlightsPage(string flightNumber)
        //{
        //    Session["flightNumber"] = flightNumber;
        //    return RedirectToAction("TaskDetailsTest");
        //}
        //public ActionResult TaskDetails()
        //{
        //    string flightNumber = Session["flightNumber"].ToString();
        //    FlightDetails flightDetails = bussines.GetDetailsForOneFlight(flightNumber);
        //    ViewBag.FlightNumber = flightDetails.FlightNumber;
        //    ViewBag.Bay = flightDetails.Bay;
        //    ViewBag.CurrentStation = flightDetails.CurrentStation;
        //    List<TaskLists> taskLists = bussines.GetTasksForParticularFlight(flightNumber);
        //    ViewBag.TaskLists = taskLists;
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult TaskDetails(string Start,string Complete)
        //{
        //    DateTime sheduledStartTime;
        //    int timeDifference;
        //    string flightNumber = Session["flightNumber"].ToString();
        //    if (string.IsNullOrEmpty(Start))
        //    {
        //        sheduledStartTime = bussines.GettingEndTime(flightNumber, Complete);
        //        timeDifference = (DateTime.Now - sheduledStartTime).Minutes;
        //        bussines.UpdateTaskEndStatus(flightNumber, Complete,"Completed", DateTime.Now,timeDifference);
        //        return RedirectToAction("TaskDetails");
        //    }
        //    else
        //    {
        //        bussines.UpdateTaskStartStatus(flightNumber, Start, "In Progress", DateTime.Now);
        //        return RedirectToAction("TaskDetails");
        //    }
        //}
        public ActionResult TaskDetailsTest()
        {
            string staffName = Session["StaffName"].ToString();
            //if (staffName== "RAMST2")
            //{
            //    Session["flightNumber"] = "343";
            //}
            //else
            //    Session["flightNumber"] = "121";
            Session["flightNumber"] = bussines.GettingFlightNumber(staffName);
            string flightNumber = Session["flightNumber"].ToString();           
            string staffDepartment = Session["StaffDepartment"].ToString();
            ViewBag.StaffName = staffName;
            ViewBag.staffDepartment = staffDepartment;
            FlightDetails flightDetails = bussines.GetDetailsForOneFlight(flightNumber);
            ViewBag.FlightNumber = flightDetails.FlightNumber;
            ViewBag.Bay = flightDetails.Bay;
            ViewBag.CurrentStation = flightDetails.CurrentStation;
            ViewBag.Departure = flightDetails.Departure;
            List<TaskLists> taskLists = bussines.GetTasksForParticularFlight(flightNumber,staffName,staffDepartment);
            ViewBag.TaskLists = taskLists;
            return View();
        }
        [HttpPost]
        public ActionResult TaskDetailsTest(string Start, string Complete)
        {
            DateTime ActualStartTime;
            double timeDifference;
            string flightNumber = Session["flightNumber"].ToString();
            string staffDepartment = Session["StaffDepartment"].ToString();
            if (string.IsNullOrEmpty(Start))
            {
                
                ActualStartTime = bussines.GettingActualStartTime(flightNumber, Complete);
                timeDifference = (DateTime.Now.Subtract(ActualStartTime)).TotalMinutes;
                bussines.UpdateTaskEndStatus(flightNumber, Complete, "Completed", DateTime.Now, timeDifference);
                if (bussines.InProgressOrYetToStartTasksForDepartment(flightNumber, staffDepartment))
                {
                    bussines.UpdateStatusInDepartmentsCompleted(flightNumber, staffDepartment,DateTime.Now);
                }
                return RedirectToAction("TaskDetailsTest");
            }
            else
            {
                if(bussines.InProgressTasksForDepartment(flightNumber,staffDepartment))
                    bussines.UpdateStatusInDepartments(flightNumber, staffDepartment,DateTime.Now);
                bussines.UpdateTaskStartStatus(flightNumber, Start, "In Progress", DateTime.Now);
                return RedirectToAction("TaskDetailsTest");
            }
        }
        public ActionResult TaskDetailsTest1()
        {
            string staffName = Session["StaffName"].ToString();
            if (staffName == "RAMST2")
            {
                Session["flightNumber"] = "343";
            }
            else
                Session["flightNumber"] = "121";
            string flightNumber = Session["flightNumber"].ToString();
            string staffDepartment = Session["StaffDepartment"].ToString();
            ViewBag.StaffName = staffName;
            ViewBag.staffDepartment = staffDepartment;
            FlightDetails flightDetails = bussines.GetDetailsForOneFlight(flightNumber);
            ViewBag.FlightNumber = flightDetails.FlightNumber;
            ViewBag.Bay = flightDetails.Bay;
            ViewBag.CurrentStation = flightDetails.CurrentStation;
            ViewBag.Departure = flightDetails.Departure;
            List<TaskLists> taskLists = bussines.GetTasksForParticularFlight(flightNumber, staffName, staffDepartment);
            ViewBag.TaskLists = taskLists;
            return View();
        }
        [HttpPost]
        public ActionResult TaskDetailsTest1(string Start, string Complete)
        {
            DateTime ActualStartTime;
            double timeDifference;
            string flightNumber = Session["flightNumber"].ToString();
            string staffDepartment = Session["StaffDepartment"].ToString();
            if (string.IsNullOrEmpty(Start))
            {

                ActualStartTime = bussines.GettingActualStartTime(flightNumber, Complete);
                timeDifference = (DateTime.Now.Subtract(ActualStartTime)).TotalMinutes;
                bussines.UpdateTaskEndStatus(flightNumber, Complete, "Completed", DateTime.Now, timeDifference);
                if (bussines.InProgressOrYetToStartTasksForDepartment(flightNumber, staffDepartment))
                {
                    bussines.UpdateStatusInDepartmentsCompleted(flightNumber, staffDepartment, DateTime.Now);
                }
                return RedirectToAction("TaskDetailsTest");
            }
            else
            {
                if (bussines.InProgressTasksForDepartment(flightNumber, staffDepartment))
                    bussines.UpdateStatusInDepartments(flightNumber, staffDepartment, DateTime.Now);
                bussines.UpdateTaskStartStatus(flightNumber, Start, "In Progress", DateTime.Now);
                return RedirectToAction("TaskDetailsTest");
            }
        }

    }
}