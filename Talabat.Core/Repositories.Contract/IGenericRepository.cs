using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		public Task<T?> GetByIdAsync(int id);
		public Task<IReadOnlyList<T>> GetAllAsync();

        public Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec);
        public Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
    }
}
