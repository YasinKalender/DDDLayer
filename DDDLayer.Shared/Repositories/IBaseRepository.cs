using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Shared.Interfaces
{
    public interface IBaseRepository<T> where T : class,new()
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(string sqlQuery, object? param = null);
        Task<T> GetSingleAsync(string sqlQuery, object? param = null);
        Task<T> GetByIdAsync<Tkey>(Tkey id);

        Task<int> AddAsync(T item);
        Task<int> InsertMultipleAsync(IEnumerable<T> entities);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync<TKey>(TKey id, bool softDelete = true);
        Task<bool> DeleteAsync(T entity, bool softDelete = true);



    }
}
