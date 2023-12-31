﻿using Dogs.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.Interfaces
{
    public interface IDogRepository : 
        IRepository<DogEntity>
    {
        Task<DogEntity> GetByName(string name);

        Task<DogEntity> NoTracingWithName(string name);
    }
}
