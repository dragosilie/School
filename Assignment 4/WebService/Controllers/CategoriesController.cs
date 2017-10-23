using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityMapping;        //Our own dataservice as a library
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace WebService.Controllers
{
    [Route("/api/categories")]
    public class CategoriesController : Controller
    {
        private IDataService _dataService;

        public CategoriesController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            return null;
        }

        [HttpPost]

        public IActionResult Post()
        {
            return null;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id)
        {
            return null;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return null;
        }
    }
}