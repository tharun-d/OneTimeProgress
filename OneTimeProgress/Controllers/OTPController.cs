using ExcelDataReader;
using OneTimeProgress.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneTimeProgress.BussinessLayer;

namespace OneTimeProgress.Controllers
{
    public class OTPController : Controller
    {
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
                return RedirectToAction("TaskDetailsTest","Staff");
            }
            else if (confirmLoginModels.userType == "Super Visor")
            {
                Session["SuperVisorName"] = confirmLoginModels.userName;
                Session["SuperVisorDepartment"] = confirmLoginModels.userDepartment;
                return RedirectToAction("FlightsPage", "SuperVisor");
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
        public ActionResult UploadData()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadData(HttpPostedFileBase ExcelFile)
        {
            bool ErrorOccured = false;
            string path = null;
            try
            {
                if (ExcelFile.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(ExcelFile.FileName);
                    path = Path.Combine(Server.MapPath("~/UploadFiles/"), _FileName);
                    ExcelFile.SaveAs(path);
                }
                ViewBag.Message = "File uploaded successfully";
            }
            catch (Exception ex)
            {
                ErrorOccured = true;
                ViewBag.Message = ex.Message;
                return View();
            }
            finally
            {
                if (ErrorOccured == false && path != null)
                {
                    DeleteData();
                    InsertData(path);
                }
            }
            return View();
        }
        //public string FinalInsert()
        //{
        //    DateTime flightdeparture = (DateTime.Now.AddHours(2));
        //    DateTime startTime, endTime, aStartTime, aEndTime;
        //    string sd, ed, asd, aed;
        //    sd = DateTime.Now.ToString("MM/dd/yyyy");
        //    ed = DateTime.Now.ToString("MM/dd/yyyy");
        //    asd = DateTime.Now.ToString("MM/dd/yyyy");
        //    aed = DateTime.Now.ToString("MM/dd/yyyy");
        //    string st, et, ast, aet;
        //    string start, end, aStart, aEnd;
        //    string _path = @"C:\Users\hpadmin\Desktop\Standard\modified.xlsx";
        //    FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
        //    SqlConnection con = new SqlConnection("Server=HIB30BWAX2; Initial Catalog = OneTimeProgress; User ID = sa; Password = Passw0rd@12;");
        //    var reader = ExcelReaderFactory.CreateReader(stream);
        //    if (reader.Name == "Tasks")//Tasks Page
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
        //            start = sd + " " + st;
        //            startTime = Convert.ToDateTime(start);
        //            cmd.Parameters.AddWithValue("@startTime", startTime);
        //            et = reader.GetDateTime(4).ToString("hh:mm");
        //            end = ed + " " + et;
        //            endTime = Convert.ToDateTime(end);
        //            cmd.Parameters.AddWithValue("@endTime", endTime);
        //            cmd.Parameters.AddWithValue("@statusOfTask", reader.GetString(5));
        //            ast = reader.GetDateTime(6).ToString("hh:mm");
        //            aStart = asd + " " + ast;
        //            aStartTime = Convert.ToDateTime(aStart);
        //            cmd.Parameters.AddWithValue("@actualStartTime", aStartTime);
        //            aet = reader.GetDateTime(7).ToString("hh:mm");
        //            aEnd = aed + " " + aet;
        //            aEndTime = Convert.ToDateTime(end);
        //            cmd.Parameters.AddWithValue("@actualEndTime", aEndTime);
        //            cmd.Parameters.AddWithValue("@timeDifference", reader.GetDouble(8));
        //            cmd.Parameters.AddWithValue("@department", reader.GetString(9));
        //            cmd.Parameters.AddWithValue("@staffName", reader.GetString(10));
        //            j = cmd.ExecuteNonQuery();
        //        }
        //        con.Close();
        //    }
        //    reader.NextResult();
        //    if (reader.Name == "Flights")//Flights sheet
        //    {
        //        DateTime taskStartTime, departureTime;
        //        string ts, dt;
        //        ts = DateTime.Now.ToString("MM/dd/yyyy");
        //        dt = DateTime.Now.ToString("MM/dd/yyyy");
        //        string tst, tdt;
        //        string tstartTime, tdepTime;
        //        int j;
        //        reader.Read();
        //        con.Open();
        //        while (reader.Read())
        //        {

        //            SqlCommand cmd = new SqlCommand("InsertIntoAllFlightDetails @equipmentName,@flightNumber,@airCraftModel,@currentStation,@bayNumber,@taskStartTime,@departureTime", con);
        //            cmd.Parameters.AddWithValue("@equipmentName", reader.GetString(0));
        //            cmd.Parameters.AddWithValue("@flightNumber", reader.GetDouble(1));
        //            cmd.Parameters.AddWithValue("@airCraftModel", reader.GetString(2));
        //            cmd.Parameters.AddWithValue("@currentStation", reader.GetString(3));
        //            cmd.Parameters.AddWithValue("@bayNumber", reader.GetDouble(4));
        //            tst = reader.GetDateTime(5).ToString("hh:mm");
        //            tstartTime = ts + " " + tst;
        //            taskStartTime = Convert.ToDateTime(tstartTime);
        //            cmd.Parameters.AddWithValue("@taskStartTime", taskStartTime);
        //            tdt = reader.GetDateTime(6).ToString("hh:mm");
        //            tdepTime = dt + " " + tdt;
        //            departureTime = Convert.ToDateTime(tdepTime);
        //            cmd.Parameters.AddWithValue("@departureTime", departureTime);
        //            j = cmd.ExecuteNonQuery();
        //        }
        //        con.Close();
        //    }
        //    reader.NextResult();
        //    if (reader.Name == "Employees")//Employees sheet
        //    {
        //        int j;
        //        reader.Read();
        //        con.Open();
        //        while (reader.Read())
        //        {
        //            SqlCommand cmd = new SqlCommand("InsertIntoLoginDetails @email,@secretPassword,@userName,@userType,@UserDepartment", con);
        //            cmd.Parameters.AddWithValue("@email", reader.GetString(0));
        //            cmd.Parameters.AddWithValue("@secretPassword", reader.GetString(1));
        //            cmd.Parameters.AddWithValue("@userName", reader.GetString(2));
        //            cmd.Parameters.AddWithValue("@userType", reader.GetString(3));
        //            cmd.Parameters.AddWithValue("@UserDepartment", reader.GetString(4));

        //            j = cmd.ExecuteNonQuery();
        //        }
        //        con.Close();
        //    }
        //    reader.NextResult();
        //    if (reader.Name == "Departments")//For airport manager view
        //    {
        //        DateTime dSheduledStartTime, dSheduledEndTime, dActualStartTime, dActualEndTime;
        //        string currentDate;
        //        currentDate = DateTime.Now.ToString("MM/dd/yyyy");
        //        string tst, tdt;
        //        string dsstartTime, dsendTime;
        //        int j;
        //        reader.Read();
        //        con.Open();
        //        while (reader.Read())
        //        {
        //            SqlCommand cmd = new SqlCommand("InsertIntoDepartments @flightNumber,@departmentName,@superVisorName,@sheduledStartTime,@sheduledEndTime,@sheduledDuration,@actualStartTime,@actualEndTime,@statusOfDepartment", con);
        //            cmd.Parameters.AddWithValue("@flightNumber", reader.GetDouble(0));
        //            cmd.Parameters.AddWithValue("@departmentName", reader.GetString(1));
        //            cmd.Parameters.AddWithValue("@superVisorName", reader.GetString(2));
        //            tst = reader.GetDateTime(3).ToString("hh:mm");
        //            dsstartTime = currentDate + " " + tst;
        //            dSheduledStartTime = Convert.ToDateTime(dsstartTime);
        //            cmd.Parameters.AddWithValue("@sheduledStartTime", dSheduledStartTime);
        //            tdt = reader.GetDateTime(4).ToString("hh:mm");
        //            dsendTime = currentDate + " " + tdt;
        //            dSheduledEndTime = Convert.ToDateTime(dsendTime);
        //            cmd.Parameters.AddWithValue("@sheduledEndTime", dSheduledEndTime);
        //            cmd.Parameters.AddWithValue("@sheduledDuration", reader.GetInt16(5));


        //            cmd.Parameters.AddWithValue("@statusOfDepartment", reader.GetString(8));
        //            j = cmd.ExecuteNonQuery();
        //        }
        //        con.Close();
        //    }
        //    return "Inserted";
        //}
        public string InsertData(string path)
        {

            string _path = path;
            FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            SqlConnection con = new SqlConnection("Server=HIB30BWAX2; Initial Catalog = OneTimeProgress; User ID = sa; Password = Passw0rd@12;");
            var reader = ExcelReaderFactory.CreateReader(stream);
            DateTime For101, For121, For343, For360, For144, For511, taskStartTime, departureTime, tempo;
            For101 = DateTime.Now;
            For121 = DateTime.Now;
            For343 = DateTime.Now;
            For360 = DateTime.Now;
            For144 = DateTime.Now;
            For511 = DateTime.Now;
            DateTime dateTimeStarter = DateTime.Now;
            int adder;
            if (dateTimeStarter.Minute <= 30)
            {
                adder = 30 - dateTimeStarter.Minute;
                dateTimeStarter = dateTimeStarter.AddMinutes(adder);
                tempo = dateTimeStarter;
            }
            else
            {
                adder = 60 - dateTimeStarter.Minute;
                dateTimeStarter = dateTimeStarter.AddMinutes(adder);
                tempo = dateTimeStarter;
            }

            if (reader.Name == "Flights")//Flights sheet
            {
                double taskStart, Departure;
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
                    taskStart = reader.GetDouble(7);
                    taskStartTime = dateTimeStarter.AddMinutes(taskStart);
                    dateTimeStarter = tempo;
                    cmd.Parameters.AddWithValue("@taskStartTime", taskStartTime);
                    Departure = reader.GetDouble(8);
                    departureTime = dateTimeStarter.AddMinutes(Departure);
                    dateTimeStarter = tempo;
                    cmd.Parameters.AddWithValue("@departureTime", departureTime);
                    if (reader.GetDouble(1) == 101)
                        For101 = departureTime;
                    else if (reader.GetDouble(1) == 121)
                        For121 = departureTime;
                    else if (reader.GetDouble(1) == 343)
                        For343 = departureTime;
                    else if (reader.GetDouble(1) == 360)
                        For360 = departureTime;
                    else if (reader.GetDouble(1) == 144)
                        For144 = departureTime;
                    else
                        For511 = departureTime;
                    j = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            reader.NextResult();
            if (reader.Name == "Tasks")//Tasks sheet
            {
                int j; double taskDuration, sheduledStart, actualStart, actualTimeDifference;
                reader.Read();
                con.Open();
                while (reader.Read())
                {
                    SqlCommand cmd = new SqlCommand("InsertIntoTaskList @flightNumber,@taskDetail,@duration,@startTime,@endTime,@statusOfTask,@actualStartTime,@actualEndTime,@timeDifference,@department,@staffName", con);
                    cmd.Parameters.AddWithValue("@flightNumber", reader.GetDouble(0));
                    cmd.Parameters.AddWithValue("@taskDetail", reader.GetString(1));
                    taskDuration = reader.GetDouble(2);
                    actualTimeDifference = reader.GetDouble(8);
                    sheduledStart = reader.GetDouble(11);
                    actualStart = reader.GetDouble(12);
                    DateTime sst, set, ast, aet;
                    cmd.Parameters.AddWithValue("@duration", taskDuration);
                    cmd.Parameters.AddWithValue("@statusOfTask", reader.GetString(5));
                    if (reader.GetDouble(0) == 101)
                    {
                        sst = For101.AddMinutes(-sheduledStart);
                        set = For101.AddMinutes(-sheduledStart + taskDuration);
                        ast = For101.AddMinutes(-actualStart);
                        aet = For101.AddMinutes(-actualStart + actualTimeDifference);
                        cmd.Parameters.AddWithValue("@startTime", sst);
                        cmd.Parameters.AddWithValue("@endTime", set);
                        cmd.Parameters.AddWithValue("@actualStartTime", ast);
                        cmd.Parameters.AddWithValue("@actualEndTime", aet);
                    }
                    else if (reader.GetDouble(0) == 121)
                    {
                        sst = For121.AddMinutes(-sheduledStart);
                        set = For121.AddMinutes(-sheduledStart + taskDuration);
                        ast = For121.AddMinutes(-actualStart);
                        aet = For121.AddMinutes(-actualStart + actualTimeDifference);
                        cmd.Parameters.AddWithValue("@startTime", sst);
                        cmd.Parameters.AddWithValue("@endTime", set);
                        cmd.Parameters.AddWithValue("@actualStartTime", ast);
                        cmd.Parameters.AddWithValue("@actualEndTime", aet);
                    }
                    else if (reader.GetDouble(0) == 343)
                    {
                        sst = For343.AddMinutes(-sheduledStart);
                        set = For343.AddMinutes(-sheduledStart + taskDuration);
                        ast = For343.AddMinutes(-actualStart);
                        aet = For343.AddMinutes(-actualStart + actualTimeDifference);
                        cmd.Parameters.AddWithValue("@startTime", sst);
                        cmd.Parameters.AddWithValue("@endTime", set);
                        cmd.Parameters.AddWithValue("@actualStartTime", ast);
                        cmd.Parameters.AddWithValue("@actualEndTime", aet);
                    }
                    else if (reader.GetDouble(0) == 360)
                    {
                        sst = For360.AddMinutes(-sheduledStart);
                        set = For360.AddMinutes(-sheduledStart + taskDuration);
                        ast = For360.AddMinutes(-actualStart);
                        aet = For360.AddMinutes(-actualStart + actualTimeDifference);
                        cmd.Parameters.AddWithValue("@startTime", sst);
                        cmd.Parameters.AddWithValue("@endTime", set);
                        cmd.Parameters.AddWithValue("@actualStartTime", ast);
                        cmd.Parameters.AddWithValue("@actualEndTime", aet);
                    }
                    else if (reader.GetDouble(0) == 144)
                    {
                        sst = For144.AddMinutes(-sheduledStart);
                        set = For144.AddMinutes(-sheduledStart + taskDuration);
                        ast = For144.AddMinutes(-actualStart);
                        aet = For144.AddMinutes(-actualStart + actualTimeDifference);
                        cmd.Parameters.AddWithValue("@startTime", sst);
                        cmd.Parameters.AddWithValue("@endTime", set);
                        cmd.Parameters.AddWithValue("@actualStartTime", ast);
                        cmd.Parameters.AddWithValue("@actualEndTime", aet);
                    }
                    else
                    {
                        sst = For511.AddMinutes(-sheduledStart);
                        set = For511.AddMinutes(-sheduledStart + taskDuration);
                        ast = For511.AddMinutes(-actualStart);
                        aet = For511.AddMinutes(-actualStart + actualTimeDifference);
                        cmd.Parameters.AddWithValue("@startTime", sst);
                        cmd.Parameters.AddWithValue("@endTime", set);
                        cmd.Parameters.AddWithValue("@actualStartTime", ast);
                        cmd.Parameters.AddWithValue("@actualEndTime", aet);
                    }
                    cmd.Parameters.AddWithValue("@timeDifference", actualTimeDifference);
                    cmd.Parameters.AddWithValue("@department", reader.GetString(9));
                    cmd.Parameters.AddWithValue("@staffName", reader.GetString(10));
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
        public string DeleteData()
        {
            return bussines.DeleteData();
        }
    }
}