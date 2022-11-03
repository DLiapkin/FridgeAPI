using Contracts.Repositries;
using Entities.Models;

namespace Repositories
{
    public class FridgeRepository : RepositoryBase<Fridge>, IFridgeRepository
    {
        public FridgeRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
