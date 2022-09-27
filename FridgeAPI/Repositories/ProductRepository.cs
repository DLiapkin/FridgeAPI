using Contracts;
using FridgeAPI.Models;

namespace FridgeAPI.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
