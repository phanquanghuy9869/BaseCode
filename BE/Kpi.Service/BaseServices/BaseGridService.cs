using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Service.BaseServices
{
    public interface IGridService<TEntityDTo, TPagingModel> : IDisposable
       //where TEntity : class, IEntityDTO
       where TEntityDTo : class
       where TPagingModel : IGridFilterModelDTO
    {
        TEntityDTo Get(int id);
        IEnumerable<TEntityDTo> GetAll();
        IEnumerable<TEntityDTo> GetPaging(TPagingModel pagingModel);
        TEntityDTo Add(TEntityDTo entityDTo);
        IEnumerable<TEntityDTo> AddRange(IEnumerable<TEntityDTo> data);
        int Count(TPagingModel pagingModel);
        void Update(TEntityDTo entityDto);
        void Delete(TEntityDTo entityDto);
    }

    public class BaseGridService<IRepository, TEntityDTo, TPagingModel> : IGridService<TEntityDTo, TPagingModel>
        //where TEntity : class, IEntityDTO
        where TEntityDTo : class
        where TPagingModel : IGridFilterModelDTO
        where IRepository : IGridRepository<TEntityDTo, TPagingModel>
    {
        protected readonly IRepository _gridRepository;

        public BaseGridService(IRepository gridRepository)
        {
            _gridRepository = gridRepository;
        }

        public virtual TEntityDTo Add(TEntityDTo entityDTo)
        {
            return _gridRepository.Add(entityDTo);
        }

        public IEnumerable<TEntityDTo> AddRange(IEnumerable<TEntityDTo> data)
        {
            return _gridRepository.AddRange(data);
        }

        public virtual int Count(TPagingModel pagingModel)
        {
            return _gridRepository.Count(pagingModel);
        }

        public virtual void Delete(TEntityDTo entityDto)
        {
            _gridRepository.Delete(entityDto);
        }

        public virtual TEntityDTo Get(int id)
        {
            return _gridRepository.Get(id);
        }

        public virtual IEnumerable<TEntityDTo> GetAll()
        {
            return _gridRepository.GetAll();
        }

        public virtual IEnumerable<TEntityDTo> GetPaging(TPagingModel pagingModel)
        {
            return _gridRepository.GetPaging(pagingModel);
        }

        public virtual void Update(TEntityDTo entityDto)
        {
            _gridRepository.Update(entityDto);
        }

        public void Dispose()
        {
            if (_gridRepository != null && _gridRepository is IDisposable)
            {
                (_gridRepository as IDisposable).Dispose();
            }
        }

    }
}
