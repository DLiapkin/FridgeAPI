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
using System.Linq;
using System.Collections.Generic;

namespace FridgeAPI.UnitTests
{
    public class FridgesControllerTests
    {
        private readonly Mock<IRepositoryManager> repositoryStub;
        private readonly Mock<ILogger<FridgesController>> loggerStub;
        private readonly Mock<IMapper> mapperStub;
        private readonly FridgesController controller;

        public FridgesControllerTests()
        {
            repositoryStub = new Mock<IRepositoryManager>();
            loggerStub = new Mock<ILogger<FridgesController>>();
            mapperStub = new Mock<IMapper>();
            controller = new FridgesController(loggerStub.Object, repositoryStub.Object, mapperStub.Object);
        }

        [Fact]
        public void GetFridgeById_UnknownId_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((Fridge)null);

            // Act
            var result = controller.GetFridgeById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetFridgeById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            Fridge expected = CreateRandomFridge();
            FridgeDto expectedDto = CreateDtoFromFridge(expected);
            repositoryStub.Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(expected);
            mapperStub.Setup(map => map.Map<FridgeDto>(expected)).Returns(expectedDto);

            // Act
            var result = controller.GetFridgeById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetFridgeById_ExistingId_ReturnsRightFridge()
        {
            // Arrange
            Fridge expected = CreateRandomFridge();
            FridgeDto expectedDto = CreateDtoFromFridge(expected);
            repositoryStub.Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(expected);
            mapperStub.Setup(map => map.Map<FridgeDto>(expected)).Returns(expectedDto);

            // Act
            var result = controller.GetFridgeById(It.IsAny<Guid>());

            // Assert
            FridgeDto dto = (FridgeDto)(result as ObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void GetFridges_ExistingFridges_ReturnsAllFridges()
        {
            // Arrange
            IEnumerable<Fridge> expected = new[] { CreateRandomFridge(), CreateRandomFridge(), CreateRandomFridge() };
            List<FridgeDto> expectedDto = new List<FridgeDto>();
            foreach(var fridge in expected)
            {
                expectedDto.Add(CreateDtoFromFridge(fridge));
            }
            repositoryStub.Setup(repo => repo.Fridge.GetAllFridges(It.IsAny<bool>()))
                .Returns(expected);
            mapperStub.Setup(map => map.Map<IEnumerable<FridgeDto>>(expected)).Returns(expectedDto);

            // Act
            var result = controller.GetFridges();

            // Assert
            IEnumerable<FridgeDto> dto = (IEnumerable<FridgeDto>)(result as ObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void CreateFridge_WithNullDto_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = controller.CreateFridge(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateFridge_WithFridgeToCreate_ReturnsCreatedFridge()
        {
            // Arrange
            Fridge fridge = CreateRandomFridge();
            FridgeDto fridgeDto = CreateDtoFromFridge(fridge);
            FridgeToCreateDto fridgeToCreate = new FridgeToCreateDto()
            {
                Name = fridge.Name,
                ModelId = fridge.ModelId,
                OwnerName = fridge.OwnerName,
            };
            repositoryStub.Setup(repo => repo.Fridge.CreateFridge(It.IsAny<Fridge>()));
            mapperStub.Setup(map => map.Map<Fridge>(fridgeToCreate)).Returns(fridge);
            mapperStub.Setup(map => map.Map<FridgeDto>(fridge)).Returns(fridgeDto);

            // Act
            var result = controller.CreateFridge(fridgeToCreate);

            // Assert
            FridgeDto createdFridge = (result as CreatedAtActionResult).Value as FridgeDto;
            Assert.Equal(fridgeDto, createdFridge);
            Assert.NotEqual(createdFridge.Id, Guid.Empty);
        }

        [Fact]
        public void UpdateFridge_WithNullDto_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = controller.UpdateFridge(It.IsAny<Guid>(), null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateFridge_WithUnknownFridge_ReturnsNotFound()
        {
            // Arrange
            Fridge fridge = CreateRandomFridge();
            FridgeToUpdateDto fridgeToUpdate = new FridgeToUpdateDto()
            {
                Name = fridge.Name,
                ModelId = fridge.ModelId,
                OwnerName = fridge.OwnerName,
            };
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((Fridge)null);

            // Act
            var result = controller.UpdateFridge(It.IsAny<Guid>(), fridgeToUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateFridge_WithExistingFridge_ReturnsNoContent()
        {
            // Arrange
            Fridge fridge = CreateRandomFridge();
            FridgeToUpdateDto fridgeToUpdate = new FridgeToUpdateDto()
            {
                Name = fridge.Name + fridge.ModelId.ToString(),
                ModelId = fridge.ModelId,
                OwnerName = fridge.OwnerName,
            };
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(fridge.Id, It.IsAny<bool>()))
                .Returns(fridge);
            repositoryStub.Setup(repo => repo.Fridge.UpdateFridge(fridge));
            mapperStub.Setup(map => map.Map(fridge, fridgeToUpdate));

            // Act
            var result = controller.UpdateFridge(fridge.Id, fridgeToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteFridge_WithUnknownFridge_ReturnsNotFound()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((Fridge)null);

            // Act
            var result = controller.DeleteFridge(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteFridge_WithExistingFridge_ReturnsNoContent()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(new Fridge());
            repositoryStub.Setup(repo => repo.Fridge.DeleteFridge(It.IsAny<Fridge>()));

            // Act
            var result = controller.DeleteFridge(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void GetFridgeProductsById_WithUnknownFridge_ReturnsNotFound()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((Fridge)null);

            // Act
            var result = controller.GetFridgeProductsById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetFridgeProductsById_WithEmptyProducts_ReturnsNotFound()
        {
            // Arrange
            Fridge fridge = CreateRandomFridge();
            fridge.Products = new List<FridgeProduct>();
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(fridge);

            // Act
            var result = controller.GetFridgeProductsById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetFridgeProductsById_WithExistingFridgeAndProducts_ReturnsAllProducts()
        {
            // Arrange
            Fridge fridge = CreateRandomFridge();
            ICollection<FridgeProduct> products = new[] { CreateRandomFridgeProduct(), CreateRandomFridgeProduct() };
            List<FridgeProductDto> expectedProducts = new List<FridgeProductDto>();
            foreach(var product in products)
            {
                expectedProducts.Add(new FridgeProductDto() 
                { 
                    Id = product.Id,
                    ProductName = Guid.NewGuid().ToString(),
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                });
            }
            fridge.Products = products;
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(fridge);
            mapperStub.Setup(map => map.Map<IEnumerable<FridgeProductDto>>(fridge.Products)).Returns(expectedProducts);

            // Act
            var result = controller.GetFridgeProductsById(It.IsAny<Guid>());

            // Assert
            IEnumerable<FridgeProductDto> dto = (IEnumerable<FridgeProductDto>)(result as ObjectResult).Value;
            Assert.Equal(expectedProducts, dto);
        }

        [Fact]
        public void CreateProductForFridge_WithUnknownFridge_ReturnsBadRequest()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((Fridge)null);
            // Act
            var result = controller.CreateProductForFridge(It.IsAny<Guid>(), null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateProductForFridge_WithNullDto_ReturnsBadRequest()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(new Fridge());
            // Act
            var result = controller.CreateProductForFridge(It.IsAny<Guid>(), null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateProductForFridge_WithProductToCreate_ReturnsCreatedProduct()
        {
            // Arrange
            FridgeProduct product = CreateRandomFridgeProduct();
            FridgeProductDto productDto = new FridgeProductDto()
            {
                Id = product.Id,
                ProductName = Guid.NewGuid().ToString(),
                ProductId = product.ProductId,
                Quantity = product.Quantity,
            };
            FridgeProductToCreateDto productToCreate = new FridgeProductToCreateDto()
            {
                ProductId = product.Id,
                Quantity = product.Quantity,
            };
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(new Fridge());
            repositoryStub.Setup(repo => repo.FridgeProduct.CreateFridgeProduct(It.IsAny<FridgeProduct>()));
            mapperStub.Setup(map => map.Map<FridgeProduct>(productToCreate)).Returns(product);
            mapperStub.Setup(map => map.Map<FridgeProductDto>(product)).Returns(productDto);

            // Act
            var result = controller.CreateProductForFridge(It.IsAny<Guid>(), productToCreate);

            // Assert
            FridgeProductDto createdProduct = (result as CreatedAtActionResult).Value as FridgeProductDto;
            Assert.Equal(productDto, createdProduct);
            Assert.NotEqual(createdProduct.Id, Guid.Empty);
        }

        [Fact]
        public void DeleteProductInFridge_WithUnknownFridge_ReturnsNotFound()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((Fridge)null);

            // Act
            var result = controller.DeleteProductInFridge(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteProductInFridge_WithUnknownProduct_ReturnsNotFound()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(new Fridge());
            repositoryStub
                .Setup(repo => repo.FridgeProduct.GetFridgeProduct(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((FridgeProduct)null);

            // Act
            var result = controller.DeleteProductInFridge(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteProductInFridge_WithExistingFridgeAndProduct_ReturnsNoContent()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.Fridge.GetFridge(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(new Fridge());
            repositoryStub
                .Setup(repo => repo.FridgeProduct.GetFridgeProduct(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(new FridgeProduct());
            repositoryStub.Setup(repo => repo.FridgeProduct.DeleteFridgeProduct(It.IsAny<FridgeProduct>()));

            // Act
            var result = controller.DeleteProductInFridge(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        private Fridge CreateRandomFridge()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                OwnerName = String.Empty,
                ModelId = Guid.NewGuid()
            };
        }

        private FridgeDto CreateDtoFromFridge(Fridge fridge)
        {
            return new()
            {
                Id = fridge.Id,
                Name = fridge.Name,
                OwnerName = fridge.OwnerName,
                ModelId = fridge.ModelId
            };
        }

        private FridgeProduct CreateRandomFridgeProduct()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                FridgeId = Guid.NewGuid(),
                Quantity = new Random().Next(20),
            };
        }
    }
}
