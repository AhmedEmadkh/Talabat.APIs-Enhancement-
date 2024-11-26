using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Repositories.Contract
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		public Task<T?> GetByIdAsync(int id);
		public Task<IEnumerable<T>> GetAllAsync();
	}
}
