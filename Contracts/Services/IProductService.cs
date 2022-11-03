using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects;

namespace Contracts.Services
{
    public interface IProductService
    {
        Task<ProductDto> Create(ProductToCreateDto productToCreate);
        Task<IEnumerable<ProductDto>> GetAll();
        Task<ProductDto> GetById(Guid id);
        Task Update(Guid id, ProductToUpdateDto productToUpdate);
        Task Delete(Guid id);
    }
}
