using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.DataAccess.Repo.Base
{
    public interface IDatabaseTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }

    public class DatabaseTransaction : IDatabaseTransaction
    {
        private DbContextTransaction _transaction;

        public DatabaseTransaction(DbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}
