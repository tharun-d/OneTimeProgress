using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneTimeProgress.BussinessEntity
{
    public class LoginModel
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
    public class ConfirmLoginModel
    {
        public string userName { get; set; }
        public string userType { get; set; }
        public string userDepartment { get; set; }
    }
    public class FlightDetails
    {
        public string FlightNumber { get; set; }
        public string FlightModel { get; set; }
        public string CurrentStation { get; set; }
        public int Bay { get; set; }
        public string TaskStartTime { get; set; }
        public string Departure { get; set; }
        public string Status { get; set; }//tasks status
        public string Colour { get; set; }//colour based on status
    }
    public class TaskLists
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public int Duration { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; } //starttime + duration
        public string ActualStartTime { get; set; }
        public string ActualEndTime { get; set; }
        public int TimeDifference { get; set; }
        public double CurrentTimeMinusActualStartTime { get; set; } // caluclated if Status is In Progress
        public string Status { get; set; }
        public string Colour { get; set; }// status completed then check the timedifference - duration if it is>1 make it red bar
        public double ProgressPercentage { get; set; }
    }
    public class ALLTaskLists
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public int Duration { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; } //starttime + duration
        public string ActualStartTime { get; set; }
        public string ActualEndTime { get; set; }
        public int TimeDifference { get; set; }
        public double CurrentTimeMinusActualStartTime { get; set; } // caluclated if Status is In Progress
        public string Status { get; set; }
        public string Colour { get; set; }// status completed then check the timedifference - duration if it is>1 make it red bar
        public double ProgressPercentage { get; set; }
        public string StaffName { get; set; }
    }
    public class DummyTaskLists
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string SuperVisor { get; set; }
        public int Duration { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; } //starttime + duration
        public string ActualStartTime { get; set; }
        public string ActualEndTime { get; set; }
        public int TimeDifference { get; set; }
        public double CurrentTimeMinusActualStartTime { get; set; } // caluclated if Status is In Progress
        public string Status { get; set; }
        public string Colour { get; set; }// status completed then check the timedifference - duration if it is>1 make it red bar
        public double ProgressPercentage { get; set; }
        public string StaffName { get; set; }
    }
    public class Departments
    {
        public string DepartmentName { get; set; }
        public string SupervisorName { get; set; }
        public string SheduledStartTime { get; set; }
        public string SheduledEndTime { get; set; }
        public int SheduledDuration { get; set; }//sheduleend-shedulestart
        public string ActualStartTime { get; set; }
        public string ActualEndTime { get; set; }
        public double ActualDuration { get; set; }//actualend-actualstart
        public string StatusofDepatment { get; set; }
        public string Colour { get; set; }//progress bar color in case progress
        public double ProgressPercentage { get; set; }
    }
}