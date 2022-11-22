using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IFridgeProductRepository : IRepository<FridgeProduct>
    {
        Task ExcecuteProcedure(string query);
    }
}
