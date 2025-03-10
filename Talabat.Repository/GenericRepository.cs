﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbContext;

		public GenericRepository(StoreContext dbContext) // ASK CLR for Creating Object From DbContext Implicitly
        {
			_dbContext = dbContext;
		}
        public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await _dbContext.Set<T>().ToListAsync();
		}

        public async Task<T?> GetByIdAsync(int id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }
        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task AddAsync(T entity)
            => await _dbContext.AddAsync(entity);

        public void Update(T entity)
            => _dbContext.Update(entity);

        public void Delete(T entity)
            => _dbContext.Remove(entity);
    }
}
