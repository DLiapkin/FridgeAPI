using Domain.Contracts;
using Domain.Models;

namespace Domain.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
