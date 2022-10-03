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
    public class ProductsControllerTests
    {
        private readonly Mock<IRepositoryManager> repositoryStub;
        private readonly Mock<ILogger<ProductsController>> loggerStub;
        private readonly Mock<IMapper> mapperStub;
        private readonly ProductsController controller;

        public ProductsControllerTests()
        {
            repositoryStub = new Mock<IRepositoryManager>();
            loggerStub = new Mock<ILogger<ProductsController>>();
            mapperStub = new Mock<IMapper>();
            controller = new ProductsController(loggerStub.Object, repositoryStub.Object, mapperStub.Object);
        }

        [Fact]
        public void GetProductById_UnknownId_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.Product.GetProduct(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((Product)null);

            // Act
            var result = controller.GetProductById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetProductById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            Product expected = CreateRandomProduct();
            ProductDto expectedDto = new ProductDto()
            {
                Id = expected.Id,
                Name = expected.Name,
                DefaultQuantity = expected.DefaultQuantity,
            };
            repositoryStub.Setup(repo => repo.Product.GetProduct(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(expected);
            mapperStub.Setup(map => map.Map<ProductDto>(expected)).Returns(expectedDto);

            // Act
            var result = controller.GetProductById(It.IsAny<Guid>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetProductById_ExistingId_ReturnsRightProduct()
        {
            // Arrange
            Product expected = CreateRandomProduct();
            ProductDto expectedDto = new ProductDto()
            {
                Id = expected.Id,
                Name = expected.Name,
                DefaultQuantity = expected.DefaultQuantity,
            };
            repositoryStub.Setup(repo => repo.Product.GetProduct(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(expected);
            mapperStub.Setup(map => map.Map<ProductDto>(expected)).Returns(expectedDto);

            // Act
            var result = controller.GetProductById(It.IsAny<Guid>());

            // Assert
            ProductDto dto = (ProductDto)(result as ObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void GetProducts_ExistingProducts_ReturnsAllproducts()
        {
            // Arrange
            IEnumerable<Product> expected = new[] { CreateRandomProduct(), CreateRandomProduct(), CreateRandomProduct() };
            List<ProductDto> expectedDto = new List<ProductDto>();
            foreach (var product in expected)
            {
                expectedDto.Add(new ProductDto()
                {
                    Id = product.Id,
                    Name = product.Name,
                    DefaultQuantity = product.DefaultQuantity,
                });
            }
            repositoryStub.Setup(repo => repo.Product.GetAllProducts(It.IsAny<bool>()))
                .Returns(expected);
            mapperStub.Setup(map => map.Map<IEnumerable<ProductDto>>(expected)).Returns(expectedDto);

            // Act
            var result = controller.GetProducts();

            // Assert
            IEnumerable<ProductDto> dto = (IEnumerable<ProductDto>)(result as ObjectResult).Value;
            Assert.Equal(expectedDto, dto);
        }

        [Fact]
        public void CreateProduct_WithNullDto_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = controller.CreateProduct(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Createproduct_WithproductToCreate_ReturnsCreatedproduct()
        {
            // Arrange
            Product product = CreateRandomProduct();
            ProductDto productDto = new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                DefaultQuantity = product.DefaultQuantity,
            };
            ProductToCreateDto productToCreate = new ProductToCreateDto()
            {
                Name = product.Name,
                DefaultQuantity = product.DefaultQuantity,
            };
            repositoryStub.Setup(repo => repo.Product.CreateProduct(It.IsAny<Product>()));
            mapperStub.Setup(map => map.Map<Product>(productToCreate)).Returns(product);
            mapperStub.Setup(map => map.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = controller.CreateProduct(productToCreate);

            // Assert
            ProductDto createdProduct = (result as CreatedAtRouteResult).Value as ProductDto;
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
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateProduct_WithUnknownProduct_ReturnsNotFound()
        {
            // Arrange
            Product product = CreateRandomProduct();
            ProductToUpdateDto productToUpdate = new ProductToUpdateDto()
            {
                Name = product.Name,
                DefaultQuantity = product.DefaultQuantity,
            };
            repositoryStub
                .Setup(repo => repo.Product.GetProduct(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((Product)null);

            // Act
            var result = controller.UpdateProduct(It.IsAny<Guid>(), productToUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Updateproduct_WithExistingProduct_ReturnsNoContent()
        {
            // Arrange
            Product product = CreateRandomProduct();
            ProductToUpdateDto productToUpdate = new ProductToUpdateDto()
            {
                Name = product.Name + product.DefaultQuantity.ToString(),
                DefaultQuantity = product.DefaultQuantity,
            };
            repositoryStub
                .Setup(repo => repo.Product.GetProduct(product.Id, It.IsAny<bool>()))
                .Returns(product);
            repositoryStub.Setup(repo => repo.Product.UpdateProduct(product));
            mapperStub.Setup(map => map.Map(product, productToUpdate));

            // Act
            var result = controller.UpdateProduct(product.Id, productToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteProduct_WithUnknownProduct_ReturnsNotFound()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.Product.GetProduct(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns((Product)null);

            // Act
            var result = controller.DeleteProduct(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteProduct_WithExistingProduct_ReturnsNoContent()
        {
            // Arrange
            repositoryStub
                .Setup(repo => repo.Product.GetProduct(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(new Product());
            repositoryStub.Setup(repo => repo.Product.DeleteProduct(It.IsAny<Product>()));

            // Act
            var result = controller.DeleteProduct(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NoContentResult>(result);
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
