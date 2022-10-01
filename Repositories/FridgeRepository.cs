using Contracts;
using Entities.Models;
using System;
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

        public Fridge GetFridge(Guid id, bool trackChanges)
        {
            return FindByCondition((Fridge fr) => fr.Id == id, trackChanges).FirstOrDefault();
        }

        public void CreateFridge(Fridge fridge)
        {
            Create(fridge);
        }

        public void UpdateFridge(Fridge fridge)
        {
            Create(fridge);
        }

        public void DeleteFridge(Fridge fridge)
        {
            Delete(fridge);
        }
    }
}
