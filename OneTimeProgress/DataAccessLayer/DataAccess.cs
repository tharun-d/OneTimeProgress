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
    }
}