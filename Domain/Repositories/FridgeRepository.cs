using Domain.Contracts;
using Domain.Models;

namespace Domain.Repositories
{
    public class FridgeRepository : RepositoryBase<Fridge>, IFridgeRepository
    {
        public FridgeRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
