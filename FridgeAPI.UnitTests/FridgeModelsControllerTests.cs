using Moq;
using Xunit;
using AutoMapper;
using Contracts;
using Entities.Models;
using Entities.DataTransferObjects;
using FridgeAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FridgeAPI.UnitTests
{
    public class FridgeModelsControllerTests
    {
        private readonly Mock<IRepositoryManager> repositoryStub;
        private readonly Mock<ILogger<FridgeModelsController>> loggerStub;
        private readonly Mock<IMapper> mapperStub;
        private readonly FridgeModelsController controller;

        public FridgeModelsControllerTests()
        {
            repositoryStub = new Mock<IRepositoryManager>();
            loggerStub = new Mock<ILogger<FridgeModelsController>>();
            mapperStub = new Mock<IMapper>();
            controller = new FridgeModelsController(loggerStub.Object, repositoryStub.Object, mapperStub.Object);
        }

        [Fact]
        public void GetFridgeModelById_UnknownId_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.FridgeModel.GetFridgeModel(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((FridgeModel)null);

            // Act
            var result = controller.GetFridgeModelById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetFridgeModelById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            FridgeModel expected = CreateRandomFridgeModel();
            FridgeModelDto expectedDto = new FridgeModelDto()
            {
                Id = expected.Id,
                Name = expected.Name,
                Year = expected.Year,
            };
            repositoryStub.Setup(repo => repo.FridgeModel.GetFridgeModel(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(expected);
            mapperStub.Setup(map => map.Map<FridgeModelDto>(expected)).Returns(expectedDto);

            // Act
            var result = controller.GetFridgeModelById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetFridgeModelById_ExistingId_ReturnsRightFridgeModel()
        {
            // Arrange
            FridgeModel expected = CreateRandomFridgeModel();
            FridgeModelDto expectedDto = new FridgeModelDto()
            {
                Id = expected.Id,
                Name = expected.Name,
                Year = expected.Year
            };
            repositoryStub.Setup(repo => repo.FridgeModel.GetFridgeModel(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(expected);
            mapperStub.Setup(map => map.Map<FridgeModelDto>(expected)).Returns(expectedDto);

            // Act
            var result = controller.GetFridgeModelById(It.IsAny<Guid>());

            // Assert
            FridgeModelDto dto = (FridgeModelDto)(result as ObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void GetFridgeModels_ExistingFridgeModels_ReturnsAllFridgeModels()
        {
            // Arrange
            IEnumerable<FridgeModel> expected = new[] { CreateRandomFridgeModel(), CreateRandomFridgeModel(), CreateRandomFridgeModel() };
            List<FridgeModelDto> expectedDto = new List<FridgeModelDto>();
            foreach (var fridgeModel in expected)
            {
                expectedDto.Add(new FridgeModelDto()
                {
                    Id = fridgeModel.Id,
                    Name = fridgeModel.Name,
                    Year = fridgeModel.Year
                });
            }
            repositoryStub.Setup(repo => repo.FridgeModel.GetAllFridgeModels(It.IsAny<bool>()))
                .Returns(expected);
            mapperStub.Setup(map => map.Map<IEnumerable<FridgeModelDto>>(expected)).Returns(expectedDto);

            // Act
            var result = controller.GetFridgeModels();

            // Assert
            IEnumerable<FridgeModelDto> dto = (IEnumerable<FridgeModelDto>)(result as ObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void CreateFridgeModel_WithNullDto_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = controller.CreateFridgeModel(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateFridgeModel_WithFridgeModelToCreate_ReturnsCreatedFridgeModel()
        {
            // Arrange
            FridgeModel fridgeModel = CreateRandomFridgeModel();
            FridgeModelDto fridgeModelDto = new FridgeModelDto()
            {
                Id = fridgeModel.Id,
                Name = fridgeModel.Name,
                Year = fridgeModel.Year
            };
            FridgeModelToCreateDto FridgeModelToCreate = new FridgeModelToCreateDto()
            {
                Name = fridgeModel.Name,
                Year = fridgeModel.Year
            };
            repositoryStub.Setup(repo => repo.FridgeModel.CreateFridgeModel(It.IsAny<FridgeModel>()));
            mapperStub.Setup(map => map.Map<FridgeModel>(FridgeModelToCreate)).Returns(fridgeModel);
            mapperStub.Setup(map => map.Map<FridgeModelDto>(fridgeModel)).Returns(fridgeModelDto);

            // Act
            var result = controller.CreateFridgeModel(FridgeModelToCreate);

            // Assert
            FridgeModelDto createdFridgeModel = (result as CreatedAtActionResult).Value as FridgeModelDto;
            Assert.Equal(fridgeModelDto, createdFridgeModel);
            Assert.NotEqual(createdFridgeModel.Id, Guid.Empty);
        }

        [Fact]
        public void UpdateFridgeModel_WithNullDto_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = controller.UpdateFridgeModel(It.IsAny<Guid>(), null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateFridgeModel_WithUnknownFridgeModel_ReturnsNotFound()
        {
            // Arrange
            FridgeModel fridgeModel = CreateRandomFridgeModel();
            FridgeModelToUpdateDto fridgeModelToUpdate = new FridgeModelToUpdateDto()
            {
                Name = fridgeModel.Name,
                Year = fridgeModel.Year,
            };
            repositoryStub
                .Setup(repo => repo.FridgeModel.GetFridgeModel(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((FridgeModel)null);

            // Act
            var result = controller.UpdateFridgeModel(It.IsAny<Guid>(), fridgeModelToUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateFridgeModel_WithExistingFridgeModel_ReturnsNoContent()
        {
            // Arrange
            FridgeModel fridgeModel = CreateRandomFridgeModel();
            FridgeModelToUpdateDto fridgeModelToUpdate = new FridgeModelToUpdateDto()
            {
                Name = fridgeModel.Name + fridgeModel.Year.ToString(),
                Year = fridgeModel.Year,
            };
            repositoryStub
                .Setup(repo => repo.FridgeModel.GetFridgeModel(fridgeModel.Id, It.IsAny<bool>()))
                .Returns(fridgeModel);
            repositoryStub.Setup(repo => repo.FridgeModel.UpdateFridgeModel(fridgeModel));
            mapperStub.Setup(map => map.Map(fridgeModel, fridgeModelToUpdate));

            // Act
            var result = controller.UpdateFridgeModel(fridgeModel.Id, fridgeModelToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteFridgeModel_WithUnknownFridgeModel_ReturnsNotFound()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.FridgeModel.GetFridgeModel(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((FridgeModel)null);

            // Act
            var result = controller.DeleteFridgeModel(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteFridgeModel_WithExistingFridgeModel_ReturnsNoContent()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.FridgeModel.GetFridgeModel(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(new FridgeModel());
            repositoryStub.Setup(repo => repo.FridgeModel.DeleteFridgeModel(It.IsAny<FridgeModel>()));

            // Act
            var result = controller.DeleteFridgeModel(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        private FridgeModel CreateRandomFridgeModel()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Year = DateTime.Now.Year + new Random().Next(-10, 5)
            };
        }
    }
}
