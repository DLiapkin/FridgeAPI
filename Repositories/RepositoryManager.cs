using Contracts;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _repositoryContext;
        private IFridgeModelRepository _fridgeModelRepository;
        private IFridgeRepository _fridgeRepository;
        private IFridgeProductRepository _fridgeProductRepository;
        private IProductRepository _productRepository;

        public UnitOfWork(DataContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IFridgeModelRepository FridgeModel
        {
            get
            {
                if (_fridgeModelRepository == null)
                    _fridgeModelRepository = new FridgeModelRepository(_repositoryContext);
                return _fridgeModelRepository;
            }
        }

        public IFridgeRepository Fridge
        {
            get
            {
                if (_fridgeRepository == null)
                    _fridgeRepository = new FridgeRepository(_repositoryContext);
                return _fridgeRepository;
            }
        }

        public IFridgeProductRepository FridgeProduct
        {
            get
            {
                if (_fridgeProductRepository == null)
                    _fridgeProductRepository = new FridgeProductRepository(_repositoryContext);
                return _fridgeProductRepository;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(_repositoryContext);
                return _productRepository;
            }
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
