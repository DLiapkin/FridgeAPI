using Moq;
using Xunit;
using Domain.Models;
using Services.Contracts;
using Services.Models;
using FridgeAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FridgeAPI.UnitTests
{
    public class FridgeModelsControllerTests
    {
        private readonly Mock<ILogger<FridgeModelsController>> loggerStub;
        private readonly Mock<IFridgeModelService> serviceStub;
        private readonly FridgeModelsController controller;

        public FridgeModelsControllerTests()
        {
            loggerStub = new Mock<ILogger<FridgeModelsController>>();
            serviceStub = new Mock<IFridgeModelService>();
            controller = new FridgeModelsController(loggerStub.Object, serviceStub.Object);
        }

        [Fact]
        public void GetFridgeModelById_UnknownId_ReturnsNotFound()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((FridgeModelResponse)null);

            // Act
            var result = controller.GetFridgeModelById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetFridgeModelById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            FridgeModel expected = CreateRandomFridgeModel();
            FridgeModelResponse expectedDto = new FridgeModelResponse()
            {
                Id = expected.Id,
                Name = expected.Name,
                Year = expected.Year,
            };
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(expectedDto);

            // Act
            var result = controller.GetFridgeModelById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetFridgeModelById_ExistingId_ReturnsRightFridgeModel()
        {
            // Arrange
            FridgeModel expected = CreateRandomFridgeModel();
            FridgeModelResponse expectedDto = new FridgeModelResponse()
            {
                Id = expected.Id,
                Name = expected.Name,
                Year = expected.Year
            };
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(expectedDto);

            // Act
            var result = controller.GetFridgeModelById(It.IsAny<Guid>());

            // Assert
            FridgeModelResponse dto = (FridgeModelResponse)(result.Result as OkObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void GetFridgeModels_ExistingFridgeModels_ReturnsAllFridgeModels()
        {
            // Arrange
            IEnumerable<FridgeModel> expected = new[] { CreateRandomFridgeModel(), CreateRandomFridgeModel(), CreateRandomFridgeModel() };
            List<FridgeModelResponse> expectedDto = new List<FridgeModelResponse>();
            foreach (var fridgeModel in expected)
            {
                expectedDto.Add(new FridgeModelResponse()
                {
                    Id = fridgeModel.Id,
                    Name = fridgeModel.Name,
                    Year = fridgeModel.Year
                });
            }
            serviceStub.Setup(repo => repo.GetAll()).ReturnsAsync(expectedDto);

            // Act
            var result = controller.GetFridgeModels();

            // Assert
            IEnumerable<FridgeModelResponse> dto = (IEnumerable<FridgeModelResponse>)(result.Result as OkObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void CreateFridgeModel_WithNullDto_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = controller.CreateFridgeModel(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void CreateFridgeModel_WithFridgeModelToCreate_ReturnsCreatedFridgeModel()
        {
            // Arrange
            FridgeModel fridgeModel = CreateRandomFridgeModel();
            FridgeModelResponse fridgeModelDto = new FridgeModelResponse()
            {
                Id = fridgeModel.Id,
                Name = fridgeModel.Name,
                Year = fridgeModel.Year
            };
            FridgeModelRequest FridgeModelToCreate = new FridgeModelRequest()
            {
                Name = fridgeModel.Name,
                Year = fridgeModel.Year
            };
            serviceStub.Setup(serv => serv.Create(It.IsAny<FridgeModelRequest>())).ReturnsAsync(fridgeModelDto);

            // Act
            var result = controller.CreateFridgeModel(FridgeModelToCreate);

            // Assert
            FridgeModelResponse createdFridgeModel = (FridgeModelResponse)(result.Result as CreatedAtActionResult).Value;
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
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFridgeModel_WithUnknownFridgeModel_ReturnsNotFound()
        {
            // Arrange
            FridgeModel fridgeModel = CreateRandomFridgeModel();
            FridgeModelRequest fridgeModelToUpdate = new FridgeModelRequest()
            {
                Name = fridgeModel.Name,
                Year = fridgeModel.Year,
            };
            serviceStub.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync((FridgeModelResponse)null);

            // Act
            var result = controller.UpdateFridgeModel(It.IsAny<Guid>(), fridgeModelToUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void UpdateFridgeModel_WithExistingFridgeModel_ReturnsNoContent()
        {
            // Arrange
            FridgeModel fridgeModel = CreateRandomFridgeModel();
            FridgeModelRequest fridgeModelToUpdate = new FridgeModelRequest()
            {
                Name = fridgeModel.Name + fridgeModel.Year.ToString(),
                Year = fridgeModel.Year,
            };
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(new FridgeModelResponse());
            serviceStub.Setup(serv => serv.Update(It.IsAny<Guid>(), fridgeModelToUpdate));

            // Act
            var result = controller.UpdateFridgeModel(fridgeModel.Id, fridgeModelToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void DeleteFridgeModel_WithUnknownFridgeModel_ReturnsNotFound()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((FridgeModelResponse)null);

            // Act
            var result = controller.DeleteFridgeModel(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void DeleteFridgeModel_WithExistingFridgeModel_ReturnsNoContent()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(new FridgeModelResponse());
            serviceStub.Setup(serv => serv.Delete(It.IsAny<Guid>()));

            // Act
            var result = controller.DeleteFridgeModel(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
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
