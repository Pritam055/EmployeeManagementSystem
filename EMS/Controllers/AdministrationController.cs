using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    public class AdministrationController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AdministrationController(UserManager<ApplicationUser> userManager,
                                        RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole(model.RoleName);
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("CreateRole", "Administration");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);

            if (role == null)
            {
                return RedirectToAction("ListRoles", "Administration");
            } 
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            foreach(var user in userManager.Users)
            {
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
                 
            return View(model);
        }

        //EmployeeInRoleViewModel
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string Id)
        {
            ViewBag.roleId = Id;
            var role = await roleManager.FindByIdAsync(Id);
            if (role != null)
            {
                var roleEmployeeList = new List<EmployeeInRoleViewModel>();
                foreach(var employee in userManager.Users)
                {
                    var model = new EmployeeInRoleViewModel
                    {
                        EmployeeId = employee.Id,
                        EmployeeEmail = employee.Email,
                        EmployeeName = employee.EmployeeName
                    };
                    if(await userManager.IsInRoleAsync(employee, role.Name))
                    {
                        model.IsSelected = true;
                    }
                    else
                    {
                        model.IsSelected = false;
                    }

                    roleEmployeeList.Add(model);
                }
                return View(roleEmployeeList);
                
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<EmployeeInRoleViewModel> eRoleList, string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role != null)
            {
                foreach(var employee in eRoleList)
                {
                    var emp = await userManager.FindByIdAsync(employee.EmployeeId);
                    IdentityResult result = null;
                    if(employee.IsSelected && !(await userManager.IsInRoleAsync(emp, role.Name)))
                    {
                        result = await userManager.AddToRoleAsync(emp, role.Name);
                    }else if(!employee.IsSelected && await userManager.IsInRoleAsync(emp, role.Name))
                    {
                        result = await userManager.RemoveFromRoleAsync(emp, role.Name);
                    }
                    else
                    {
                        continue;
                    }   
                }
                return RedirectToAction("EditRole", "Administration", new { id = role.Id });
            } 
            return View();
        }
    }
}
