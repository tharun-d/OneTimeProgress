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
            if (bussines.LoginValidator(loginModel))
            {
                return RedirectToAction("FlightsPage");
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
            return RedirectToAction("TaskDetails");
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
        public ActionResult TaskDetails(string task)
        {
            return View();
        }
        public string InsertTasks()
        {
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
                    SqlCommand cmd = new SqlCommand("InsertIntoTaskList @flightNumber,@taskDetail,@duration,@startTime", con);
                    cmd.Parameters.AddWithValue("@flightNumber", reader.GetDouble(0));
                    cmd.Parameters.AddWithValue("@taskDetail", reader.GetString(1));
                    cmd.Parameters.AddWithValue("@startTime", reader.GetDouble(2));
                    cmd.Parameters.AddWithValue("@duration", reader.GetDouble(3));
                    
                    j = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return "Inserted into Task List Table";
        }
    }
}