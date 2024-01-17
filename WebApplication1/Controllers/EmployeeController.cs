using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Intrinsics.Arm;
using WebAPI.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration? configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this._env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from dbo.Employee";
            DataTable table = new DataTable();
            string conString = configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(conString))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"insert into dbo.Employee (EmployeeName, Department, DateOfJoiing, PhotoFileName) values ('"
                           + emp.EmployeeName + "',"
                           + "'" + emp.Department + "',"
                           + "'" + emp.DateOfJoiing + "',"
                           + "'" + emp.PhotoFileName + "')";
            DataTable table = new DataTable();
            string conString = configuration.GetConnectionString("EmployeeAppCon");
            using (SqlConnection myCon = new SqlConnection(conString))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }

            return new JsonResult("Employee Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"update dbo.Employee set "
                            + "EmployeeName = '" + emp.EmployeeName + "',"
                            + "Department = '" + emp.Department + "',"
                            + "DateOfJoiing = '" + emp.DateOfJoiing + "',"
                            + "PhotoFileName = '" + emp.PhotoFileName + "' "
                           + " where EmployeeId = " + emp.EmployeeId;
            DataTable table = new DataTable();
            string conString = configuration.GetConnectionString("EmployeeAppCon");
            using (SqlConnection myCon = new SqlConnection(conString))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }

            return new JsonResult("Employee Updated Successfully");
        }


        [HttpDelete("{EmployeeID}")]
        public JsonResult Delete(int EmployeeID)
        {
            string query = @"delete from dbo.Employee "
                            + " where EmployeeId = " + EmployeeID;
            DataTable table = new DataTable();
            string conString = configuration.GetConnectionString("EmployeeAppCon");
            using (SqlConnection myCon = new SqlConnection(conString))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }

            return new JsonResult("Employee Deleted Successfully");
        }

        [Route ("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + @"\Photos\" + fileName;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("annonymous.png");             
            }
        }
    }
}
