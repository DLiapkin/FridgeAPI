using Contracts;
using Entities.Models;
using System.Linq;
using System.Collections.Generic;

namespace Repositories
{
    public class FridgeRepository : RepositoryBase<Fridge>, IFridgeRepository
    {
        public FridgeRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }

        public IEnumerable<Fridge> GetAllFridges(bool trackChanges) 
        {
            return FindAll(trackChanges).OrderBy(c => c.Name).ToList();
        }  
    }
}
