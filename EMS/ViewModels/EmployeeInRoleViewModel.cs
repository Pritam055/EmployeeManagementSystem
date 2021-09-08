using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.ViewModels
{
    public class EmployeeInRoleViewModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public bool IsSelected { get; set; }
    }
}
