using DDDLayer.Shared.Interfaces;
using DDDLayer.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Shared.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DbConnection dbConnection)
        {
            Connection = dbConnection;
        }


        public Guid TransactionId { get; private set; }

        public DbConnection Connection { get; private set; }

        public DbTransaction Transaction { get; private set; }

        public void Begin()
        {
            Transaction = Connection.BeginTransaction();
            TransactionId = Guid.NewGuid();
        }

        public void Begin(IsolationLevel isolationLevel)
        {
            Transaction = Connection.BeginTransaction(isolationLevel);
        }

        public async Task BeginAsync()
        {
            Transaction =await Connection.BeginTransactionAsync();
        }

        public async Task BeginAsync(IsolationLevel isolationLevel)
        {
            Transaction = await Connection.BeginTransactionAsync(isolationLevel);
        }

        public void Commit()
        {
            Transaction?.Commit();
        }

        public async Task CommitAsync()
        {
            if(Transaction != null)
                await Transaction?.CommitAsync();
        }

        public IBaseRepository<T> GetBaseRepository<T>() where T : class, new()
        {
            return new BaseRepository<T>(this);
        }

        public void RollBack()
        {
            Transaction.Rollback();
        }

        public async Task RollBackAsync()
        {
            await Transaction.RollbackAsync();
        }
    }
}
