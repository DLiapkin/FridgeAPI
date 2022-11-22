using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Contracts;
using Services.Models;
using System.Threading.Tasks;

namespace FridgeAPI.Controllers
{
    [Route("api/fridges")]
    [ApiController]
    public class FridgesController : ControllerBase
    {
        private readonly ILogger<FridgesController> _logger;
        private readonly IFridgeService _service;

        public FridgesController(ILogger<FridgesController> logger, IFridgeService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetFridges()
        {
            try
            {
                IEnumerable<FridgeResponse> fridgesDto = await _service.GetAll();
                return Ok(fridgesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFridges)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFridgeById(Guid id)
        {
            try
            {
                FridgeResponse fridgeDto = await _service.GetById(id);
                if (fridgeDto == null)
                {
                    _logger.LogInformation($"Fridge with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                return Ok(fridgeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFridgeById)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFridge([FromBody] FridgeRequest fridgeToCreate)
        {
            try
            {
                if (fridgeToCreate == null)
                {
                    _logger.LogError("FridgeRequest object sent from client is null.");
                    return BadRequest("FridgeRequest is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the FridgeRequest object");
                    return UnprocessableEntity(ModelState);
                }
                FridgeResponse fridgeToReturn = await _service.Create(fridgeToCreate);
                return CreatedAtAction(nameof(CreateFridge), new { id = fridgeToReturn.Id }, fridgeToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFridge(Guid id, [FromBody] FridgeRequest fridgeToUpdate)
        {
            try 
            {
                if (fridgeToUpdate == null)
                {
                    _logger.LogError("FridgeRequest object sent from client is null.");
                    return BadRequest("FridgeRequest object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the FridgeRequest object");
                    return UnprocessableEntity(ModelState);
                }
                if (await _service.GetById(id) == null)
                {
                    _logger.LogInformation($"Fridge with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                await _service.Update(id, fridgeToUpdate);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFridge(Guid id)
        {
            try 
            {
                if (await _service.GetById(id) == null)
                {
                    _logger.LogInformation($"Fridge with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                await _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{fridgeId}/products")]
        public async Task<IActionResult> GetFridgeProductsById(Guid fridgeId)
        {
            try
            {
                if (await _service.GetById(fridgeId) == null)
                {
                    _logger.LogInformation($"Fridge with id: {fridgeId} doesn't exist in the database.");
                    return NotFound();
                }
                IEnumerable<FridgeProductResponse> productsDto = await _service.GetProducts(fridgeId);
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFridgeProductsById)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{fridgeId}/products")]
        public async Task<IActionResult> CreateProductForFridge(Guid fridgeId, [FromBody] FridgeProductRequest productToCreate)
        {
            try
            {
                if (await _service.GetById(fridgeId) == null)
                {
                    _logger.LogInformation($"Fridge with id: {fridgeId} doesn't exist in the database.");
                    return NotFound();
                }
                if (productToCreate == null)
                {
                    _logger.LogError("FridgeProductRequest object sent from client is null.");
                    return BadRequest("FridgeProductRequest is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the FridgeProductRequest object");
                    return UnprocessableEntity(ModelState);
                }
                FridgeProductResponse productToReturn = await _service.CreateProduct(fridgeId, productToCreate);
                return CreatedAtAction(nameof(CreateProductForFridge), 
                    new { fridgeId, id = productToReturn.Id }, productToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateProductForFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{fridgeId}/products/{fridgeProductId}")]
        public async Task<IActionResult> DeleteProductInFridge(Guid fridgeId, Guid fridgeProductId)
        {
            try
            {
                if (await _service.GetById(fridgeId) == null)
                {
                    _logger.LogInformation($"Fridge with id: {fridgeId} doesn't exist in the database.");
                    return NotFound();
                }
                if (await _service.GetProductById(fridgeProductId) == null)
                {
                    _logger.LogInformation($"There is no such a product with id: {fridgeProductId}.");
                    return NotFound();
                }
                await _service.DeleteProduct(fridgeProductId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteProductInFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("refresh-product")]
        public async Task<IActionResult> RefreshProduct()
        {
            try
            {
                await _service.RefreshProduct();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(RefreshProduct)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
