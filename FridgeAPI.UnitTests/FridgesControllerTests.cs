using Moq;
using Xunit;
using AutoMapper;
using Domain.Models;
using Services.Contracts;
using Services.Models;
using FridgeAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FridgeAPI.UnitTests
{
    public class FridgesControllerTests
    {
        private readonly Mock<ILogger<FridgesController>> loggerStub;
        private readonly Mock<IFridgeService> serviceStub;
        private readonly FridgesController controller;

        public FridgesControllerTests()
        {
            loggerStub = new Mock<ILogger<FridgesController>>();
            serviceStub = new Mock<IFridgeService>();
            controller = new FridgesController(loggerStub.Object, serviceStub.Object);
        }

        [Fact]
        public void GetFridgeById_UnknownId_ReturnsNotFound()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((FridgeResponse)null);

            // Act
            var result = controller.GetFridgeById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetFridgeById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            Fridge expected = CreateRandomFridge();
            FridgeResponse expectedDto = CreateDtoFromFridge(expected);
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(expectedDto);

            // Act
            var result = controller.GetFridgeById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetFridgeById_ExistingId_ReturnsRightFridge()
        {
            // Arrange
            Fridge expected = CreateRandomFridge();
            FridgeResponse expectedDto = CreateDtoFromFridge(expected);
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(expectedDto);

            // Act
            var result = controller.GetFridgeById(It.IsAny<Guid>());

            // Assert
            FridgeResponse dto = (FridgeResponse)(result.Result as OkObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void GetFridges_ExistingFridges_ReturnsAllFridges()
        {
            // Arrange
            IEnumerable<Fridge> expected = new[] { CreateRandomFridge(), CreateRandomFridge(), CreateRandomFridge() };
            List<FridgeResponse> expectedDto = new List<FridgeResponse>();
            foreach(var fridge in expected)
            {
                expectedDto.Add(CreateDtoFromFridge(fridge));
            }
            serviceStub.Setup(serv => serv.GetAll()).ReturnsAsync(expectedDto);

            // Act
            var result = controller.GetFridges();

            // Assert
            IEnumerable<FridgeResponse> dto = (IEnumerable<FridgeResponse>)(result.Result as OkObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void CreateFridge_WithNullDto_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = controller.CreateFridge(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void CreateFridge_WithFridgeToCreate_ReturnsCreatedFridge()
        {
            // Arrange
            Fridge fridge = CreateRandomFridge();
            FridgeResponse fridgeDto = CreateDtoFromFridge(fridge);
            FridgeRequest fridgeToCreate = new FridgeRequest()
            {
                Name = fridge.Name,
                ModelId = fridge.ModelId,
                OwnerName = fridge.OwnerName,
            };
            serviceStub.Setup(serv => serv.Create(It.IsAny<FridgeRequest>())).ReturnsAsync(fridgeDto);

            // Act
            var result = controller.CreateFridge(fridgeToCreate);

            // Assert
            FridgeResponse createdFridge = (FridgeResponse)(result.Result as CreatedAtActionResult).Value;
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
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFridge_WithUnknownFridge_ReturnsNotFound()
        {
            // Arrange
            Fridge fridge = CreateRandomFridge();
            FridgeRequest fridgeToUpdate = new FridgeRequest()
            {
                Name = fridge.Name,
                ModelId = fridge.ModelId,
                OwnerName = fridge.OwnerName,
            };
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((FridgeResponse)null);

            // Act
            var result = controller.UpdateFridge(It.IsAny<Guid>(), fridgeToUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void UpdateFridge_WithExistingFridge_ReturnsNoContent()
        {
            // Arrange
            Fridge fridge = CreateRandomFridge();
            FridgeRequest fridgeToUpdate = new FridgeRequest()
            {
                Name = fridge.Name + fridge.ModelId.ToString(),
                ModelId = fridge.ModelId,
                OwnerName = fridge.OwnerName,
            };
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(new FridgeResponse());
            serviceStub.Setup(serv => serv.Update(It.IsAny<Guid>(), fridgeToUpdate));

            // Act
            var result = controller.UpdateFridge(fridge.Id, fridgeToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void DeleteFridge_WithUnknownFridge_ReturnsNotFound()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((FridgeResponse)null);

            // Act
            var result = controller.DeleteFridge(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void DeleteFridge_WithExistingFridge_ReturnsNoContent()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(new FridgeResponse());
            serviceStub.Setup(serv => serv.Delete(It.IsAny<Guid>()));

            // Act
            var result = controller.DeleteFridge(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetFridgeProductsById_WithUnknownFridge_ReturnsNotFound()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((FridgeResponse)null);

            // Act
            var result = controller.GetFridgeProductsById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetFridgeProductsById_WithEmptyProducts_ReturnsEmptyEnumerable()
        {
            // Arrange
            Fridge fridge = CreateRandomFridge();
            fridge.Products = new List<FridgeProduct>();
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(CreateDtoFromFridge(fridge));
            serviceStub.Setup(serv => serv.GetProducts(It.IsAny<Guid>()))
                .ReturnsAsync(Enumerable.Empty<FridgeProductResponse>);

            // Act
            var result = controller.GetFridgeProductsById(It.IsAny<Guid>());

            // Assert
            IEnumerable<FridgeProductResponse> products = (IEnumerable<FridgeProductResponse>)(result.Result as OkObjectResult).Value;
            Assert.Empty(products);
        }

        [Fact]
        public void GetFridgeProductsById_WithExistingFridgeAndProducts_ReturnsAllProducts()
        {
            // Arrange
            Fridge fridge = CreateRandomFridge();
            ICollection<FridgeProduct> products = new[] { CreateRandomFridgeProduct(), CreateRandomFridgeProduct() };
            List<FridgeProductResponse> expectedProducts = new List<FridgeProductResponse>();
            foreach(var product in products)
            {
                expectedProducts.Add(new FridgeProductResponse() 
                { 
                    Id = product.Id,
                    ProductName = Guid.NewGuid().ToString(),
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                });
            }
            fridge.Products = products;
            serviceStub.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(CreateDtoFromFridge(fridge));
            serviceStub.Setup(serv => serv.GetProducts(It.IsAny<Guid>())).ReturnsAsync(expectedProducts);

            // Act
            var result = controller.GetFridgeProductsById(It.IsAny<Guid>());

            // Assert
            List<FridgeProductResponse> dto = (List<FridgeProductResponse>)(result.Result as OkObjectResult).Value;
            Assert.Equal(expectedProducts, dto);
        }

        [Fact]
        public void CreateProductForFridge_WithUnknownFridge_ReturnsBadRequest()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((FridgeResponse)null);
            // Act
            var result = controller.CreateProductForFridge(It.IsAny<Guid>(), null);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void CreateProductForFridge_WithNullDto_ReturnsBadRequest()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(new FridgeResponse());
            // Act
            var result = controller.CreateProductForFridge(It.IsAny<Guid>(), null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void CreateProductForFridge_WithProductToCreate_ReturnsCreatedProduct()
        {
            // Arrange
            FridgeProduct product = CreateRandomFridgeProduct();
            FridgeProductResponse productDto = new FridgeProductResponse()
            {
                Id = product.Id,
                ProductName = Guid.NewGuid().ToString(),
                ProductId = product.ProductId,
                Quantity = product.Quantity,
            };
            FridgeProductRequest productToCreate = new FridgeProductRequest()
            {
                ProductId = product.Id,
                Quantity = product.Quantity,
            };
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(new FridgeResponse());
            serviceStub.Setup(serv => serv.CreateProduct(It.IsAny<Guid>(), It.IsAny<FridgeProductRequest>()))
                .ReturnsAsync(productDto);

            // Act
            var result = controller.CreateProductForFridge(It.IsAny<Guid>(), productToCreate);

            // Assert
            FridgeProductResponse createdProduct = (FridgeProductResponse)(result.Result as CreatedAtActionResult).Value;
            Assert.Equal(productDto, createdProduct);
            Assert.NotEqual(createdProduct.Id, Guid.Empty);
        }

        [Fact]
        public void DeleteProductInFridge_WithUnknownFridge_ReturnsNotFound()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((FridgeResponse)null);

            // Act
            var result = controller.DeleteProductInFridge(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void DeleteProductInFridge_WithUnknownProduct_ReturnsNotFound()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(new FridgeResponse());
            serviceStub.Setup(serv => serv.GetProductById(It.IsAny<Guid>())).ReturnsAsync((FridgeProductResponse)null);

            // Act
            var result = controller.DeleteProductInFridge(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void DeleteProductInFridge_WithExistingFridgeAndProduct_ReturnsNoContent()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(new FridgeResponse());
            serviceStub.Setup(serv => serv.GetProductById(It.IsAny<Guid>())).ReturnsAsync(new FridgeProductResponse());
            serviceStub.Setup(serv => serv.DeleteProduct(It.IsAny<Guid>()));

            // Act
            var result = controller.DeleteProductInFridge(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
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

        private FridgeResponse CreateDtoFromFridge(Fridge fridge)
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
