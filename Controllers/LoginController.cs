using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using mozn.Models;

namespace mozn.Controllers
{
    public class LoginController : Controller
    {

        private readonly IConfiguration _configuration;
        private SqlConnection con;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private void connection()
        {
            string connectionString = _configuration.GetConnectionString("MoznDataBase").ToString();
            con = new SqlConnection(connectionString);
        }

        [HttpGet]
        [Route("api/[controller]")]
        public List<LoginForm> Index()
        {
            List<LoginForm> userlist = new List<LoginForm>();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("GetAllloginData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    userlist.Add(
                        new LoginForm
                        {
                            userName = Convert.ToString(dr["userName"]),
                            Password = Convert.ToString(dr["Password"])                          
                        });
                }
                return userlist;

            }
            catch(Exception ex)
            {
                return userlist;
            }
        }

        [HttpPost]
        [Route("api/[controller]")]
        private void userRegister(LoginForm LoginForm)
        {
            SqlConnection myConnection = new SqlConnection();
            connection();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "INSERT INTO tblEmployee (EmployeeId,Name,ManagerId) Values (@EmployeeId,@Name,@ManagerId)";
            sqlCmd.Connection = myConnection;


            sqlCmd.Parameters.AddWithValue("@EmployeeId", LoginForm.userName);
            sqlCmd.Parameters.AddWithValue("@Name", LoginForm.Password);
            myConnection.Open();
            int rowInserted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}
