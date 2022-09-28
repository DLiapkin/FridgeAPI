using Contracts;
using Entities.Models;

namespace Repositories
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
