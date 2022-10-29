using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : BaseEntity
    {
        protected DataContext Context;

        public RepositoryBase(DataContext repositoryContext)
        {
            Context = repositoryContext;
        }

        public IEnumerable<T> FindAll(bool trackChanges)
        {
            return !trackChanges ? Context.Set<T>().AsNoTracking() : Context.Set<T>();
        }

        public T FindById(Guid id, bool trackChanges)
        {
            return !trackChanges ? Context.Set<T>().AsNoTracking().FirstOrDefault(e => e.Id == id) : Context.Set<T>().FirstOrDefault(e => e.Id == id);
        }

        public void Create(T entity) 
        {
            Context.Set<T>().Add(entity);
        }

        public void Update(T entity) 
        {
            Context.Set<T>().Update(entity);
        }

        public void Delete(T entity) 
        {
            Context.Set<T>().Remove(entity);
        }
    }
}
