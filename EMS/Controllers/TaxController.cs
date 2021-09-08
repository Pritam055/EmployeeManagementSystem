using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Data;
using EMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EMS.Controllers
{
    [Authorize]
    public class TaxController : Controller
    {
        private readonly ADbContext dbcontext;
        public TaxController(ADbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        } 
        [HttpGet]
        public IActionResult GetTaxTable(bool isSuccess=false)
        {
            var incomeTaxList = dbcontext.IncomeTax.ToList();
            var incomeTaxViewList = new List<IncomeTaxViewModel>();

            foreach (var item in incomeTaxList)
            {
                var obj = new IncomeTaxViewModel
                {
                    Id = item.Id,
                    TaxableAmount = item.TaxableAmount,
                    Rate = item.Rate
                };
                incomeTaxViewList.Add(obj);
            }
            ViewBag.isSuccess = isSuccess;
            return View(incomeTaxViewList);
        }

        [HttpGet]
        public IActionResult EditIncomeTax(int taxId)
        {
            var taxModel = dbcontext.IncomeTax.FirstOrDefault(x => x.Id == taxId);
            if (taxModel != null)
            {
                var taxViewModel = new IncomeTaxViewModel
                {
                    Id = taxModel.Id,
                    TaxableAmount = taxModel.TaxableAmount,
                    Rate = taxModel.Rate
                };
                return View(taxViewModel);
            }
            return View();
        }

        [HttpPost]
        public IActionResult EditIncomeTax(IncomeTaxViewModel model)
        {
            var taxModel = dbcontext.IncomeTax.FirstOrDefault(x => x.Id == model.Id);
            if (taxModel != null)
            { 
                taxModel.Id = model.Id;
                taxModel.TaxableAmount = model.TaxableAmount;
                taxModel.Rate = model.Rate;

                dbcontext.SaveChanges();
                return RedirectToAction("GetTaxTable", "Tax", new { isSuccess = true } );
            }
            return View();
        }
    }
}
