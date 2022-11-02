using System;
using System.Collections.Generic;
using Entities.DataTransferObjects;

namespace Contracts.Services
{
    public interface IFridgeModelService
    {
        FridgeModelDto Create(FridgeModelToCreateDto fridgeModelToCreate);
        IEnumerable<FridgeModelDto> GetAll();
        FridgeModelDto GetById(Guid id);
        void Update(Guid id, FridgeModelToUpdateDto fridgeModelToUpdate);
        void Delete(Guid id);
    }
}
