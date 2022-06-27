using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]  // aka api/v1/catalog
    public class CatalogController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;


        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]  // happy path return type
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();
            return Ok(products);  // we use Ok (and other actions) due to returning a wrapper ActionResult
        }
        
        
        [HttpGet("{id:length(24)}", Name = "GetProduct")] 
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] 
        [ProducesResponseType((int) HttpStatusCode.NotFound)]  // we list other error actions returned by this endpoint
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repository.GetProduct(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            return Ok(product);
        }


        [Route("Category/{category}", Name = "GetProductsByCategory")]  // action=GetProductsByCategory (from method name)        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)] 
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var products = await _repository.GetProductsByCategory(category);
            return Ok(products);
        }
        
        
        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] 
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);  // matches GetProductById given name (Name="GetProduct")
        }  

        
        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] 
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {  // IActionResult instead of ActionResult since we are not wrapping any returned object
            return Ok(await _repository.UpdateProduct(product));  // wrap returned boolean
        }


        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] 
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _repository.DeleteProduct(id));
        } 
    }
}
