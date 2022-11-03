using Contracts.Repositries;
using Entities.Models;

namespace Repositories
{
    public class FridgeModelRepository : RepositoryBase<FridgeModel>, IFridgeModelRepository
    {
        public FridgeModelRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
