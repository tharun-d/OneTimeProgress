using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using OneTimeProgress.BussinessEntity;

namespace OneTimeProgress.DataAccessLayer
{
    public class DataAccess
    {
        string connection = ConfigurationManager.ConnectionStrings["Dev"].ConnectionString;
        public bool LoginValidator(LoginModel loginModel)
        {
            SqlCommand sda;
            SqlConnection con = new SqlConnection(connection);

            con.Open();
            sda = new SqlCommand("LoginValidator @userName,@password", con);
            SqlParameter p1 = new SqlParameter("@userName", loginModel.userName);
            SqlParameter p2 = new SqlParameter("@password", loginModel.password);
            sda.Parameters.Add(p1);
            sda.Parameters.Add(p2);

            SqlDataReader dr = sda.ExecuteReader();
            if(dr.HasRows)
            {
                con.Close();
                return true;
            }
            
            return false;
        }
        public List<FlightDetails> GetAllFlightDetails()
        {
            List<FlightDetails> flightDetails = new List<FlightDetails>();

            SqlCommand sda;
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            sda = new SqlCommand("GetAllFlightsDetails", con);
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
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            sda = new SqlCommand("GetTasksForParticularFlight @flightNumber", con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            sda.Parameters.Add(p1);
            SqlDataReader dr = sda.ExecuteReader();
            while (dr.Read())
            {
                TaskLists details = new TaskLists()
                {
                    Task = Convert.ToString(dr[0]),
                    Duration = Convert.ToInt32(dr[1]),
                    StartTime = GetIdealStartTimeForTask(Convert.ToInt32(dr[2])).ToShortTimeString(),
                    EndTime = GetIdealEndTimeForTask(Convert.ToInt32(dr[2]), Convert.ToInt32(dr[1])).ToShortTimeString()
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
        public FlightDetails GetDetailsForOneFlight(string flightNumber)
        {
            FlightDetails flightDetails = new FlightDetails();
            SqlCommand sda;
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            sda = new SqlCommand("GetDetailsForOneFlight @flightNumber", con);
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
    }
}