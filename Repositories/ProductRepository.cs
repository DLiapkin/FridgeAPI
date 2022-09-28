using Contracts;
using Entities.Models;

namespace Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
