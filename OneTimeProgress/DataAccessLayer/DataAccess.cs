﻿using System;
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
                FlightDetails flightDetail = new FlightDetails()
                {
                    FlightNumber = Convert.ToString(dr[0]),
                    FlightModel = Convert.ToString(dr[1]),
                    CurrentStation = Convert.ToString(dr[2]),
                    Bay = Convert.ToInt32(dr[3]),
                    TaskStartTime = Convert.ToString(dr[4]),
                    Departure = Convert.ToString(dr[5])
                };
                if (flightDetail.FlightNumber == "1000")
                {
                    flightDetail.Status = "Departed - On time";
                    flightDetail.Colour = "green";
                }
                else if (flightDetail.FlightNumber == "1001")
                {
                    flightDetail.Status = "In Progress";
                    flightDetail.Colour = "yellow";
                }
                else
                {
                    flightDetail.Status = "Sheduled";
                    flightDetail.Colour = "white";
                }
                flightDetails.Add(flightDetail);
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
                    Id = Convert.ToInt32(dr[0]),
                    Task = Convert.ToString(dr[1]),
                    Duration = Convert.ToInt32(dr[2]),//originaltimedifference
                    StartTime = (Convert.ToDateTime(dr[3])).ToString("HH:mm"),
                    EndTime = (Convert.ToDateTime(dr[4])).ToString("HH:mm"),
                    Status = Convert.ToString(dr[5]),
                    ActualStartTime = Convert.ToDateTime(dr[6]).ToString("HH:mm"),
                    ActualEndTime = Convert.ToDateTime(dr[7]).ToString("HH:mm"),
                    TimeDifference = Convert.ToInt32(dr[8]),//actualtimedifference   
                    Colour = ""
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
                    details.CurrentTimeMinusActualStartTime = DateTime.Now.Subtract(Convert.ToDateTime(dr[6])).TotalMinutes;
                    details.ProgressPercentage = ProgressCaluclator(details.CurrentTimeMinusActualStartTime, details.Duration);
                    if(details.ProgressPercentage==100)
                    {
                        details.Colour = "Red";
                    }
                }
                if (details.TimeDifference < 0)
                {
                    details.TimeDifference = 0;
                }
                if (details.Status == "Completed")
                {
                    if (details.TimeDifference-details.Duration > 0)
                    {
                        details.Colour = "Red";
                    }
                }
                    taskLists.Add(details);
            }
            con.Close();
            return taskLists;
        }
        public double ProgressCaluclator(double currentDuration,int totalDuration)
        {
            double percentage;
            percentage = (currentDuration / totalDuration)*100;
            if (percentage>100)
            {
                return 100;
            }
            return percentage;
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
            sda = new SqlCommand(commonThings.getTasksForParticularFlight, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            sda.Parameters.Add(p1);
            SqlDataReader dr = sda.ExecuteReader();
            while (dr.Read())
            {
                ALLTaskLists details = new ALLTaskLists()
                {
                    Id = Convert.ToInt32(dr[0]),
                    Task = Convert.ToString(dr[1]),
                    Duration = Convert.ToInt32(dr[2]),//originaltimedifference
                    StartTime = (Convert.ToDateTime(dr[3])).ToString("HH:mm"),
                    EndTime = (Convert.ToDateTime(dr[4])).ToString("HH:mm"),
                    Status = Convert.ToString(dr[5]),
                    ActualStartTime = Convert.ToDateTime(dr[6]).ToString("HH:mm"),
                    ActualEndTime = Convert.ToDateTime(dr[7]).ToString("HH:mm"),
                    TimeDifference = Convert.ToInt32(dr[8]),//actualtimedifference   
                    Colour = ""
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
                    details.CurrentTimeMinusActualStartTime = DateTime.Now.Subtract(Convert.ToDateTime(dr[6])).TotalMinutes;
                    details.ProgressPercentage = ProgressCaluclator(details.CurrentTimeMinusActualStartTime, details.Duration);
                    if (details.ProgressPercentage == 100)
                    {
                        details.Colour = "Red";
                    }
                }
                if (details.TimeDifference < 0)
                {
                    details.TimeDifference = 0;
                }
                if (details.Status == "Completed")
                {
                    if (details.TimeDifference - details.Duration > 0)
                    {
                        details.Colour = "Red";
                    }
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
        public void UpdateTaskEndStatus(string flightNumber, string id, string statusUpdate, DateTime currentTime,double timeDifference)
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
        public DateTime GettingActualStartTime(string flightNumber, string id)
        {
            DateTime actualStartTime;
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.gettingActualStartTime, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            sda.Parameters.Add(p1);
            SqlParameter p2 = new SqlParameter("@id", id);
            sda.Parameters.Add(p2);
            SqlDataReader dr = sda.ExecuteReader();
            if (dr.Read())
            {
                actualStartTime = Convert.ToDateTime(dr[0]);
                con.Close();
                return actualStartTime;
            }
            return DateTime.Now;
        }
        public List<Departments> GetAllDepartmentsStatus(string flightNumber)
        {
            List<Departments> listOfDepartments = new List<Departments>();
            SqlCommand sda;
            SqlConnection con = new SqlConnection(GetConnectionString());
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sda = new SqlCommand(commonThings.gettingAllDepartmentsStatus, con);
            SqlParameter p1 = new SqlParameter("@flightNumber", flightNumber);
            sda.Parameters.Add(p1);      
            SqlDataReader dr = sda.ExecuteReader();

            if (dr.Read())
            {
                Departments departments = new Departments()
                {
                    DepatmentName=Convert.ToString(dr[0]),
                    SupervisorName=Convert.ToString(dr[1]),
                    SheduledStartTime = Convert.ToDateTime(dr[2]).ToString("HH:mm"),
                    SheduledEndTime = Convert.ToDateTime(dr[3]).ToString("HH:mm"),
                    ActualStartTime =Convert.ToDateTime(dr[4]).ToString("HH:mm"),
                    ActualEndTime = Convert.ToDateTime(dr[5]).ToString("HH:mm"),
                    StatusofDepatment=Convert.ToString(dr[6])
                };
                departments.SheduledDuration = (Convert.ToDateTime(dr[3]) - Convert.ToDateTime(dr[2])).ToString("HH:mm");
                if(departments.StatusofDepatment=="Completed")
                {
                    departments.ActualDuration = (Convert.ToDateTime(dr[5]) - Convert.ToDateTime(dr[4])).ToString("HH:mm");
                }
                if (departments.StatusofDepatment == "In Progress")
                {
                    
                }
                listOfDepartments.Add(departments);
            }
            con.Close();
            return listOfDepartments;
        }
    }
}