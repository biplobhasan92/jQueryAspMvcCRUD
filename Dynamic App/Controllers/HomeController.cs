using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Dynamic_App.Models;
using System.Web.Script.Serialization;

namespace Dynamic_App.Controllers
{
    public class HomeController : Controller
    {

        // DBConn
        string conStr = "Data Source=.; Initial Catalog=Student;Integrated Security=True;";
        // Data Source=.;Initial Catalog=Student;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();


        public ActionResult Index()
        {
            List<Department> listDepts = this.getsDept();
            ViewBag.listDepts = listDepts;
            return View();
        }

        public ActionResult Indexs()
        {
            DataSet es = this.getEmployees();
            ViewBag.empList = es.Tables[0];
            return View();
        }



        public ActionResult EditEmp(int id)
        {
            Employee e2 = new Employee();
            List<Department> listDepts = this.getsDept();
            ViewBag.listDepts = listDepts;
            List<Employee> eList = this.getEmpByID(id);
            e2 = eList.FirstOrDefault();
            return View(e2);
        }




        public bool SaveEmployee(Employee emp)
        {
            bool b = false;
            try
            {
                string sSQL = @"insert into Employee(EmployeeName,EmployeeEmail,DepartmentID, EmployeeGender) " +
                    " values('"+emp.EmployeeName+"', '"+emp.EmployeeEmail+"', "+emp.DepartmentID+ ", " + emp.EmployeeGender + " )";

                conn = new SqlConnection(conStr);
                cmd = new SqlCommand(sSQL, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
                b = true;
                conn.Close();
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;
        }


        [HttpPost]
        public JsonResult Save(Employee e)
        {
            Employee employee = new Employee();
            if (this.SaveEmployee(e))
            {
                employee.ErrorMessage = "";
            }
            else
            {
                employee.ErrorMessage = "Data Not Saved";
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(employee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(Employee e)
        {
            Employee e2 = new Employee();
            if (!this.UpdateEmployee(e))
            {
                e2.ErrorMessage = "Update Faild!";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(e2);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Delete(int id)
        {
            if (this.DeleteEmployee(id))
                return RedirectToAction("Indexs");
            return RedirectToAction("Indexs");
        }

        public bool UpdateEmployee(Employee emp)
        {
            bool b = false;
            try
            {
                string sSQL = @"Update Employee SET EmployeeName='" + emp.EmployeeName + "',EmployeeEmail='" + emp.EmployeeEmail + "',DepartmentID=" + emp.DepartmentID + ", EmployeeGender=" + emp.EmployeeGender + "  " +
                    " Where EmployeeID ="+emp.EmployeeID;

                conn = new SqlConnection(conStr);
                cmd = new SqlCommand(sSQL, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
                b = true;
                conn.Close();
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;
        }

        public bool DeleteEmployee(int id)
        {
            bool b = false;
            try
            {
                string sSQL = @"Delete from Employee Where EmployeeID =" + id;
                conn = new SqlConnection(conStr);
                cmd = new SqlCommand(sSQL, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
                b = true;
                conn.Close();
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;
        }

        public DataSet getEmployees()
        {
            string sSql = @"Select " +
                "HH.* , " +
                "(Select DepartmentName From Department Where DepartmentID = HH.DepartmentID) AS DepartmentName, " +
                " CASE WHEN HH.EmployeeGender = 1 THEN 'Male' ELSE 'Female' END as GenderName " +
              " from Employee AS HH ";
            conn = new SqlConnection(conStr);
            cmd = new SqlCommand(sSql, conn);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public List<Employee> getEmpByID(int id)
        {
            List<Employee> employees = new List<Employee>();
            
            try 
            {
                string sSql = "Select HH.* , (Select DepartmentName From Department Where DepartmentID = HH.DepartmentID) AS DepartmentName from Employee AS HH Where EmployeeID ="+id;
                conn = new SqlConnection(conStr);
                cmd = new SqlCommand(sSql, conn);
                cmd.CommandType = CommandType.Text;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Employee emp = new Employee();
                    emp.EmployeeID = Convert.ToInt32(dr["EmployeeID"]);
                    emp.EmployeeName = dr["EmployeeName"].ToString();
                    emp.DepartmentID = Convert.ToInt32(dr["EmployeeID"]);
                    emp.DepartmentName = dr["DepartmentName"].ToString();
                    emp.EmployeeEmail = dr["EmployeeEmail"].ToString();
                    emp.EmployeeGender = Convert.ToInt32(dr["EmployeeGender"]);
                    employees.Add(emp);
                }
            }
            catch (Exception ex)
            {

            }
            return employees;
        }

        public List<Department> getsDept()
        {
            
            List<Department> Departments = new List<Department>();

            try
            {
                string _sSQL = "Select * from Department";
                conn = new SqlConnection(conStr);
                cmd = new SqlCommand(_sSQL, conn);
                cmd.CommandType = CommandType.Text;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Department d = new Department();
                    d.DepartmentID = Convert.ToInt32(dr["DepartmentID"]);
                    d.DepartmentName = dr["DepartmentName"].ToString();
                    Departments.Add(d);
                }
            }
            catch (Exception ex)
            {

            }
            return Departments;
        }

        
    }
}