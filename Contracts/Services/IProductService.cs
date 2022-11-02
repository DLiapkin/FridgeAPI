using System;
using System.Collections.Generic;
using Entities.DataTransferObjects;

namespace Contracts.Services
{
    public interface IProductService
    {
        ProductDto Create(ProductToCreateDto productToCreate);
        IEnumerable<ProductDto> GetAll();
        ProductDto GetById(Guid id);
        void Update(Guid id, ProductToUpdateDto productToUpdate);
        void Delete(Guid id);
    }
}
