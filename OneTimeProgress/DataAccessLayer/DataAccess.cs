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
                    StartTime = GetIdealStartTimeForTask(Convert.ToInt32(dr[3])).ToShortTimeString(),
                    EndTime = GetIdealEndTimeForTask(Convert.ToInt32(dr[3]), Convert.ToInt32(dr[2])).ToShortTimeString(),
                    Status = Convert.ToString(dr[4])
                };
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
                    Status = Convert.ToString(dr[1]),
                };
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
        public void UpdateTaskStatus(string flightNumber, string id, string statusUpdate)
        {
            int j;
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.updateTaskStatus, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            SqlParameter p2 = new SqlParameter("@id", id);
            SqlParameter p3 = new SqlParameter("@statusUpdate", statusUpdate);
            sda.Parameters.Add(p1);
            sda.Parameters.Add(p2);
            sda.Parameters.Add(p3);
            j = sda.ExecuteNonQuery();
            con.Close();
        }
    }
}