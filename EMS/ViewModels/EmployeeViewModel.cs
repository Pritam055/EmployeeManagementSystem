using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.ViewModels
{
    public class EmployeeViewModel
    {   
        public  string Id { get; set; }

        [Required] 
        public string EmployeeName { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public float TotalSalary { get; set; }
        [Required]
        public float PF { get; set; }

  
    }
}
