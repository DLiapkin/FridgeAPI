using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects;

namespace Contracts.Services
{
    public interface IFridgeService
    {
        Task<FridgeDto> Create(FridgeToCreateDto fridgeToCreate);
        Task<IEnumerable<FridgeDto>> GetAll();
        Task<FridgeDto> GetById(Guid id);
        Task Update(Guid id, FridgeToUpdateDto fridgeToUpdate);
        Task Delete(Guid id);

        Task<FridgeProductDto> CreateProduct(Guid fridgeId, FridgeProductToCreateDto productToCreateDto);
        Task<IEnumerable<FridgeProductDto>> GetProducts(Guid fridgeId);
        Task<FridgeProductDto> GetProductById(Guid productId);
        Task DeleteProduct(Guid fridgeProductId);
        Task RefreshProduct();
    }
}
