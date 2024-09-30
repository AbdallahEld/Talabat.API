using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		#region Without Specifications
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		void Delete(T item);
		void Update(T item);
		#endregion
		#region With Specifications
		public Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
		public Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec);
		public Task<int> GetCountWithSpecAsync (ISpecifications<T> Spec);
		#endregion
		Task Add(T item);
	}
}
