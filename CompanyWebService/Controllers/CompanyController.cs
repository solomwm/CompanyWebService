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
    [Route("company")]
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
                dataEngine = new CompanyesDataEngine(dataFile)
                {
                    Companyes = baseCompanyes.ToList<Company>()
                };
            }
        }

        public IActionResult Index()
        {
            return Redirect("GetAll");
        }

        public IEnumerable<Company> GetAll()
        {
            return dataEngine.Companyes;
        }

        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var company = dataEngine.GetCompany(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [Route("getbyinn/{inn}")]
        public IActionResult GetByInn(string inn)
        {
            var company = dataEngine.GetByInn(inn);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [Route("getbyname/{name}")]
        public IActionResult GetByName(string name)
        {
            var companyES = dataEngine.GetByName(name);
            if (companyES == null || companyES.Count() == 0)
            {
                return NotFound();
            }
            return Ok(companyES);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return Redirect(@"Get\" + id);
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            return null;
        }
    }
}
