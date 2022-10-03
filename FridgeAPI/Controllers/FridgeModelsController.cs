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
    [Route("api/fridge-models")]
    [ApiController]
    public class FridgeModelsController : ControllerBase
    {
        private readonly ILogger<FridgeModelsController> _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public FridgeModelsController(ILogger<FridgeModelsController> logger, IRepositoryManager repositoryManager, IMapper mapper)
        {
            _logger = logger;
            _repository = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetFridgeModels()
        {
            try
            {
                var fridgeModels = _repository.FridgeModel.GetAllFridgeModels(trackChanges: true);
                var fridgeModelsDto = _mapper.Map<IEnumerable<FridgeModelDto>>(fridgeModels);
                return Ok(fridgeModelsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFridgeModels)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetFridgeModelById(Guid id)
        {
            try
            {
                var fridgeModel = _repository.FridgeModel.GetFridgeModel(id, trackChanges: false);
                if (fridgeModel == null)
                {
                    _logger.LogInformation($"FridgeModel with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                else
                {
                    var fridgeModelDto = _mapper.Map<FridgeModelDto>(fridgeModel);
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
        public IActionResult CreateFridgeModel([FromBody] FridgeModelToCreateDto fridgeModelToCreateDto)
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
                    FridgeModel fridgeModel = _mapper.Map<FridgeModel>(fridgeModelToCreateDto);
                    _repository.FridgeModel.CreateFridgeModel(fridgeModel);
                    _repository.Save();
                    var fridgeModelToReturn = _mapper.Map<FridgeModelDto>(fridgeModel);
                    return CreatedAtRoute(nameof(CreateFridgeModel), new { id = fridgeModelToReturn.Id }, fridgeModelToReturn);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateFridgeModel)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFridgeModel(Guid id, [FromBody] FridgeModelToUpdateDto fridgeModel)
        {
            try
            {
                if (fridgeModel == null)
                {
                    _logger.LogError("FridgeModelToUpdateDto object sent from client is null.");
                    return BadRequest("FridgeModelToUpdateDto object is null");
                }
                var fridgeModelEntity = _repository.FridgeModel.GetFridgeModel(id, trackChanges: false);
                if (fridgeModelEntity == null)
                {
                    _logger.LogInformation($"FridgeModel with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the FridgeModelToUpdateDto object");
                    return UnprocessableEntity(ModelState);
                }
                _mapper.Map(fridgeModel, fridgeModelEntity);
                _repository.FridgeModel.UpdateFridgeModel(fridgeModelEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateFridgeModel)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFridgeModel(Guid id)
        {
            try
            {
                var fridgeModel = _repository.FridgeModel.GetFridgeModel(id, trackChanges: false);
                if (fridgeModel == null)
                {
                    _logger.LogInformation($"FridgeModel with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                _repository.FridgeModel.DeleteFridgeModel(fridgeModel);
                _repository.Save();
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
