using Entities.Models;
using System.Collections.Generic;

namespace Contracts
{
    public interface IFridgeRepository
    {
        IEnumerable<Fridge> GetAllFridges(bool trackChanges);
    }
}
