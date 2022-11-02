using Contracts.Services;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
        public IActionResult GetProducts()
        {
            try
            {
                IEnumerable<ProductDto> products = _service.GetAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetProducts)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(Guid id)
        {
            try
            {
                ProductDto product = _service.GetById(id);
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
        public IActionResult CreateProduct([FromBody] ProductToCreateDto productDto)
        {
            try
            {
                if (productDto == null)
                {
                    _logger.LogError("ProductToCreateDto object sent from client is null.");
                    return BadRequest("ProductToCreateDto is null");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        _logger.LogError("Invalid model state for the ProductToCreateDto object");
                        return UnprocessableEntity(ModelState);
                    }
                    ProductDto productToReturn = _service.Create(productDto);
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
        public IActionResult UpdateProduct(Guid id, [FromBody] ProductToUpdateDto product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogError("ProductToUpdateDto object sent from client is null.");
                    return BadRequest("ProductToUpdateDto object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the ProductToUpdateDto object");
                    return UnprocessableEntity(ModelState);
                }
                if (_service.GetById(id) == null)
                {
                    _logger.LogInformation($"Fridge with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                _service.Update(id, product);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateProduct)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            try
            {
                if (_service.GetById(id) == null)
                {
                    _logger.LogInformation($"Product with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                _service.Delete(id);
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
