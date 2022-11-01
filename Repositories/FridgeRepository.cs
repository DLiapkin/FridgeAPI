using Contracts;
using Entities.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Repositories
{
    public class FridgeRepository : RepositoryBase<Fridge>, IFridgeRepository
    {
        public FridgeRepository(DataContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
