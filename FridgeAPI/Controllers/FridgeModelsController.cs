using Contracts.Services;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FridgeAPI.Controllers
{
    [Route("api/fridge-models")]
    [ApiController]
    public class FridgeModelsController : ControllerBase
    {
        private readonly ILogger<FridgeModelsController> _logger;
        private readonly IFridgeModelService _service;

        public FridgeModelsController(ILogger<FridgeModelsController> logger, IFridgeModelService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetFridgeModels()
        {
            try
            {
                IEnumerable<FridgeModelDto> fridgeModelsDto = await _service.GetAll();
                return Ok(fridgeModelsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFridgeModels)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFridgeModelById(Guid id)
        {
            try
            {
                var fridgeModelDto = await _service.GetById(id);
                if (fridgeModelDto == null)
                {
                    _logger.LogInformation($"FridgeModel with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                else
                {
                    return Ok(fridgeModelDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFridgeModelById)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFridgeModel([FromBody] FridgeModelToCreateDto fridgeModelToCreateDto)
        {
            try
            {
                if (fridgeModelToCreateDto == null)
                {
                    _logger.LogError("FridgeModelToCreateDto object sent from client is null.");
                    return BadRequest("FridgeModelToCreateDto is null");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        _logger.LogError("Invalid model state for the FridgeModelToCreateDto object");
                        return UnprocessableEntity(ModelState);
                    }
                    FridgeModelDto fridgeModelToReturn = await _service.Create(fridgeModelToCreateDto);
                    return CreatedAtAction(nameof(CreateFridgeModel), new { id = fridgeModelToReturn.Id }, fridgeModelToReturn);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateFridgeModel)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFridgeModel(Guid id, [FromBody] FridgeModelToUpdateDto fridgeModelToUpdate)
        {
            try
            {
                if (fridgeModelToUpdate == null)
                {
                    _logger.LogError("FridgeModelToUpdateDto object sent from client is null.");
                    return BadRequest("FridgeModelToUpdateDto object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the FridgeModelToUpdateDto object");
                    return UnprocessableEntity(ModelState);
                }
                if (await _service.GetById(id) == null)
                {
                    _logger.LogInformation($"FridgeModel with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                await _service.Update(id, fridgeModelToUpdate);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateFridgeModel)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFridgeModel(Guid id)
        {
            try
            {
                if (await _service.GetById(id) == null)
                {
                    _logger.LogInformation($"FridgeModel with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                await _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteFridgeModel)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
