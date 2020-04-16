using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.Core.Extensions;
using Kpi.Core.Helper;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using Kpi.DataAccess.Repo.EFRepos.Business;

namespace Kpi.DataAccess.Repo.EFRepos.Kpi
{
    public interface IKpiCriterionTypeRepository : IGridRepository<KpiCriterionTypeDTO, KpiCriterionTypePaginFilterModelDTO>
    {
        IEnumerable<CriterionCatalogDTO> GetKpiCatalogs();
    }

    public class KpiCriterionTypeRepository : BaseGridRepository<Kpi_CriterionType, KpiCriterionTypeDTO, KpiCriterionTypePaginFilterModelDTO>, IKpiCriterionTypeRepository
    {
        IGenericRepository<Kpi_CriterionCatalog> _catalogRepo;
        IGenericRepository<Kpi_CriterionTypeCatalog> _crTypeCatlRepo;

        public KpiCriterionTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _catalogRepo = unitOfWork.GetDataRepository<Kpi_CriterionCatalog>();
            _crTypeCatlRepo = unitOfWork.GetDataRepository<Kpi_CriterionTypeCatalog>();
        }

        public IEnumerable<CriterionCatalogDTO> GetKpiCatalogs()
        {
            var res = this._catalogRepo.SelectAll().ProjectTo<CriterionCatalogDTO>().ToList();
            return res;
        }

        public override KpiCriterionTypeDTO Add(KpiCriterionTypeDTO entityDTo)
        {
            using (var trans = this._unitOfWork.BeginTransaction())
            {
                if (string.IsNullOrWhiteSpace(entityDTo.Code) || string.IsNullOrWhiteSpace(entityDTo.Name))
                {
                    throw new Exception("Chưa nhập thông tin mã/tên bộ tiêu chuẩn");
                }

                var entity = AutoMapperHelper.Map<KpiCriterionTypeDTO, Kpi_CriterionType>(entityDTo);
                entity.Id = _repo.SelectAll().Max(x => x.Id) + 1;
                var result = _repo.Add(entity);
                if (entityDTo.Catalogs != null && entityDTo.Catalogs.Count > 0)
                {
                    foreach (var item in entityDTo.Catalogs)
                    {
                        item.CriterionTypeId = result.Id;
                        var crTypeCatlEnt = AutoMapperHelper.Map<Kpi_CriterionTypeCatalogDTO, Kpi_CriterionTypeCatalog>(item);
                        _crTypeCatlRepo.Add(crTypeCatlEnt);
                    }
                }

                _unitOfWork.SaveChanges();

                trans.Commit();
                return AutoMapperHelper.Map<Kpi_CriterionType, KpiCriterionTypeDTO>(result);
            }
        }

        public override void Update(KpiCriterionTypeDTO entityDto)
        {
            using (var trans = this._unitOfWork.BeginTransaction())
            {
                var oldDto = this._repo.SelectWhere(x => x.Id == entityDto.Id).FirstOrDefault();
                if (oldDto == null)
                {
                    throw new Exception("Không tìm thấy bộ tiêu chuẩn id='" + entityDto.Id + "'");
                }
                oldDto.Code = entityDto.Code;
                oldDto.Name = entityDto.Name;

                this._repo.UpdateByProperties(oldDto,
                    new List<Expression<Func<Kpi_CriterionType, object>>> { x => x.Code, x => x.Name, });
                _unitOfWork.SaveChanges();

                #region catalog
                var oldCritCat = _crTypeCatlRepo.SelectWhere(x => x.CriterionTypeId == entityDto.Id).ToList();
                var lstAddCritCat = new List<Kpi_CriterionTypeCatalogDTO>();
                if (entityDto.Catalogs != null && entityDto.Catalogs.Any())
                {
                    foreach (var item in entityDto.Catalogs)
                    {
                        if ((oldCritCat.Count > 0 && !oldCritCat.Exists(u => u.Id == item.Id)) || oldCritCat.Count == 0)
                        {
                            lstAddCritCat.Add(item);
                        }
                    }
                }

                var lstDeleteCritCat = new List<Kpi_CriterionTypeCatalog>();
                if (oldCritCat.Count > 0)
                {
                    foreach (var item in oldCritCat)
                    {
                        if ((entityDto.Catalogs != null && entityDto.Catalogs.Any() && !entityDto.Catalogs.Exists(u => u.Id == item.Id)) || entityDto.Catalogs == null || !entityDto.Catalogs.Any())
                        {
                            lstDeleteCritCat.Add(item);
                        }
                    }
                }

                if (lstDeleteCritCat.Count > 0)
                {
                    foreach (var item in lstDeleteCritCat)
                    {
                        _crTypeCatlRepo.Delete(item);
                        _unitOfWork.SaveChanges();
                    }
                }

                if (lstAddCritCat.Count > 0)
                {
                    foreach (var item in lstAddCritCat)
                    {
                        item.CriterionTypeId = entityDto.Id;
                        var crTypeCatlEnt = AutoMapperHelper.Map<Kpi_CriterionTypeCatalogDTO, Kpi_CriterionTypeCatalog>(item);
                        _crTypeCatlRepo.Add(crTypeCatlEnt);
                        _unitOfWork.SaveChanges();
                    }
                }
                #endregion

                trans.Commit();
            }
        }

        protected override IQueryable<Kpi_CriterionType> PagingFilter(IQueryable<Kpi_CriterionType> query, KpiCriterionTypePaginFilterModelDTO pagingModel)
        {
            if (!string.IsNullOrWhiteSpace(pagingModel.Code))
            {
                query = query.Where(x => x.Code.Contains(pagingModel.Code));
            }
            if (!string.IsNullOrWhiteSpace(pagingModel.Name))
            {
                query = query.Where(x => x.Name.Contains(pagingModel.Name));
            }
            return query;
        }

        public override KpiCriterionTypeDTO Get(int id)
        {
            var result = _repo.Get(x => x.Id == id);
            if (result != null)
            {
                var res = AutoMapperHelper.Map<Kpi_CriterionType, KpiCriterionTypeDTO>(result);
                res.Catalogs = GetCatalogs(id);
                return res;
            }

            return null;
        }

        private List<Kpi_CriterionTypeCatalogDTO> GetCatalogs(int id)
        {
            var res = new List<Kpi_CriterionTypeCatalogDTO>();
            var resDb = _crTypeCatlRepo.SelectWhere(x => x.CriterionTypeId == id);
            var catalogs = _catalogRepo.SelectAll();
            var query = from ctc in resDb
                        join ct in catalogs on ctc.CriterionCatalogId equals ct.Id into gj
                        from ret in gj.DefaultIfEmpty()
                        select new { ctc, name = ret.CriterionTitle };
            res = query.Select(x => new Kpi_CriterionTypeCatalogDTO()
            {
                CatalogName = x.name,
                CriterionCatalogId = x.ctc.CriterionCatalogId,
                CriterionTypeId = x.ctc.CriterionTypeId,
                Id = x.ctc.Id,
                StartPoint = x.ctc.StartPoint
            }).ToList();
            return res;
        }
    }
}
