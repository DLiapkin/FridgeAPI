using Contracts.Repositries;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Repositories
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }

        public async Task ExcecuteProcedure(string query)
        {
            await Context.Database.ExecuteSqlRawAsync(query);
        }
    }
}
