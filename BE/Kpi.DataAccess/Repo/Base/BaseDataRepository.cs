using System;

namespace Kpi.DataAccess.Repo.Base
{
    public interface IDataRepository<TEntity> where TEntity : class
    {
    }

    public class BaseDataRepository<TEntity> : IDataRepository<TEntity>, IDisposable where TEntity : class
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseDataRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual void Dispose()
        {
            if (_unitOfWork != null && _unitOfWork is IDisposable)
            {
                (_unitOfWork as IDisposable).Dispose();
            }
        }
    }
}
