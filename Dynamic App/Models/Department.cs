using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dynamic_App.Models
{
    public class Department
    {
        public Department()
        {
            DepartmentID = 0;
            DepartmentName = "";
        }

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
}