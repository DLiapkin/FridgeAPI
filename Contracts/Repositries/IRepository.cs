using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Repositries
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> FindAll(bool trackChanges);
        Task<T> FindById(Guid id, bool trackChanges);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
