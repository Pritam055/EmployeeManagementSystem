using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Models
{
    public class IncomeTax
    {  
        public int Id { get; set; }
        public float TaxableAmount{get;set;}
        public float Rate { get; set; }
    }
}
