using DDDLayer.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Shared.Uow
{
    public interface IUnitOfWork
    {

        Guid TransactionId { get; }
        DbConnection Connection { get; }
        DbTransaction Transaction { get; }

        IBaseRepository<T> GetBaseRepository<T>() where T : class, new();

        void Begin();
        void Begin(IsolationLevel isolationLevel);
        void Commit();
        void RollBack();

        Task BeginAsync();
        Task BeginAsync(IsolationLevel isolationLevel);
        Task CommitAsync();
        Task RollBackAsync();


    }
}
