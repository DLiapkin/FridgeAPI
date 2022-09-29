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
                var fridge = _repository.Fridge.GetFridge(id, trackChanges: true);
                var fridgeDto = _mapper.Map<FridgeDto>(fridge);
                return Ok(fridgeDto);
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
