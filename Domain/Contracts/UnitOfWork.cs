using System.Threading.Tasks;

namespace Domain.Contracts
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
