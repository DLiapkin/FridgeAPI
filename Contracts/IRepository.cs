using System;
using System.Collections.Generic;

namespace Contracts
{
    public interface IRepository<T>
    {
        IEnumerable<T> FindAll(bool trackChanges);
        T FindById(Guid id, bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
