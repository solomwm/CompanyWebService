using INNService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace INNService.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private Company[] baseCompanyes = {new Company {Id = 1, INN = "0000000000", Name = "OOO FuckeL" },
            new Company {Id = 2, INN = "0000000001", Name = "OOO OneMore, OneMore..."} };
        private CompanyesDataEngine dataEngine;

        public CompanyController()
        {
            var dataFile = Directory.GetCurrentDirectory() + @"\wwwroot\CompanyesDB.dat";
            if (System.IO.File.Exists(dataFile))
            {
                dataEngine = new CompanyesDataEngine(dataFile);
            }
            else
            {
                dataEngine = new CompanyesDataEngine()
                {
                    Companyes = baseCompanyes.ToList<Company>(),
                };
                dataEngine.SaveData();
            }
        }

        [HttpGet]
        public IEnumerable<Company> GetAll()
        {
            return dataEngine.Companyes;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var company = dataEngine.GetCompany(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpGet("getbyinn/{inn}")]
        public IActionResult GetByInn(string inn)
        {
            var company = dataEngine.GetByInn(inn);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpGet("getbyname/{name}")]
        public IActionResult GetByName(string name)
        {
            var companyES = dataEngine.GetByName(name);
            if (companyES == null || companyES.Count() == 0)
            {
                return NotFound();
            }
            return Ok(companyES);
        }

        [HttpGet("edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            Company model;
            if (id.HasValue)
            {
                model = dataEngine.GetCompany(id.Value);
                if (model == null) return NotFound();
            }
            else model = new Company();
            return Ok(model);
        }

        [HttpPut("edit/{id?}")]
        public IActionResult Edit(Company model)
        {
            dataEngine.EditOrCreate(model);
            return RedirectToAction(nameof(model.Id));
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            dataEngine.Delete(id);
            return RedirectToAction(nameof(GetAll));
        }
    }
}
