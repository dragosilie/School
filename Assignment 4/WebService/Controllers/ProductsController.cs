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
    [Route("/api/products")]
    public class ProductsController : Controller
    {
        private IDataService _dataService;

        public ProductsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var products = _dataService.GetProduct(id);
            if (products == null) return NotFound();
            return Ok(products);
        }

        [HttpGet]
        [Route("")]
        [Route("name/{substring}")]


        public IActionResult GetProductSubstringName(string substring)
        {
            var products = _dataService.GetProductByName(substring);
            if (!products.Any()) return NotFound(products);
            return Ok(products);
        }

        [HttpGet]
        [Route("")]
        [Route("category/{id}")]
        public IActionResult GetProductCategory(int id)
        {
            var products = _dataService.GetProductByCategory(id);
            if (!products.Any()) return NotFound(products);
            return Ok(products);
        }
    }
}