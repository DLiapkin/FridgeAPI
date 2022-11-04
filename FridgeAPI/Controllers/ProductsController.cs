using Services.Contracts;
using Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FridgeAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _service;

        public ProductsController(ILogger<ProductsController> logger, IProductService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                IEnumerable<ProductResponse> products = await _service.GetAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetProducts)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                ProductResponse product = await _service.GetById(id);
                if (product == null)
                {
                    _logger.LogInformation($"Product with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                else
                {
                    return Ok(product);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetProductById)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest productToCreate)
        {
            try
            {
                if (productToCreate == null)
                {
                    _logger.LogError("ProductRequest object sent from client is null.");
                    return BadRequest("ProductRequest is null");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        _logger.LogError("Invalid model state for the ProductRequest object");
                        return UnprocessableEntity(ModelState);
                    }
                    ProductResponse productToReturn = await _service.Create(productToCreate);
                    return CreatedAtAction(nameof(CreateProduct), new { id = productToReturn.Id }, productToReturn);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateProduct)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductRequest productToUpdate)
        {
            try
            {
                if (productToUpdate == null)
                {
                    _logger.LogError("ProductRequest object sent from client is null.");
                    return BadRequest("ProductRequest object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the ProductRequest object");
                    return UnprocessableEntity(ModelState);
                }
                if (await _service.GetById(id) == null)
                {
                    _logger.LogInformation($"Product with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                await _service.Update(id, productToUpdate);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateProduct)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                if (await _service.GetById(id) == null)
                {
                    _logger.LogInformation($"Product with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                await _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteProduct)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
