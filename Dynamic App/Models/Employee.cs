using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dynamic_App.Models
{
    public class Employee
    {
        public Employee()
        {
            EmployeeID = 0;
            DepartmentID = 0;
            EmployeeGender = 0;
            EmployeeName = "";
            EmployeeEmail = "";
            ErrorMessage = "";
        }

        public int EmployeeID { get; set; }
        public int DepartmentID { get; set; }
        public int EmployeeGender { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeEmail { get; set; }
        public string ErrorMessage { get; set; }
    }
}