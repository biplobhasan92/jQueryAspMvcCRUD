using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspMvcCRUD.Models
{
    public class Employee
    {
        public Employee()
        {
            EmployeeID = 0;
            EmpName = "";
            EmpEmail = "";
            EmpDeptID = 0;
            EmpDept = "";
            EmpGender = 0;
            ErrorMessage = "";
        }




        public int EmployeeID { get; set;}
        public string EmpName { get; set;}
        public string EmpEmail { get; set;}
        public int EmpDeptID { get; set; }
        public string EmpDept { get; set;}
        public string ErrorMessage { get; set;}
        public int EmpGender { get; set;}

        public string EmpGenderName
        {
            get
            {
                string s = "";
                if (this.EmpGender == 0)
                {
                    s = "Male";
                }
                else
                {
                    s = "Female";
                }
                return s;
            }
        }

    }
}