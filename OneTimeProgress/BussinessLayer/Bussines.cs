using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneTimeProgress.DataAccessLayer;
using OneTimeProgress.BussinessEntity;
namespace OneTimeProgress.BussinessLayer
{
    public class Bussines
    {
        DataAccess dataAccess = new DataAccess();
        //CommonThings commonThings = new CommonThings();
        //private string GetConnectionString()
        //{
        //    return CommonThings.GetConnectionString();
        //}
        public ConfirmLoginModel LoginValidator(LoginModel loginModel)
        {
            return dataAccess.LoginValidator(loginModel);
        }
        //public string LoginValidator(LoginModel loginModel)
        //{
        //    string connectionString;
        //    DataSet dataSet = new DataSet();
        //    connectionString = GetConnectionString();
        //    SqlParameter[] storedParams = new SqlParameter[2];
        //    storedParams[0] = new SqlParameter("@userName", loginModel.userName);
        //    storedParams[1] = new SqlParameter("@password", loginModel.password);
        //    dataSet = DAL.ExecuteDataset(connectionString, CommandType.StoredProcedure, commonThings.loginValidator, storedParams);
        //    if (dataSet.Tables.Count > 0)
        //    {
        //        return dataSet.Tables[0].Rows[0][0].ToString();
        //    }
        //    else
        //        return "Not Found";
        //}
        public List<FlightDetails> GetAllFlightDetails()
        {
            return dataAccess.GetAllFlightDetails();
        }
        //public List<FlightDetails> GetAllFlightDetails()
        //{
        //    List<FlightDetails> flightDetails = new List<FlightDetails>();
        //    string connectionString;
        //    DataSet dataSet = new DataSet();
        //    connectionString = GetConnectionString();
        //    SqlParameter[] storedParams = new SqlParameter[1];
        //    dataSet = DAL.ExecuteDataset(connectionString, CommandType.StoredProcedure, commonThings.loginValidator, storedParams);
        //    if (dataSet.Tables.Count > 0)
        //    {
        //        int rowsCount = dataSet.Tables[0].Rows.Count;
        //        int colomnsCount = dataSet.Tables[0].Columns.Count;

        //        for (int i = 0; i < rowsCount; i++)
        //        {
        //            for (int j = 0; j < colomnsCount; j++)
        //            {
        //                FlightDetails FD = new FlightDetails()
        //                {
        //                    FlightNumber = Convert.ToString(dataSet.Tables[0].Rows[i][j]),
        //                    FlightModel = Convert.ToString(dr[1]),
        //                    CurrentStation = Convert.ToString(dr[2]),
        //                    Bay = Convert.ToInt32(dr[3]),
        //                    TaskStartTime = Convert.ToString(dr[4]),
        //                    Departure = Convert.ToString(dr[5])
        //                };
        //                flightDetails.Add(FD);
        //            }
        //        }

        //    }
        //}
        public List<TaskLists> GetTasksForParticularFlight(string flightNumber, string staffName, string staffDepartment)
        {
            return dataAccess.GetTasksForParticularFlight(flightNumber, staffName, staffDepartment);
        }
        public List<ALLTaskLists> GetStatusOfAllTasks(string flightNumber, string superVisorDepartment)
        {
            return dataAccess.GetStatusOfAllTasks(flightNumber, superVisorDepartment);
        }
        public FlightDetails GetDetailsForOneFlight(string flightNumber)
        {
            return dataAccess.GetDetailsForOneFlight(flightNumber);
        }
        public void UpdateTaskStartStatus(string flightNumber, string task, string statusUpdate, DateTime currentTime)
        {
            dataAccess.UpdateTaskStartStatus(flightNumber, task, statusUpdate, currentTime);
        }
        public void UpdateTaskEndStatus(string flightNumber, string task, string statusUpdate, DateTime currentTime, double timeDifference)
        {
            dataAccess.UpdateTaskEndStatus(flightNumber, task, statusUpdate, currentTime, timeDifference);
        }
        public DateTime GettingStartTime(string flightNumber, string id)
        {
            return dataAccess.GettingStartTime(flightNumber, id);
        }
        public DateTime GettingEndTime(string flightNumber, string id)
        {
            return dataAccess.GettingEndTime(flightNumber, id);
        }
        public DateTime GettingActualStartTime(string flightNumber, string id)
        {
            return dataAccess.GettingActualStartTime(flightNumber, id);
        }
        public List<Departments> GetAllDepartmentsStatus(string flightNumber)
        {
            return dataAccess.GetAllDepartmentsStatus(flightNumber);
        }
  
        public string DeleteData()
        {
            return dataAccess.DeleteData();
        }
        public void UpdateStatusInDepartments(string flightNumber, string departmentName, DateTime time)
        {
            dataAccess.UpdateStatusInDepartments(flightNumber, departmentName, time);
        }
        public void UpdateStatusInDepartmentsCompleted(string flightNumber, string departmentName, DateTime time)
        {
            dataAccess.UpdateStatusInDepartmentsCompleted(flightNumber, departmentName, time);
        }
        public bool InProgressOrYetToStartTasksForDepartment(string flightNumber, string departmentName)
        {
            return dataAccess.InProgressOrYetToStartTasksForDepartment(flightNumber, departmentName);
        }
        public bool InProgressTasksForDepartment(string flightNumber, string departmentName)
        {
            return dataAccess.InProgressTasksForDepartment(flightNumber, departmentName);
        }
        public List<DummyTaskLists> GetDummyTasks(string flightNumber, string department)
        {
            return dataAccess.GetDummyTasks(flightNumber, department);
        }
    }
}