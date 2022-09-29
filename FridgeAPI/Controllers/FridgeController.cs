using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Contracts;
using Entities.Models;

namespace FridgeAPI.Controllers
{
    [Route("api/fridges")]
    [ApiController]
    public class FridgeController : ControllerBase
    {
        private readonly ILogger<FridgeController> _logger;
        private readonly IRepositoryManager _repository;

        public FridgeController(ILogger<FridgeController> logger, IRepositoryManager repositoryManager)
        {
            _logger = logger;
            _repository = repositoryManager;
        }

        [HttpGet]
        public IActionResult GetFridges()
        {
            try
            {
                var fridges = _repository.Fridge.GetAllFridges(trackChanges: true);
                return Ok(fridges);
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
                var fridge = _repository.Fridge.GetFridge(id, trackChanges: true);
                return Ok(fridge);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFridgeById)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateFridge([FromBody] Fridge fridge)
        {
            try
            {
                _repository.Fridge.CreateFridge(fridge);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateFridge)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
