using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHostingEnvironment hostingEnvironment;
        public EmployeeController(UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        } 
        // CRUD operations
        [HttpGet]
        public IActionResult ListEmployee(string searchedString)
        { 
            if (searchedString == null)
            {
                var user_list = userManager.Users;
                return View(user_list); 
            }
            else
            {
                var user_list = userManager.Users;
                var emp_list = user_list.Where(e => e.EmployeeName.Contains(searchedString) || e.Address.Contains(searchedString) || e.Email.Contains(searchedString));
                return View(emp_list);
            } 
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeById(string empId)
        { 
            var user =  await userManager.FindByIdAsync(empId);
            if (user != null)
            { 
                
                return View(user);
            }
            return View(user);
        }

      
         
        /*[HttpGet]
        public async Task<IActionResult> EditEmployee(string empId)
        {
            var user = await userManager.FindByIdAsync(empId);
            if (user != null)
            {
                var model = new EmployeeViewModel
                {
                    Id = user.Id,
                    EmployeeName = user.EmployeeName,
                    Address = user.Address,
                    Email = user.Email,
                    TotalSalary = user.TotalSalary,
                    PF = user.PF,

                };
                return View(model);
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.Email = model.Email;
                user.Address = model.Address;
                user.EmployeeName = model.EmployeeName;
                user.TotalSalary = model.TotalSalary;
                user.PF = model.PF;

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListEmployee", "Employee");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }  
            }
            return View(model);
        }
*/

        [HttpGet]
        public async Task<IActionResult> EditEmployee(string empId)
        {
            var employee1 = await userManager.FindByIdAsync(empId);
            if (employee1 != null)
            {
                var employee2 = new EditEmployeeViewModel
                {
                    Id = employee1.Id,
                    EmployeeName = employee1.EmployeeName,
                    Address = employee1.Address,
                    Email= employee1.Email,
                    TotalSalary = employee1.TotalSalary,
                    PF = employee1.PF,
                    ExistingPhotoPath = employee1.Photo,
                    Photo = null
                };
                
                 
                return View(employee2);
            }
            return View(employee1);
        
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await userManager.FindByIdAsync(model.Id);
                if (employee != null)
                {
                    employee.EmployeeName = model.EmployeeName;
                    employee.Address = model.Address;
                    employee.Email = model.Email;
                    employee.TotalSalary = model.TotalSalary;
                    employee.PF = model.PF;

                    string uniqueFileName = null;
                    if (model.Photo != null)
                    {
                        // if there is photo previously then we need to delete so
                        if (model.ExistingPhotoPath != null)
                        {
                            string filep = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                            System.IO.File.Delete(filep);
                        }
                        string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        model.Photo.CopyTo(fs);
                        fs.Close();
                    }

                    if(model.Photo==null && model.ExistingPhotoPath!=null)
                    {
                        uniqueFileName = model.ExistingPhotoPath;
                    }
                    employee.Photo = uniqueFileName;

                  /*  var updatedEmployee = new ApplicationUser
                    {
                        Id = employee.Id,
                        Email = employee.Email,
                        TotalSalary = employee.TotalSalary,
                        PF = employee.PF,
                        Address = model.Address,
                        EmployeeName = model.EmployeeName,
                        Photo = employee.Photo
                    };*/
                    var result = await userManager.UpdateAsync(employee);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListEmployee", "Employee");
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }  
                }
                return View(employee);
            }
           
            return View();
        }

        [HttpPost] 
        public  async Task<IActionResult> DeleteEmployee(string empId)
        {
            var user = await userManager.FindByIdAsync(empId);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListEmployee", "Employee");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

    }
}
