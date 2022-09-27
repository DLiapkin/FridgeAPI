using Contracts;
using FridgeAPI.Models;

namespace FridgeAPI.Repositories
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
