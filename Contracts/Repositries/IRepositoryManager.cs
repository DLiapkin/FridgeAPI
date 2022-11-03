using System.Threading.Tasks;

namespace Contracts.Repositries
{
    public interface IUnitOfWork
    {
        IFridgeModelRepository FridgeModel { get; }
        IFridgeRepository Fridge { get; }
        IFridgeProductRepository FridgeProduct { get; }
        IProductRepository Product { get; }
        Task Save();
    }
}
