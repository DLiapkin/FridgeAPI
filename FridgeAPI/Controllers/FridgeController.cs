using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Contracts;
using Entities.Models;
using Entities.DataTransferObjects;

namespace FridgeAPI.Controllers
{
    [Route("api/fridges")]
    [ApiController]
    public class FridgeController : ControllerBase
    {
        private readonly ILogger<FridgeController> _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public FridgeController(ILogger<FridgeController> logger, IRepositoryManager repositoryManager, IMapper mapper)
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
                if(fridgeDto == null)
                {
                    _logger.LogError("FridgeToCreateDto object sent from client is null.");
                    return BadRequest("FridgeToCreateDto is null");
                }
                else
                {
                    Fridge fridge = _mapper.Map<Fridge>(fridgeDto);
                    _repository.Fridge.CreateFridge(fridge);
                    _repository.Save();
                    return Ok();
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
                    _logger.LogError("fridgeToUpdateDto object sent from client is null.");
                    return BadRequest("fridgeToUpdateDto object is null");
                }
                var fridgeEntity = _repository.Fridge.GetFridge(id, trackChanges: false);
                if (fridgeEntity == null)
                {
                    _logger.LogInformation($"Fridge with id: {id} doesn't exist in the database.");
                    return NotFound();
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
    }
}
