using Entities.Models;
using System.Threading.Tasks;

namespace Contracts.Repositries
{
    public interface IFridgeProductRepository : IRepository<FridgeProduct>
    {
        Task ExcecuteProcedure(string query);
    }
}
