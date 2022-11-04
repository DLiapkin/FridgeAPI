using Domain.Contracts;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _repositoryContext;
        private IFridgeModelRepository _fridgeModelRepository;
        private IFridgeRepository _fridgeRepository;
        private IFridgeProductRepository _fridgeProductRepository;
        private IProductRepository _productRepository;

        public UnitOfWork(DataContext repositoryContext, IFridgeModelRepository fridgeModelRepository,
            IFridgeRepository fridgeRepository, IFridgeProductRepository fridgeProductRepository, IProductRepository productRepository)
        {
            _repositoryContext = repositoryContext;
            _fridgeModelRepository = fridgeModelRepository;
            _fridgeRepository = fridgeRepository;
            _fridgeProductRepository = fridgeProductRepository;
            _productRepository = productRepository;
        }

        public IFridgeModelRepository FridgeModel
        {
            get
            {
                return _fridgeModelRepository;
            }
        }

        public IFridgeRepository Fridge
        {
            get
            {
                return _fridgeRepository;
            }
        }

        public IFridgeProductRepository FridgeProduct
        {
            get
            {
                return _fridgeProductRepository;
            }
        }

        public IProductRepository Product
        {
            get
            {
                return _productRepository;
            }
        }

        public async Task Save()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
