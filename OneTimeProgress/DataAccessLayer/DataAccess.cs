using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using OneTimeProgress.BussinessEntity;
using OneTimeProgress.Common;

namespace OneTimeProgress.DataAccessLayer
{
    public class DataAccess
    {
        CommonThings commonThings = new CommonThings();
        private string GetConnectionString()
        {
            return CommonThings.GetConnectionString();
        }
        public string LoginValidator(LoginModel loginModel)
        {
            string result;
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.loginValidator, con);
            SqlParameter p1 = new SqlParameter("@userName", loginModel.userName);
            SqlParameter p2 = new SqlParameter("@password", loginModel.password);
            sda.Parameters.Add(p1);
            sda.Parameters.Add(p2);

            SqlDataReader dr = sda.ExecuteReader();
            if(dr.Read())
            { 
                result=Convert.ToString(dr[0]);
                con.Close();
            }
            else
                result = "Not Found";
            return result;
        }
        public List<FlightDetails> GetAllFlightDetails()
        {
            List<FlightDetails> flightDetails = new List<FlightDetails>();

            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.getAllFlightsDetails, con);
            SqlDataReader dr = sda.ExecuteReader();
            while (dr.Read())
            {
                FlightDetails details = new FlightDetails()
                {
                    FlightNumber = Convert.ToString(dr[0]),
                    FlightModel = Convert.ToString(dr[1]),
                    CurrentStation = Convert.ToString(dr[2]),
                    Bay = Convert.ToInt32(dr[3]),
                    TaskStartTime = Convert.ToString(dr[4]),
                    Departure = Convert.ToString(dr[5])
                };
                flightDetails.Add(details);
            }
            con.Close();
            return flightDetails;
        }
        public List<TaskLists> GetTasksForParticularFlight(string flightNumber)
        {
            List<TaskLists> taskLists = new List<TaskLists>();
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.getTasksForParticularFlight, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            sda.Parameters.Add(p1);
            SqlDataReader dr = sda.ExecuteReader();
            while (dr.Read())
            {
                TaskLists details = new TaskLists()
                {
                    Id= Convert.ToInt32(dr[0]),
                    Task = Convert.ToString(dr[1]),
                    Duration = Convert.ToInt32(dr[2]),
                    StartTime = (Convert.ToDateTime(dr[3])).ToShortTimeString(),
                    EndTime = (Convert.ToDateTime(dr[4])).ToShortTimeString(),
                    Status = Convert.ToString(dr[5]),
                    ActualStartTime = Convert.ToDateTime(dr[6]).ToShortTimeString(),
                    ActualEndTime = Convert.ToDateTime(dr[7]).ToShortTimeString(),
                    TimeDifference = Convert.ToInt32(dr[8])
                };
                if (details.Status == "Yet To Start")
                {
                    details.TimeDifference = 0;
                    details.ActualStartTime = "-";
                    details.ActualEndTime = "-";
                }
                if (details.Status == "In Progress")
                {
                    details.TimeDifference = 0;
                    details.ActualEndTime = "-";
                    details.CurrentTimeMinusActualStartTime = DateTime.Now.Subtract(Convert.ToDateTime(dr[6])).Minutes;
                }
                if (details.TimeDifference < 0)
                {
                    details.TimeDifference = 0;
                }
                taskLists.Add(details);
            }
            con.Close();
            return taskLists;
        }
        public DateTime GetIdealStartTimeForTask(int minutes)
        {
            DateTime startTime;
            DateTime flightdeparture = (DateTime.Now.AddHours(2));
            startTime = (flightdeparture.AddMinutes(-minutes));
            return startTime;
        }
        public DateTime GetIdealEndTimeForTask(int startMinutes,int Endminutes)
        {
            DateTime endTime;
            DateTime startTime = GetIdealStartTimeForTask(startMinutes);
            endTime = (startTime.AddMinutes(Endminutes));
            return endTime;
        }
        public List<ALLTaskLists> GetStatusOfAllTasks(string flightNumber)
        {
            List<ALLTaskLists> taskLists = new List<ALLTaskLists>();
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.getStatusOfAllTasks, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            sda.Parameters.Add(p1);
            SqlDataReader dr = sda.ExecuteReader();
            while (dr.Read())
            {
                ALLTaskLists details = new ALLTaskLists()
                {
                    Task = Convert.ToString(dr[0]),
                    Duration = Convert.ToInt32(dr[1]),
                    StartTime = Convert.ToDateTime(dr[2]).ToShortTimeString(),
                    EndTime = Convert.ToDateTime(dr[3]).ToShortTimeString(),
                    ActualStartTime = Convert.ToDateTime(dr[4]).ToShortTimeString(),
                    ActualEndTime = Convert.ToDateTime(dr[5]).ToShortTimeString(),
                    TimeDifference = Convert.ToInt32(dr[6]),
                    StatusOfTask = Convert.ToString(dr[7])
                    
                };
                if (details.StatusOfTask=="Yet To Start")
                {
                    details.TimeDifference = 0;
                    details.ActualStartTime = "-";
                    details.ActualEndTime = "-";
                }
                if (details.StatusOfTask == "In Progress")
                {
                    details.TimeDifference = 0;
                    details.ActualEndTime = "-";
                }
                if (details.TimeDifference<0)
                {
                    details.TimeDifference = 0;
                }
                taskLists.Add(details);
            }
            con.Close();
            return taskLists;
        }
        public FlightDetails GetDetailsForOneFlight(string flightNumber)
        {
            FlightDetails flightDetails = new FlightDetails();
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.getDetailsForOneFlight, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            sda.Parameters.Add(p1);
            SqlDataReader dr = sda.ExecuteReader();
            if (dr.Read())
            {
                flightDetails.FlightNumber = Convert.ToString(dr[0]);
                flightDetails.FlightModel = Convert.ToString(dr[1]);
                flightDetails.CurrentStation = Convert.ToString(dr[2]);
                flightDetails.Bay = Convert.ToInt32(dr[3]);
                flightDetails.TaskStartTime = Convert.ToString(dr[4]);
                flightDetails.Departure = Convert.ToString(dr[5]);
            }
            con.Close();
            return flightDetails;
        }
        public void UpdateTaskStartStatus(string flightNumber, string id, string statusUpdate,DateTime currentTime)
        {
            int j;
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.updateTaskStartTime, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            SqlParameter p2 = new SqlParameter("@id", id);
            SqlParameter p3 = new SqlParameter("@statusUpdate", statusUpdate);
            SqlParameter p4 = new SqlParameter("@currentTime", currentTime);
            sda.Parameters.Add(p1);
            sda.Parameters.Add(p2);
            sda.Parameters.Add(p3);
            sda.Parameters.Add(p4);
            j = sda.ExecuteNonQuery();
            con.Close();
        }
        public void UpdateTaskEndStatus(string flightNumber, string id, string statusUpdate, DateTime currentTime,int timeDifference)
        {
            int j;
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }  
            sda = new SqlCommand(commonThings.updateTaskEndTime, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            SqlParameter p2 = new SqlParameter("@id", id);
            SqlParameter p3 = new SqlParameter("@statusUpdate", statusUpdate);
            SqlParameter p4 = new SqlParameter("@currentTime", currentTime);
            SqlParameter p5 = new SqlParameter("@timeDifference", timeDifference);
            sda.Parameters.Add(p1);
            sda.Parameters.Add(p2);
            sda.Parameters.Add(p3);
            sda.Parameters.Add(p4);
            sda.Parameters.Add(p5);
            j = sda.ExecuteNonQuery();
            con.Close();
        }
        public DateTime GettingStartTime(string flightNumber, string id)
        {
            DateTime sheduledStartTime;
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.gettingStartTime, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            sda.Parameters.Add(p1);
            SqlParameter p2 = new SqlParameter("@id", id);
            sda.Parameters.Add(p2);
            SqlDataReader dr = sda.ExecuteReader();
            if (dr.Read())
            {
                sheduledStartTime = Convert.ToDateTime(dr[0]);
                con.Close();
                return sheduledStartTime;
            }
            return DateTime.Now;
        }
        public DateTime GettingEndTime(string flightNumber, string id)
        {
            DateTime sheduledEndTime;
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.gettingEndTime, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            sda.Parameters.Add(p1);
            SqlParameter p2 = new SqlParameter("@id", id);
            sda.Parameters.Add(p2);
            SqlDataReader dr = sda.ExecuteReader();
            if (dr.Read())
            {
                sheduledEndTime = Convert.ToDateTime(dr[0]);
                con.Close();
                return sheduledEndTime;
            }
            return DateTime.Now;
        }
    }
}