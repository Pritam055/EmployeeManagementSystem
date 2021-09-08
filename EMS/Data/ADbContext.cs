using EMS.Models;
using EMS.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Data
{
    public class ADbContext: IdentityDbContext<ApplicationUser>
    {
        public ADbContext(DbContextOptions<ADbContext> options):base(options) {  }
      // public DbSet<Employee> Employee { get; set; }
        public DbSet<IncomeTax> IncomeTax { get; set; } 
    }
}
