using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Models;

namespace Services.Contracts
{
    public interface IFridgeModelService
    {
        Task<FridgeModelResponse> Create(FridgeModelRequest fridgeModelToCreate);
        Task<IEnumerable<FridgeModelResponse>> GetAll();
        Task<FridgeModelResponse> GetById(Guid id);
        Task Update(Guid id, FridgeModelRequest fridgeModelToUpdate);
        Task Delete(Guid id);
    }
}
