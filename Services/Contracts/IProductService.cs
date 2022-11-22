using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Models;

namespace Services.Contracts
{
    public interface IProductService
    {
        Task<ProductResponse> Create(ProductRequest productToCreate);
        Task<IEnumerable<ProductResponse>> GetAll();
        Task<ProductResponse> GetById(Guid id);
        Task Update(Guid id, ProductRequest productToUpdate);
        Task Delete(Guid id);
    }
}
