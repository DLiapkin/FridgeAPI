using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repositries;
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

        public async Task<IEnumerable<T>> FindAll(bool trackChanges)
        {
            if (trackChanges)
            {
                return await Context.Set<T>().ToListAsync();
            }
            else
            {
                return await Context.Set<T>().AsNoTracking().ToListAsync();
            }
        }

        public async Task<T> FindById(Guid id, bool trackChanges)
        {
            if (trackChanges)
            {
                return await Context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            }
            else
            {
                return await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            }
        }

        public async Task Create(T entity) 
        {
            await Context.Set<T>().AddAsync(entity);
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
