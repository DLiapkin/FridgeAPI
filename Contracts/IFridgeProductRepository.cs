using Entities.Models;
using System;
using System.Collections.Generic;

namespace Contracts
{
    public interface IFridgeProductRepository
    {
        IEnumerable<FridgeProduct> GetAllFridgeProducts(bool trackChanges);
        FridgeProduct GetFridgeProduct(Guid id, bool trackChanges);
        void CreateFridgeProduct(FridgeProduct fridgeProduct);
        void UpdateFridgeProduct(FridgeProduct fridgeProduct);
        void DeleteFridgeProduct(FridgeProduct fridgeProduct);
    }
}
