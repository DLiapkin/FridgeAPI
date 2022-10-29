using Contracts;
using Entities.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }

        public IEnumerable<Product> GetAllProducts(bool trackChanges)
        {
            return FindAll(trackChanges);
        }

        public Product GetProduct(Guid id, bool trackChanges)
        {
            return FindById(id, trackChanges);
        }

        public void CreateProduct(Product product)
        {
            Create(product);
        }

        public void UpdateProduct(Product product)
        {
            Update(product);
        }

        public void DeleteProduct(Product product)
        {
            Delete(product);
        }
    }
}
