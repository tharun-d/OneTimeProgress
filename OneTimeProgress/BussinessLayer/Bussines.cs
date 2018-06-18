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
        public bool LoginValidator(LoginModel loginModel)
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
        public FlightDetails GetDetailsForOneFlight(string flightNumber)
        {
            return dataAccess.GetDetailsForOneFlight(flightNumber);
        }
    }
}