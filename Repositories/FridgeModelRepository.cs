using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class FridgeModelRepository : RepositoryBase<FridgeModel>, IFridgeModelRepository
    {
        public FridgeModelRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }

        public IEnumerable<FridgeModel> GetAllFridgeModels(bool trackChanges)
        {
            return FindAll(trackChanges);
        }

        public FridgeModel GetFridgeModel(Guid id, bool trackChanges)
        {
            return FindByCondition((FridgeModel fm) => fm.Id == id, trackChanges).FirstOrDefault();
        }

        public void CreateFridgeModel(FridgeModel fridgeModel)
        {
            Create(fridgeModel);
        }

        public void UpdateFridgeModel(FridgeModel fridgeModel)
        {
            Update(fridgeModel);
        }

        public void DeleteFridgeModel(FridgeModel fridgeModel)
        {
            Delete(fridgeModel);
        }
    }
}
