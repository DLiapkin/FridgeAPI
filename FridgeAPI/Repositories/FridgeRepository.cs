using Contracts;
using FridgeAPI.Models;

namespace FridgeAPI.Repositories
{
    public class FridgeRepository : RepositoryBase<Fridge>, IFridgeRepository
    {
        public FridgeRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
