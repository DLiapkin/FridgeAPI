using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Contracts;

namespace FridgeAPI.Controllers
{
    [Route("api/[controller]")]
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
                _logger.LogError($"Something went wrong in the {nameof(GetFridges)} action { ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
