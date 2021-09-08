using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.ViewModels
{
    public class IncomeTaxViewModel
    { 
        public int Id { get; set; }
        [Required]
        public float TaxableAmount { get; set; }
        [Required]
        public float Rate { get; set; }
    }
}
