using Domain.Contracts;
using Domain.Models;

namespace Domain.Repositories
{
    public class FridgeModelRepository : RepositoryBase<FridgeModel>, IFridgeModelRepository
    {
        public FridgeModelRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
