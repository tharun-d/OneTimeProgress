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
        PostgressDataAccess PostgressDataAccess = new PostgressDataAccess();
        public ActionResult Index()
        {
           // PostgressDataAccess.Test();
            return View();
        }
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
            if (staffName== "RAMST2")
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
            if (string.IsNullOrEmpty(Start))
            {
                ActualStartTime = bussines.GettingActualStartTime(flightNumber, Complete);
                timeDifference = (DateTime.Now.Subtract(ActualStartTime)).TotalMinutes;
                bussines.UpdateTaskEndStatus(flightNumber, Complete, "Completed", DateTime.Now, timeDifference);
                return RedirectToAction("TaskDetailsTest");
            }
            else
            {
                bussines.UpdateTaskStartStatus(flightNumber, Start, "In Progress", DateTime.Now);
                return RedirectToAction("TaskDetailsTest");
            }
        }
        //public string InsertTasksandDepartMents()
        //{
        //    DateTime flightdeparture = (DateTime.Now.AddHours(2));
        //    DateTime startTime;
        //    DateTime endTime;
        //    DateTime departmentStartTime;
        //    DateTime departmentEndTime;
        //    double departmentSheduledMinutes;
        //    string _path = @"C:\Users\hpadmin\Desktop\Standard\AllTaskDetails.xlsx";
        //    FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
        //    SqlConnection con = new SqlConnection("Server=HIB30BWAX2; Initial Catalog = OneTimeProgress; User ID = sa; Password = Passw0rd@12;");
        //    var reader = ExcelReaderFactory.CreateReader(stream);
        //    if (true)
        //    {
        //        int j;
        //        reader.Read();
        //        con.Open();
        //        while (reader.Read())
        //        {
                    
        //            SqlCommand cmd = new SqlCommand("InsertIntoTaskList @flightNumber,@taskDetail,@duration,@startTime,@endTime,@statusOfTask,@actualStartTime,@actualEndTime,@timeDifference,@department,@staffName", con);
        //            cmd.Parameters.AddWithValue("@flightNumber", reader.GetDouble(0));
        //            cmd.Parameters.AddWithValue("@taskDetail", reader.GetString(1));
        //            startTime = (flightdeparture.AddMinutes(-reader.GetDouble(2)));
        //            cmd.Parameters.AddWithValue("@startTime", startTime);
        //            cmd.Parameters.AddWithValue("@duration", reader.GetDouble(3));
        //            endTime = startTime.AddMinutes(reader.GetDouble(3));
        //            cmd.Parameters.AddWithValue("@endTime", endTime);
        //            cmd.Parameters.AddWithValue("@statusOfTask", "Yet To Start");
        //            cmd.Parameters.AddWithValue("@actualStartTime", DateTime.Now);
        //            cmd.Parameters.AddWithValue("@actualEndTime", DateTime.Now);
        //            cmd.Parameters.AddWithValue("@timeDifference", 0);
        //            cmd.Parameters.AddWithValue("@department", reader.GetString(4));
        //            cmd.Parameters.AddWithValue("@staffName", reader.GetString(5));
        //            j = cmd.ExecuteNonQuery();
        //        }
        //        con.Close();
        //    }
        //    reader.NextResult();
        //    if(true)
        //    {
        //        int j;
        //        reader.Read();
        //        con.Open();
        //        while (reader.Read())
        //        {
        //            departmentStartTime = flightdeparture.AddMinutes(-reader.GetDouble(3));
        //            departmentEndTime = flightdeparture.AddMinutes(-reader.GetDouble(4));
        //            departmentSheduledMinutes = departmentEndTime.Subtract(departmentStartTime).TotalMinutes;
        //            SqlCommand cmd = new SqlCommand("InsertIntoDepartments @flightNumber,@departmentName,@superVisorName,@sheduledStartTime,@sheduledEndTime,@sheduledDuration,@actualStartTime,@actualEndTime,@statusOfDepartment", con);
        //            cmd.Parameters.AddWithValue("@flightNumber", reader.GetDouble(0));
        //            cmd.Parameters.AddWithValue("@departmentName", reader.GetString(1));
        //            cmd.Parameters.AddWithValue("@superVisorName", reader.GetString(2));
        //            cmd.Parameters.AddWithValue("@sheduledStartTime", departmentStartTime);
        //            cmd.Parameters.AddWithValue("@sheduledEndTime", departmentEndTime);
        //            cmd.Parameters.AddWithValue("@sheduledDuration", departmentSheduledMinutes);
        //            cmd.Parameters.AddWithValue("@actualStartTime", DateTime.Now);
        //            cmd.Parameters.AddWithValue("@actualEndTime", DateTime.Now);
        //            cmd.Parameters.AddWithValue("@statusOfDepartment", "Yet To Start");
        //            j = cmd.ExecuteNonQuery();
        //        }
        //        con.Close();
        //    }
        //    return "Inserted into Task List and Department Table";
        //}
        //public string Insert()
        //{
        //    DateTime flightdeparture = (DateTime.Now.AddHours(2));
        //    DateTime startTime,endTime;
        //    string sd,ed;
        //    sd = DateTime.Now.ToString("MM/dd/yyyy");
        //    ed = DateTime.Now.ToString("MM/dd/yyyy");
        //    string st,et;
        //    string start,end;
        //    string _path = @"C:\Users\hpadmin\Desktop\Standard\modified.xlsx";
        //    FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
        //    SqlConnection con = new SqlConnection("Server=HIB30BWAX2; Initial Catalog = OneTimeProgress; User ID = sa; Password = Passw0rd@12;");
        //    var reader = ExcelReaderFactory.CreateReader(stream);
        //    if (true)
        //    {
        //        int j;
        //        reader.Read();
        //        con.Open();
        //        while (reader.Read())
        //        {
        //            SqlCommand cmd = new SqlCommand("InsertIntoTaskList @flightNumber,@taskDetail,@duration,@startTime,@endTime,@statusOfTask,@actualStartTime,@actualEndTime,@timeDifference,@department,@staffName", con);
        //            cmd.Parameters.AddWithValue("@flightNumber", reader.GetDouble(0));
        //            cmd.Parameters.AddWithValue("@taskDetail", reader.GetString(1));
        //            cmd.Parameters.AddWithValue("@duration", reader.GetDouble(2));
        //            st = reader.GetDateTime(3).ToString("hh:mm");
        //            start = sd +" "+ st;
        //            startTime = Convert.ToDateTime(start);
        //            cmd.Parameters.AddWithValue("@startTime", startTime);
        //            et = reader.GetDateTime(4).ToString("hh:mm");
        //            end = ed + " " + et;
        //            endTime = Convert.ToDateTime(end);
        //            cmd.Parameters.AddWithValue("@endTime", endTime);
        //            cmd.Parameters.AddWithValue("@statusOfTask", reader.GetString(5));
        //            cmd.Parameters.AddWithValue("@actualStartTime", reader.GetDateTime(6));
        //            cmd.Parameters.AddWithValue("@actualEndTime", reader.GetDateTime(7));
        //            cmd.Parameters.AddWithValue("@timeDifference", reader.GetDouble(8));
        //            cmd.Parameters.AddWithValue("@department", reader.GetString(9));
        //            cmd.Parameters.AddWithValue("@staffName", reader.GetString(10));
        //            j = cmd.ExecuteNonQuery();
        //        }
        //        con.Close();
        //    }
            
        //    return "Inserted into Task List and Department Table";
        //}
        public string FinalInsert()
        {
            DateTime flightdeparture = (DateTime.Now.AddHours(2));
            DateTime startTime, endTime,aStartTime,aEndTime;
            string sd, ed,asd,aed;
            sd = DateTime.Now.ToString("MM/dd/yyyy");
            ed = DateTime.Now.ToString("MM/dd/yyyy");
            asd = DateTime.Now.ToString("MM/dd/yyyy");
            aed = DateTime.Now.ToString("MM/dd/yyyy");
            string st, et,ast,aet;
            string start, end,aStart,aEnd;
            string _path = @"C:\Users\hpadmin\Desktop\Standard\modified.xlsx";
            FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            SqlConnection con = new SqlConnection("Server=HIB30BWAX2; Initial Catalog = OneTimeProgress; User ID = sa; Password = Passw0rd@12;");
            var reader = ExcelReaderFactory.CreateReader(stream);
            if (reader.Name=="Tasks")//Tasks Page
            {
                int j;
                reader.Read();
                con.Open();
                while (reader.Read())
                {
                    SqlCommand cmd = new SqlCommand("InsertIntoTaskList @flightNumber,@taskDetail,@duration,@startTime,@endTime,@statusOfTask,@actualStartTime,@actualEndTime,@timeDifference,@department,@staffName", con);
                    cmd.Parameters.AddWithValue("@flightNumber", reader.GetDouble(0));
                    cmd.Parameters.AddWithValue("@taskDetail", reader.GetString(1));
                    cmd.Parameters.AddWithValue("@duration", reader.GetDouble(2));
                    st = reader.GetDateTime(3).ToString("hh:mm");
                    start = sd + " " + st;
                    startTime = Convert.ToDateTime(start);
                    cmd.Parameters.AddWithValue("@startTime", startTime);
                    et = reader.GetDateTime(4).ToString("hh:mm");
                    end = ed + " " + et;
                    endTime = Convert.ToDateTime(end);
                    cmd.Parameters.AddWithValue("@endTime", endTime);
                    cmd.Parameters.AddWithValue("@statusOfTask", reader.GetString(5));
                    ast = reader.GetDateTime(6).ToString("hh:mm");
                    aStart = asd + " " + ast;
                    aStartTime = Convert.ToDateTime(aStart);
                    cmd.Parameters.AddWithValue("@actualStartTime", aStartTime);
                    aet = reader.GetDateTime(7).ToString("hh:mm");
                    aEnd = aed + " " + aet;
                    aEndTime = Convert.ToDateTime(end);
                    cmd.Parameters.AddWithValue("@actualEndTime",aEndTime);
                    cmd.Parameters.AddWithValue("@timeDifference", reader.GetDouble(8));
                    cmd.Parameters.AddWithValue("@department", reader.GetString(9));
                    cmd.Parameters.AddWithValue("@staffName", reader.GetString(10));
                    j = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            reader.NextResult();
            if (reader.Name=="Flights")//Flights sheet
            {
                DateTime taskStartTime, departureTime;
                string ts, dt;
                ts = DateTime.Now.ToString("MM/dd/yyyy");
                dt = DateTime.Now.ToString("MM/dd/yyyy");
                string tst, tdt;
                string tstartTime, tdepTime;
                int j;
                reader.Read();
                con.Open();
                while (reader.Read())
                {
                    
                    SqlCommand cmd = new SqlCommand("InsertIntoAllFlightDetails @equipmentName,@flightNumber,@airCraftModel,@currentStation,@bayNumber,@taskStartTime,@departureTime", con);
                    cmd.Parameters.AddWithValue("@equipmentName", reader.GetString(0));
                    cmd.Parameters.AddWithValue("@flightNumber", reader.GetDouble(1));
                    cmd.Parameters.AddWithValue("@airCraftModel", reader.GetString(2));
                    cmd.Parameters.AddWithValue("@currentStation", reader.GetString(3));
                    cmd.Parameters.AddWithValue("@bayNumber", reader.GetDouble(4));
                    tst = reader.GetDateTime(5).ToString("hh:mm");
                    tstartTime = ts + " " + tst;
                    taskStartTime = Convert.ToDateTime(tstartTime);
                    cmd.Parameters.AddWithValue("@taskStartTime", taskStartTime);
                    tdt = reader.GetDateTime(6).ToString("hh:mm");
                    tdepTime = dt + " " + tdt;
                    departureTime = Convert.ToDateTime(tdepTime);
                    cmd.Parameters.AddWithValue("@departureTime", departureTime);
                    j = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            reader.NextResult();
            if (reader.Name == "Employees")//Employees sheet
            { 
                int j;
                reader.Read();
                con.Open();
                while (reader.Read())
                {
                    SqlCommand cmd = new SqlCommand("InsertIntoLoginDetails @email,@secretPassword,@userName,@userType,@UserDepartment", con);
                    cmd.Parameters.AddWithValue("@email", reader.GetString(0));
                    cmd.Parameters.AddWithValue("@secretPassword", reader.GetString(1));
                    cmd.Parameters.AddWithValue("@userName", reader.GetString(2));
                    cmd.Parameters.AddWithValue("@userType", reader.GetString(3));
                    cmd.Parameters.AddWithValue("@UserDepartment", reader.GetString(4));
                  
                    j = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return "Inserted";
        }
    }
}