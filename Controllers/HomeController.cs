using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AspMvcCRUD.Models;

namespace AspMvcCRUD.Controllers
{
    public class HomeController : Controller
    {

        List<Employee> _oEmployees = new List<Employee>();
        Employee _oEmployee = new Employee();

        string cString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=CRUD_MVC;Integrated Security=True";

        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Indexs()
        {
            DataSet ds = this.GetEmployees();
            ViewBag.empList = ds.Tables[0];
            return View();
        }


        public ActionResult EditEmp(int id)
        {
            
            List<Employee> es = new List<Employee>();
            Employee e2 = new Employee();
            DataSet ds = this.GetEmploueeByID(id);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Employee e = new Employee();
                e.EmployeeID = Convert.ToInt32(dr["EmployeeID"]);
                e.EmpName = dr["EmpName"].ToString();
                e.EmpEmail = dr["EmpEmail"].ToString();
                e.EmpDeptID = Convert.ToInt32(dr["EmpDeptID"]);
                e.EmpDept = dr["EmpDept"].ToString();
                e.EmpGender = Convert.ToInt32(dr["EmpGender"]);
                es.Add(e);
            }
            e2 = es.FirstOrDefault();
            ViewBag.empList = ds.Tables[0];
            return View(e2);
        }


        [HttpPost]
        public JsonResult Save(Employee oEmployee)
        {

            _oEmployee = new Employee();
            try
            {
                this.SaveEmployee(oEmployee);
            }
            catch (Exception ex)
            {
                oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Employee emp)
        {
            _oEmployee = new Employee();
            try
            {
                this.UpdateEmployee(emp);
            }
            catch (Exception ex)
            {
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            string sSQL = "Delete from Employee Where EmployeeID = "+id;
            conn = new SqlConnection(cString);
            cmd = new SqlCommand(sSQL,conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Indexs");
        }


        public bool UpdateEmployee(Employee e)
        {
            bool b = false;
            string _sQL = @" Update Employee SET EmpName='"+e.EmpName+"', EmpEmail='"+e.EmpEmail+"', EmpDeptID="+e.EmpDeptID+", EmpDept='"+e.EmpDept+ "', EmpGender = " + e.EmpGender+" Where EmployeeID = "+e.EmployeeID;
            conn = new SqlConnection(cString);
            cmd = new SqlCommand(_sQL, conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            if (cmd.ExecuteNonQuery() > 0)
            {
                b = true;
            }
            else
            {
                b = false;
            }

            conn.Close();
            
            return b;
        }


        public DataSet GetEmployees()
        {
            string sSQl = "Select * From Employee";
            conn = new SqlConnection(cString);
            cmd = new SqlCommand(sSQl, conn);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public DataSet GetEmploueeByID(int id)
        {
            string sSQl = "Select * from Employee Where EmployeeID = "+id;
            conn = new SqlConnection(cString);
            cmd = new SqlCommand(sSQl, conn);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public bool SaveEmployee(Employee e)
        {
            bool b = false;
            int bt = 0;
            try
            {

                String sSQL = @"Insert into Employee(EmpName, EmpEmail, EmpDeptID, EmpDept, EmpGender) values('"+e.EmpName.Trim()+"', '"+e.EmpEmail.Trim()+"', "+e.EmpDeptID+", '"+e.EmpDept+"', "+e.EmpGender+")";
                conn = new SqlConnection(cString);
                cmd = new SqlCommand(sSQL, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0) { b = true; }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                b = false;
            }

            return b;
        }
    }
}