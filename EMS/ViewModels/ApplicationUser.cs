using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.ViewModels
{
    public class ApplicationUser: IdentityUser
    {  
        public string EmployeeName { get; set; }
        public string Address { get; set; } 
        public float TotalSalary { get; set; } 
        public float PF { get; set; } 
        public string Photo { get; set; }
    }
}
