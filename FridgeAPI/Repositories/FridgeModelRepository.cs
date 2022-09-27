using Contracts;
using FridgeAPI.Models;

namespace FridgeAPI.Repositories
{
    public class FridgeModelRepository : RepositoryBase<FridgeModel>, IFridgeModelRepository
    {
        public FridgeModelRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
