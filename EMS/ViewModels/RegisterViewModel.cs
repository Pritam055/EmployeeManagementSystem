using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password do not match.")]
        public string ConfirmPassword { get; set; }

        // added info for employee data
        [Display(Name="Employee Name")]
        [Required(ErrorMessage = "Employee name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot be more then 50 characters")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Employee address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Employee Total Salary is requrired")]
        public float TotalSalary { get; set; }

        [Display(Name= "PF (Provident Fund)")]
        [Required(ErrorMessage = "Provident Fund amount is required")]
        public float PF { get; set; }
         
      
        public IFormFile Photo { get; set; }
    }
}
