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
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> serviceStub;
        private readonly Mock<ILogger<ProductsController>> loggerStub;
        private readonly ProductsController controller;

        public ProductsControllerTests()
        {
            loggerStub = new Mock<ILogger<ProductsController>>();
            serviceStub = new Mock<IProductService>();
            controller = new ProductsController(loggerStub.Object, serviceStub.Object);
        }

        [Fact]
        public void GetProductById_UnknownId_ReturnsNotFound()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((ProductResponse)null);

            // Act
            var result = controller.GetProductById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetProductById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            Product expected = CreateRandomProduct();
            ProductResponse expectedDto = new ProductResponse()
            {
                Id = expected.Id,
                Name = expected.Name,
                DefaultQuantity = expected.DefaultQuantity,
            };
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(expectedDto);

            // Act
            var result = controller.GetProductById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetProductById_ExistingId_ReturnsRightProduct()
        {
            // Arrange
            Product expected = CreateRandomProduct();
            ProductResponse expectedDto = new ProductResponse()
            {
                Id = expected.Id,
                Name = expected.Name,
                DefaultQuantity = expected.DefaultQuantity,
            };
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(expectedDto);

            // Act
            var result = controller.GetProductById(It.IsAny<Guid>());

            // Assert
            ProductResponse dto = (ProductResponse)(result.Result as OkObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void GetProducts_ExistingProducts_ReturnsAllProducts()
        {
            // Arrange
            IEnumerable<Product> expected = new[] { CreateRandomProduct(), CreateRandomProduct(), CreateRandomProduct() };
            List<ProductResponse> expectedDto = new List<ProductResponse>();
            foreach (var product in expected)
            {
                expectedDto.Add(new ProductResponse()
                {
                    Id = product.Id,
                    Name = product.Name,
                    DefaultQuantity = product.DefaultQuantity,
                });
            }
            serviceStub.Setup(serv => serv.GetAll()).ReturnsAsync(expectedDto);

            // Act
            var result = controller.GetProducts();

            // Assert
            IEnumerable<ProductResponse> dto = (IEnumerable<ProductResponse>)(result.Result as OkObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void CreateProduct_WithNullDto_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = controller.CreateProduct(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void CreateProduct_WithproductToCreate_ReturnsCreatedProduct()
        {
            // Arrange
            Product product = CreateRandomProduct();
            ProductResponse productDto = new ProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
                DefaultQuantity = product.DefaultQuantity,
            };
            ProductRequest productToCreate = new ProductRequest()
            {
                Name = product.Name,
                DefaultQuantity = product.DefaultQuantity,
            };
            serviceStub.Setup(serv => serv.Create(It.IsAny<ProductRequest>())).ReturnsAsync(productDto);

            // Act
            var result = controller.CreateProduct(productToCreate);

            // Assert
            ProductResponse createdProduct = (ProductResponse)(result.Result as CreatedAtActionResult).Value;
            Assert.Equal(productDto, createdProduct);
            Assert.NotEqual(createdProduct.Id, Guid.Empty);
        }

        [Fact]
        public void UpdateProduct_WithNullDto_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = controller.UpdateProduct(It.IsAny<Guid>(), null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateProduct_WithUnknownProduct_ReturnsNotFound()
        {
            // Arrange
            Product product = CreateRandomProduct();
            ProductRequest productToUpdate = new ProductRequest()
            {
                Name = product.Name,
                DefaultQuantity = product.DefaultQuantity,
            };
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((ProductResponse)null);

            // Act
            var result = controller.UpdateProduct(It.IsAny<Guid>(), productToUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Updateproduct_WithExistingProduct_ReturnsNoContent()
        {
            // Arrange
            Product product = CreateRandomProduct();
            ProductRequest productToUpdate = new ProductRequest()
            {
                Name = product.Name + product.DefaultQuantity.ToString(),
                DefaultQuantity = product.DefaultQuantity,
            };
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(new ProductResponse());
            serviceStub.Setup(serv => serv.Update(It.IsAny<Guid>(), productToUpdate));

            // Act
            var result = controller.UpdateProduct(product.Id, productToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void DeleteProduct_WithUnknownProduct_ReturnsNotFound()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync((ProductResponse)null);

            // Act
            var result = controller.DeleteProduct(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void DeleteProduct_WithExistingProduct_ReturnsNoContent()
        {
            // Arrange
            serviceStub.Setup(serv => serv.GetById(It.IsAny<Guid>())).ReturnsAsync(new ProductResponse());
            serviceStub.Setup(serv => serv.Delete(It.IsAny<Guid>()));

            // Act
            var result = controller.DeleteProduct(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        private Product CreateRandomProduct()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                DefaultQuantity = new Random().Next(1, 100),
            };
        }
    }
}
