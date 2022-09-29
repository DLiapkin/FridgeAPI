using Entities.Models;
using System;
using System.Collections.Generic;

namespace Contracts
{
    public interface IFridgeRepository
    {
        IEnumerable<Fridge> GetAllFridges(bool trackChanges);
        IEnumerable<Fridge> GetFridge(Guid id, bool trackChanges);
        void CreateFridge(Fridge fridge);
        void UpdateFridge(Fridge fridge);
        void DeleteFridge(Fridge fridge);
    }
}
