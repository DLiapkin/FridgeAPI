namespace Contracts
{
    public interface IUnitOfWork
    {
        IFridgeModelRepository FridgeModel { get; }
        IFridgeRepository Fridge { get; }
        IFridgeProductRepository FridgeProduct { get; }
        IProductRepository Product { get; }
        void Save();
    }
}
