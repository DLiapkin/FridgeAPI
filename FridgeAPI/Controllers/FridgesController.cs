using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Contracts;
using Entities.Models;
using Entities.DataTransferObjects;
using System.Linq;

namespace FridgeAPI.Controllers
{
    [Route("api/fridges")]
    [ApiController]
    public class FridgesController : ControllerBase
    {
        private readonly ILogger<FridgesController> _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public FridgesController(ILogger<FridgesController> logger, IRepositoryManager repositoryManager, IMapper mapper)
        {
            _logger = logger;
            _repository = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetFridges()
        {
            try
            {
                var fridges = _repository.Fridge.GetAllFridges(trackChanges: true);
                var fridgesDto = _mapper.Map<IEnumerable<FridgeDto>>(fridges);
                return Ok(fridgesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFridges)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetFridgeById(Guid id)
        {
            try
            {
                var fridge = _repository.Fridge.GetFridge(id, trackChanges: false);
                if (fridge == null)
                {
                    _logger.LogInformation($"Fridge with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                else
                {
                    var fridgeDto = _mapper.Map<FridgeDto>(fridge);
                    return Ok(fridgeDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFridgeById)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateFridge([FromBody] FridgeToCreateDto fridgeDto)
        {
            try
            {
                if (fridgeDto == null)
                {
                    _logger.LogError("FridgeToCreateDto object sent from client is null.");
                    return BadRequest("FridgeToCreateDto is null");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        _logger.LogError("Invalid model state for the ProductToCreateDto object");
                        return UnprocessableEntity(ModelState);
                    }
                    Fridge fridge = _mapper.Map<Fridge>(fridgeDto);
                    _repository.Fridge.CreateFridge(fridge);
                    _repository.Save();
                    var fridgeToReturn = _mapper.Map<FridgeDto>(fridge);
                    return CreatedAtAction(nameof(CreateFridge), new { id = fridgeToReturn.Id }, fridgeToReturn);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFridge(Guid id, [FromBody] FridgeToUpdateDto fridge)
        {
            try 
            {
                if (fridge == null)
                {
                    _logger.LogError("FridgeToUpdateDto object sent from client is null.");
                    return BadRequest("FridgeToUpdateDto object is null");
                }
                var fridgeEntity = _repository.Fridge.GetFridge(id, trackChanges: false);
                if (fridgeEntity == null)
                {
                    _logger.LogInformation($"Fridge with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the ProductToCreateDto object");
                    return UnprocessableEntity(ModelState);
                }
                _mapper.Map(fridge, fridgeEntity);
                _repository.Fridge.UpdateFridge(fridgeEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFridge(Guid id)
        {
            try 
            {
                var fridge = _repository.Fridge.GetFridge(id, trackChanges: false);
                if (fridge == null)
                {
                    _logger.LogInformation($"Fridge with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                _repository.Fridge.DeleteFridge(fridge);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{fridgeId}/products")]
        public IActionResult GetFridgeProductsById(Guid fridgeId)
        {
            try
            {
                var fridge = _repository.Fridge.GetFridge(fridgeId, trackChanges: true);
                if (fridge == null)
                {
                    _logger.LogInformation($"Fridge with id: {fridgeId} doesn't exist in the database.");
                    return NotFound();
                }
                var products = fridge.Products.ToList();
                if (!products.Any())
                {
                    _logger.LogInformation($"There isn't any products in fridge with id: {fridgeId}.");
                    return NotFound();
                }
                var productsDto = _mapper.Map<IEnumerable<FridgeProductDto>>(products);
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFridgeProductsById)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{fridgeId}/products")]
        public IActionResult CreateProductForFridge(Guid fridgeId, [FromBody] FridgeProductToCreateDto productToCreateDto)
        {
            try
            {
                var fridge = _repository.Fridge.GetFridge(fridgeId, trackChanges: true);
                if (fridge == null)
                {
                    _logger.LogInformation($"Fridge with id: {fridgeId} doesn't exist in the database.");
                    return NotFound();
                }
                if (productToCreateDto == null)
                {
                    _logger.LogError("ProductToCreateDto object sent from client is null.");
                    return BadRequest("ProductToCreateDto is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the ProductToCreateDto object");
                    return UnprocessableEntity(ModelState);
                }
                var product = _mapper.Map<FridgeProduct>(productToCreateDto);
                product.FridgeId = fridgeId;
                _repository.FridgeProduct.CreateFridgeProduct(product);
                _repository.Save();
                var productToReturn = _mapper.Map<FridgeProductDto>(product);
                return CreatedAtAction(nameof(CreateProductForFridge), 
                    new { fridgeId = fridgeId, id = productToReturn.Id }, productToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateProductForFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{fridgeId}/products/{fridgeProductId}")]
        public IActionResult DeleteProductInFridge(Guid fridgeId, Guid fridgeProductId)
        {
            try
            {
                var fridge = _repository.Fridge.GetFridge(fridgeId, trackChanges: true);
                if (fridge == null)
                {
                    _logger.LogInformation($"Fridge with id: {fridgeId} doesn't exist in the database.");
                    return NotFound();
                }
                var product = _repository.FridgeProduct.GetFridgeProduct(fridgeProductId, trackChanges: false);
                if (product == null)
                {
                    _logger.LogInformation($"There is no such a product with id: {fridgeProductId}.");
                    return NotFound();
                }
                _repository.FridgeProduct.DeleteFridgeProduct(product);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteProductInFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("refresh-product")]
        public IActionResult RefreshProduct()
        {
            try
            {
                string StoredProc = "EXEC usp_ReplenishProduct";
                _repository.FridgeProduct.ExcecuteProcedure(StoredProc);
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
