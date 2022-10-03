using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ProductsController(ILogger<ProductsController> logger, IRepositoryManager repositoryManager, IMapper mapper)
        {
            _logger = logger;
            _repository = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _repository.Product.GetAllProducts(trackChanges: true);
                var productDto = _mapper.Map<IEnumerable<ProductDto>>(products);
                return Ok(productDto);
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
                var product = _repository.Product.GetProduct(id, trackChanges: false);
                if (product == null)
                {
                    _logger.LogInformation($"Product with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                else
                {
                    var productDto = _mapper.Map<ProductDto>(product);
                    return Ok(productDto);
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
                    Product product = _mapper.Map<Product>(productDto);
                    _repository.Product.CreateProduct(product);
                    _repository.Save();
                    var productToReturn = _mapper.Map<ProductDto>(product);
                    return CreatedAtRoute(nameof(CreateProduct), new { id = productToReturn.Id }, productToReturn);
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
                var productEntity = _repository.Product.GetProduct(id, trackChanges: false);
                if (productEntity == null)
                {
                    _logger.LogInformation($"Fridge with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the ProductToCreateDto object");
                    return UnprocessableEntity(ModelState);
                }
                _mapper.Map(product, productEntity);
                _repository.Product.UpdateProduct(productEntity);
                _repository.Save();
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
                var product = _repository.Product.GetProduct(id, trackChanges: false);
                if (product == null)
                {
                    _logger.LogInformation($"Product with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                _repository.Product.DeleteProduct(product);
                _repository.Save();
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
