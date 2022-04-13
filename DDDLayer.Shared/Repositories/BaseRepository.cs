using Dapper;
using Dapper.Contrib.Extensions;
using DDDLayer.Shared.Interfaces;
using DDDLayer.Shared.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Shared.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> AddAsync(T item)
        {
            return _unitOfWork.Connection.InsertAsync(item);
        }

        public async Task<bool> DeleteAsync<TKey>(TKey id, bool softDelete = true)
        {
            return await DeleteAsync(id, softDelete);
        }

        public Task<bool> DeleteAsync(T entity, bool softDelete = true)
        {
            return _unitOfWork.Connection.DeleteAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _unitOfWork.Connection.GetAllAsync<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(string sqlQuery, object? param = null)
        {
            return await _unitOfWork.Connection.QueryAsync<T>(sqlQuery, param);
        }

        public async Task<T> GetSingleAsync(string sqlQuery, object? param = null)
        {
           return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<T>(sqlQuery, param);

        }
     

        public async Task<T> GetByIdAsync<Tkey>(Tkey id)
        {
           return await _unitOfWork.Connection.GetAsync<T>(id);
        }

        public Task<int> InsertMultipleAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(T entity)
        {
            
            return _unitOfWork.Connection.UpdateAsync<T>(entity);
        }
    }
}
