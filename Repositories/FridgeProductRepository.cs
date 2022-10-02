using Contracts;
using Entities.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateFridgeProduct(FridgeProduct fridgeProduct)
        {
            Create(fridgeProduct);
        }

        public IEnumerable<FridgeProduct> GetAllFridgeProducts(bool trackChanges)
        {
            return FindAll(trackChanges);
        }

        public FridgeProduct GetFridgeProduct(Guid id, bool trackChanges)
        {
            return FindByCondition((FridgeProduct f) => f.Id == id, trackChanges).FirstOrDefault();
        }

        public void UpdateFridgeProduct(FridgeProduct fridgeProduct)
        {
            Update(fridgeProduct);
        }

        public void DeleteFridgeProduct(FridgeProduct fridgeProduct)
        {
            Delete(fridgeProduct);
        }

        public void ExcecuteProcedure(string query)
        {
            Context.Database.ExecuteSqlRaw(query);
        }
    }
}
