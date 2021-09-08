using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.ViewModels
{
    public class ProfileViewModel : ApplicationUser
    {
        public string EmployeeEmail { get; set; } 
        public double TotalTax { get; set; } 
    }
}
