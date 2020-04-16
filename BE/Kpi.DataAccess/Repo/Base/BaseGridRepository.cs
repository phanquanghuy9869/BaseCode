using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.Core.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Kpi.DataAccess.Repo.Base
{
    public interface IGridRepository<TEntityDTo, TPagingModel>
         //where TEntity : class, IEntityDTO
         where TEntityDTo : class
         where TPagingModel : IGridFilterModelDTO
    {
        TEntityDTo Get(int id);
        IEnumerable<TEntityDTo> GetAll();
        IEnumerable<TEntityDTo> GetPaging(TPagingModel pagingModel);
        TEntityDTo Add(TEntityDTo entityDTo);
        IEnumerable<TEntityDTo> AddRange(IEnumerable<TEntityDTo> entityDTOs);
        int Count(TPagingModel pagingModel);
        void Update(TEntityDTo entityDto);
        void Delete(TEntityDTo entityDto);
    }

    public class BaseGridRepository<TEntity, TEntityDTo, TPagingModel> : BaseDataRepository<TEntity>, IGridRepository<TEntityDTo, TPagingModel>
        where TEntity : class, IEntityDTO
        where TEntityDTo : class
        where TPagingModel : IGridFilterModelDTO
    {
        protected IGenericRepository<TEntity> _repo;

        public BaseGridRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repo = unitOfWork.GetDataRepository<TEntity>();
        }

        public virtual TEntityDTo Get(int id)
        {
            var result = _repo.Get(x => x.Id == id);
            return AutoMapperHelper.Map<TEntity, TEntityDTo>(result);
        }

        public virtual IEnumerable<TEntityDTo> GetAll()
        {
            return _repo.SelectAll().ProjectTo<TEntityDTo>().ToList();
        }

        public virtual IEnumerable<TEntityDTo> GetPaging(TPagingModel pagingModel)
        {
            var query = this.PagingFilter(_repo.SelectAll(), pagingModel);
            return query.OrderBy(x => x.Id).Skip(pagingModel.Start - 1).Take(pagingModel.Length).ProjectTo<TEntityDTo>().ToList();
        }

        public virtual TEntityDTo Add(TEntityDTo entityDTo)
        {
            var entity = AutoMapperHelper.Map<TEntityDTo, TEntity>(entityDTo);
            var result = _repo.Add(entity);
            _unitOfWork.SaveChanges();
            return AutoMapperHelper.Map<TEntity, TEntityDTo>(result);
        }

        public virtual IEnumerable<TEntityDTo> AddRange(IEnumerable<TEntityDTo> entityDTOs)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TEntityDTo, TEntity>();
            });
            IMapper mapper = config.CreateMapper();
            IEnumerable<TEntity> entities = mapper.Map<List<TEntityDTo>, List<TEntity>>(entityDTOs as List<TEntityDTo>);
            entities = _repo.AddRange(entities);
            _unitOfWork.SaveChanges();

            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TEntity, TEntityDTo>();
            });
            mapper = config.CreateMapper();
            var result = mapper.Map<List<TEntity>, List<TEntityDTo>>(entities as List<TEntity>);
            return result;
        }

        public virtual int Count(TPagingModel pagingModel)
        {
            var query = this.PagingFilter(_repo.SelectAll(), pagingModel);
            return query.Count();
        }

        public virtual void Update(TEntityDTo entityDto)
        {
            var entity = AutoMapperHelper.Map<TEntityDTo, TEntity>(entityDto);
            _repo.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public virtual void Delete(TEntityDTo entityDto)
        {
            var entity = AutoMapperHelper.Map<TEntityDTo, TEntity>(entityDto);
            _repo.Delete(entity);
            _unitOfWork.SaveChanges();
        }

        protected virtual IQueryable<TEntity> PagingFilter(IQueryable<TEntity> query, TPagingModel pagingModel)
        {
            return query;
        }

    }

}
