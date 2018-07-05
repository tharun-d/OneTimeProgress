using Npgsql;
using OneTimeProgress.BussinessEntity;
using OneTimeProgress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneTimeProgress.DataAccessLayer
{
    public class PostgressDataAccess
    {
        //CommonThings commonThings = new CommonThings();
        //private string GetConnectionString()
        //{
        //    return CommonThings.GetConnectionString();
        //}
        //public ConfirmLoginModel LoginValidator(LoginModel loginModel)
        //{
        //    NpgsqlCommand sda;
        //    NpgsqlConnection con = new NpgsqlConnection(GetConnectionString());
        //    ConfirmLoginModel result = new ConfirmLoginModel();
        //        con.Open();
            
        //    sda = new NpgsqlCommand(commonThings.loginValidator, con);
         
        //    if (dr.Read())
        //    {
        //        result.userName = Convert.ToString(dr[0]);
        //        result.userType = Convert.ToString(dr[1]);
        //        result.userDepartment = Convert.ToString(dr[2]);
        //        con.Close();
        //    }
        //    return result;
        //}

    }
}