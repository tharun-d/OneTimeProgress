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

namespace OneTimeProgress.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff

        Bussines bussines = new Bussines();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (bussines.LoginValidator(loginModel)=="Staff")
            {
                Session["StaffName"] = loginModel.userName;
                return RedirectToAction("FlightsPage");
            }
            if (bussines.LoginValidator(loginModel) == "Super Visor")
            {
                Session["SuperVisorName"] = loginModel.userName;
                return RedirectToAction("FlightsPage","SuperVisor");
            }

            ViewBag.Message = "Sorry we dont find you";
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
            Session["flightNumber"] = flightNumber;
            return RedirectToAction("TaskDetailsTest");
        }
        public ActionResult TaskDetails()
        {
            string flightNumber = Session["flightNumber"].ToString();
            FlightDetails flightDetails = bussines.GetDetailsForOneFlight(flightNumber);
            ViewBag.FlightNumber = flightDetails.FlightNumber;
            ViewBag.Bay = flightDetails.Bay;
            ViewBag.CurrentStation = flightDetails.CurrentStation;
            List<TaskLists> taskLists = bussines.GetTasksForParticularFlight(flightNumber);
            ViewBag.TaskLists = taskLists;
            return View();
        }
        [HttpPost]
        public ActionResult TaskDetails(string Start,string Complete)
        {
            DateTime sheduledStartTime;
            int timeDifference;
            string flightNumber = Session["flightNumber"].ToString();
            if (string.IsNullOrEmpty(Start))
            {
                sheduledStartTime = bussines.GettingEndTime(flightNumber, Complete);
                timeDifference = (DateTime.Now - sheduledStartTime).Minutes;
                bussines.UpdateTaskEndStatus(flightNumber, Complete,"Completed", DateTime.Now,timeDifference);
                return RedirectToAction("TaskDetails");
            }
            else
            {
                bussines.UpdateTaskStartStatus(flightNumber, Start, "In Progress", DateTime.Now);
                return RedirectToAction("TaskDetails");
            }
        }
        public ActionResult TaskDetailsTest()
        {
            string flightNumber = Session["flightNumber"].ToString();
            FlightDetails flightDetails = bussines.GetDetailsForOneFlight(flightNumber);
            ViewBag.FlightNumber = flightDetails.FlightNumber;
            ViewBag.Bay = flightDetails.Bay;
            ViewBag.ProgressBar = 25.4855;
            ViewBag.CurrentStation = flightDetails.CurrentStation;
            List<TaskLists> taskLists = bussines.GetTasksForParticularFlight(flightNumber);
            ViewBag.TaskLists = taskLists;
            return View();
        }
        [HttpPost]
        public ActionResult TaskDetailsTest(string Start, string Complete)
        {
            DateTime sheduledStartTime;
            int timeDifference;
            string flightNumber = Session["flightNumber"].ToString();
            if (string.IsNullOrEmpty(Start))
            {
                sheduledStartTime = bussines.GettingEndTime(flightNumber, Complete);
                timeDifference = (DateTime.Now - sheduledStartTime).Minutes;
                bussines.UpdateTaskEndStatus(flightNumber, Complete, "Completed", DateTime.Now, timeDifference);
                return RedirectToAction("TaskDetailsTest");
            }
            else
            {
                bussines.UpdateTaskStartStatus(flightNumber, Start, "In Progress", DateTime.Now);
                return RedirectToAction("TaskDetailsTest");
            }
        }
        public string InsertTasks()
        {
            DateTime flightdeparture = (DateTime.Now.AddHours(2));
            DateTime startTime;
            DateTime endTime;
            string _path = @"C:\Users\hpadmin\Desktop\Standard\TaskDetails.xlsx";
            FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            SqlConnection con = new SqlConnection("Server=HIB30BWAX2; Initial Catalog = OneTimeProgress; User ID = sa; Password = Passw0rd@12;");
            var reader = ExcelReaderFactory.CreateReader(stream);
            if (true)
            {
                int j;
                reader.Read();
                con.Open();
                while (reader.Read())
                {
                    SqlCommand cmd = new SqlCommand("InsertIntoTaskList @flightNumber,@taskDetail,@duration,@startTime,@endTime,@statusOfTask,@actualStartTime,@actualEndTime,@timeDifference", con);
                    cmd.Parameters.AddWithValue("@flightNumber", reader.GetDouble(0));
                    cmd.Parameters.AddWithValue("@taskDetail", reader.GetString(1));
                    startTime = (flightdeparture.AddMinutes(-reader.GetDouble(2)));
                    cmd.Parameters.AddWithValue("@startTime", startTime);
                    cmd.Parameters.AddWithValue("@duration", reader.GetDouble(3));
                    endTime = startTime.AddMinutes(reader.GetDouble(3));
                    cmd.Parameters.AddWithValue("@endTime", endTime);
                    cmd.Parameters.AddWithValue("@statusOfTask", "Yet To Start");
                    cmd.Parameters.AddWithValue("@actualStartTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@actualEndTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@timeDifference", 0);
                    j = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return "Inserted into Task List Table";
        }
    }
}