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
        public string LoginValidator(LoginModel loginModel)
        {
            return dataAccess.LoginValidator(loginModel);
        }
        public List<FlightDetails> GetAllFlightDetails()
        {
            return dataAccess.GetAllFlightDetails();
        }
        public List<TaskLists> GetTasksForParticularFlight(string flightNumber)
        {
            return dataAccess.GetTasksForParticularFlight(flightNumber);
        }
        public List<ALLTaskLists> GetStatusOfAllTasks(string flightNumber)
        {
            return dataAccess.GetStatusOfAllTasks(flightNumber);
        }
        public FlightDetails GetDetailsForOneFlight(string flightNumber)
        {
            return dataAccess.GetDetailsForOneFlight(flightNumber);
        }
        public void UpdateTaskStatus(string flightNumber,string task,string statusUpdate)
        {
           dataAccess.UpdateTaskStatus(flightNumber,task, statusUpdate);
        }
    }
}