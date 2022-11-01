using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }

        public void ExcecuteProcedure(string query)
        {
            Context.Database.ExecuteSqlRaw(query);
        }
    }
}
