using System;
using System.Collections.Generic;
using Entities.DataTransferObjects;

namespace Contracts.Services
{
    public interface IFridgeService
    {
        FridgeDto Create(FridgeToCreateDto fridgeToCreate);
        IEnumerable<FridgeDto> GetAll();
        FridgeDto GetById(Guid id);
        void Update(Guid id, FridgeToUpdateDto fridgeToUpdate);
        void Delete(Guid id);

        FridgeProductDto CreateProduct(Guid fridgeId, FridgeProductToCreateDto productToCreateDto);
        IEnumerable<FridgeProductDto> GetProducts(Guid fridgeId);
        FridgeProductDto GetProductById(Guid productId);
        void DeleteProduct(Guid fridgeProductId);
        void RefreshProduct();
    }
}
