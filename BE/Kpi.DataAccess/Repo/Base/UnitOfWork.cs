using Kpi.DataAccess.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.DataAccess.Repo.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IDatabaseTransaction BeginTransaction();
        IGenericRepository<TEntity> GetDataRepository<TEntity>() where TEntity : class;
        void SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; set; }
        private Dictionary<Type, object> _repositories;

        public UnitOfWork()
        {
            Context = new BRG_KpiEntities();
            _repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<TEntity> GetDataRepository<TEntity>() where TEntity : class
        {
            var key = typeof(TEntity);

            if (_repositories.TryGetValue(key, out object repository))
            {
                return repository as IGenericRepository<TEntity>;
            }

            repository = new GenericEFRepository<TEntity>(Context);
            _repositories.Add(typeof(TEntity), repository);
            return repository as IGenericRepository<TEntity>;
        }

        public IDatabaseTransaction BeginTransaction()
        {
            return new DatabaseTransaction(Context.Database.BeginTransaction());
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }

    }
}