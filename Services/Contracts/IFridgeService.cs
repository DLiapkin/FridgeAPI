using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Models;

namespace Services.Contracts
{
    public interface IFridgeService
    {
        Task<FridgeResponse> Create(FridgeRequest fridgeToCreate);
        Task<IEnumerable<FridgeResponse>> GetAll();
        Task<FridgeResponse> GetById(Guid id);
        Task Update(Guid id, FridgeRequest fridgeToUpdate);
        Task Delete(Guid id);

        Task<FridgeProductResponse> CreateProduct(Guid fridgeId, FridgeProductRequest productToCreateDto);
        Task<IEnumerable<FridgeProductResponse>> GetProducts(Guid fridgeId);
        Task<FridgeProductResponse> GetProductById(Guid productId);
        Task DeleteProduct(Guid fridgeProductId);
        Task RefreshProduct();
    }
}
