using System;
using System.Runtime.Caching;
using System.Configuration;

namespace OneTimeProgress.Common
{
    public class CommonThings
    {
        private static string key = "CONN_STRING";
        private static ObjectCache _cache = MemoryCache.Default;
        // keeping connectin  string in Cache 24 hours 
        private static int _defaultCacheExpireInSeconds = 86400;
        public static string GetConnectionString()
        {
            string connString = string.Empty;
            if (_cache.Contains(key))
            {
                connString = (string)_cache.Get(key);
            }
            else
            {
                 connString = ConfigurationManager.ConnectionStrings["Dev"].ConnectionString;
                _cache.Add(key, connString, DateTimeOffset.Now.AddSeconds(_defaultCacheExpireInSeconds));
            }

            return connString;
        }
        #region Procedures
        public readonly string loginValidator = "LoginValidator @userName,@password";
        public readonly string getAllFlightsDetails = "GetAllFlightsDetails";
        public readonly string getTasksForParticularFlight = "GetTasksForParticularFlight @flightNumber,@staffName,@staffDepartment";
        public readonly string getTasksForParticularFlightDepartmentWise = "GetTasksForParticularFlightDepartmentWise @flightNumber,@supervisorDepartment";
        public readonly string getStatusOfAllTasks = "GetStatusOfAllTasks @flightNumber";
        public readonly string getDetailsForOneFlight = "GetDetailsForOneFlight @flightNumber";
        public readonly string updateTaskStartTime = "UpdateTaskStartTime @flightNumber,@id,@statusUpdate,@currentTime";
        public readonly string updateTaskEndTime = "UpdateTaskEndTime @flightNumber,@id,@statusUpdate,@currentTime,@timeDifference";
        public readonly string gettingStartTime = "GettingStartTime @flightNumber,@id";
        public readonly string gettingEndTime = "GettingEndTime @flightNumber,@id";
        public readonly string gettingActualStartTime = "GettingActualStartTime @flightNumber,@id";
        public readonly string gettingAllDepartmentsStatus = "GettingAllDepartmentsStatus @flightNumber";
        public readonly string deleteData = "DeleteData";
        public readonly string countingTasksForDepartment = "CountingTasksForDepartment @flightNumber,@departmentName";
        public readonly string inProgressTasksForDepartment = "InProgressTasksForDepartment @flightNumber,@departmentName";
        public readonly string inProgressOrYetToStartTasksForDepartment = "InProgressOrYetToStartTasksForDepartment @flightNumber,@departmentName";
        public readonly string updateStatusInDepartments = "UpdateStatusInDepartments @flightNumber,@departmentName,@actualStartTime";
        public readonly string updateStatusInDepartmentsCompleted = "UpdateStatusInDepartmentsCompleted @flightNumber,@departmentName,@actualEndTime";
        #endregion
    }
}