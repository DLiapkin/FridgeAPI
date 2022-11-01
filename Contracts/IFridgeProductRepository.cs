using Entities.Models;

namespace Contracts
{
    public interface IFridgeProductRepository : IRepository<FridgeProduct>
    {
        void ExcecuteProcedure(string query);
    }
}
