using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects;

namespace Contracts.Services
{
    public interface IFridgeModelService
    {
        Task<FridgeModelDto> Create(FridgeModelToCreateDto fridgeModelToCreate);
        Task<IEnumerable<FridgeModelDto>> GetAll();
        Task<FridgeModelDto> GetById(Guid id);
        Task Update(Guid id, FridgeModelToUpdateDto fridgeModelToUpdate);
        Task Delete(Guid id);
    }
}
