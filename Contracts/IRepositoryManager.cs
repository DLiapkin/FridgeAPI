namespace Contracts
{
    public interface IRepositoryManager
    {
        IFridgeModelRepository FridgeModel { get; }
        IFridgeRepository Fridge { get; }
        IFridgeProductRepository FridgeProduct { get; }
        IProductRepository Product { get; }
        void Save();
    }
}
