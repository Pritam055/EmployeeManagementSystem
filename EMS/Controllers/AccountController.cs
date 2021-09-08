using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EMS.Data;
using EMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{

    //  password: Hello@12
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private ADbContext dbcontext;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHostingEnvironment hostingEnvironment;
        public AccountController(ADbContext dbcontext, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IHostingEnvironment hostingEnvironment) 
        {
            this.dbcontext = dbcontext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet] 
        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        } 
        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo!=null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    model.Photo.CopyTo(fs);
                    fs.Close();
                }
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    TotalSalary = model.TotalSalary,
                    PF = model.PF,
                    Address = model.Address,
                    EmployeeName = model.EmployeeName,
                    Photo = uniqueFileName
                };
                var result =await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Employee");
                    // this is for user to register he/she is registered
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("ListEmployee", "Employee");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RemeberMe, false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(model.Email);
                    HttpContext.Session.SetString("UserId",user.Id);
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string empId)
        {
            // hello
            var Id = HttpContext.Session.GetString("UserId");
            var user = await userManager.FindByIdAsync(empId);
            if (user == null)
            {
                return RedirectToAction("index", "home");
            }


            var employee = new ProfileViewModel
            {
                Id = user.Id,
                EmployeeName = user.EmployeeName,
                EmployeeEmail = user.Email,
                Address = user.Address,
                TotalSalary = user.TotalSalary,
                PF = user.PF,
                Photo = user.Photo
            };
            employee.TotalTax = EmployeeTaxCalculation(employee.TotalSalary-employee.PF);
           
            return View(employee); 
        }

        // ---------------- Tax Calucation part------------------- 
        public double EmployeeTaxCalculation(double salary)
        {
            // pf does not have tax 
            double x = 0.0, tax = 0.0;
            int count = 1;
            while (salary > 0)
            {
                if (count <= 3)
                {
                    if (salary > 100000)
                    {
                        salary = salary - 100000;
                        x = 100000;
                    }
                    else
                    {
                        x = salary;
                        salary = 0;
                    }
                    tax = tax + Tax_func(count, x);
                    count += 1;
                }
                else
                {
                    tax = tax + Tax_func(count,x) ;
                    break;
                }
            }
            return Math.Round(tax,2);
        }
        public double Tax_func(int count, double salary)
        {
            var tax_object = dbcontext.IncomeTax.ToList();
            var tax1 = tax_object.SingleOrDefault(x => x.Id == 1);
            var tax2 = tax_object.SingleOrDefault(x => x.Id == 2);
            var tax3 = tax_object.SingleOrDefault(x => x.Id == 3);
            var tax4 = tax_object.SingleOrDefault(x => x.Id == 4);

            if (count == 1)
                return (tax1.Rate/100)* salary;
            if (count == 2)
                return (tax2.Rate/100) * salary;
            if (count == 3)
                return (tax3.Rate/100)* salary;
            if (count == 4)
                return (tax4.Rate / 100) * salary;
            return 0;
        }
    }
}
