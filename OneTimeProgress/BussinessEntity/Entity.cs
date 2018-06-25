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
    public class FlightDetails
    {
        public string FlightNumber { get; set; }
        public string FlightModel { get; set; }
        public string CurrentStation { get; set; }
        public int Bay { get; set; }
        public string TaskStartTime { get; set; }
        public string Departure { get; set; }
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
        public int CurrentTimeMinusActualStartTime { get; set; } // caluclated if Status is In Progress
        public string Status { get; set; }
    }
    public class ALLTaskLists
    {
        public string Task { get; set; }
        public int Duration { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ActualStartTime { get; set; }
        public string ActualEndTime { get; set; }
        public int TimeDifference { get; set; }
        public string StatusOfTask { get; set; }
    }
}