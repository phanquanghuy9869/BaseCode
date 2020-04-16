using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.DataAccess.Repo.EFRepos.Kpi
{
    public interface ICriterionCatalogRepository : IGridRepository<CriterionCatalogDTO, KpiCriterionCatalogPaginFilterModelDTO>
    {
        IEnumerable<CriterionCatalogDTO> GetCriterionCatalog();
    }

    public class CriterionCatalogRepository : BaseGridRepository<Kpi_CriterionCatalog, CriterionCatalogDTO, KpiCriterionCatalogPaginFilterModelDTO>, ICriterionCatalogRepository
    {
        private readonly IGenericRepository<Kpi_CriterionCatalog> _criterionCatalogGRepo;

        public CriterionCatalogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._criterionCatalogGRepo = this._unitOfWork.GetDataRepository<Kpi_CriterionCatalog>();
        }

        public IEnumerable<CriterionCatalogDTO> GetCriterionCatalog()
        {
            return this._criterionCatalogGRepo.SelectAll().ProjectTo<CriterionCatalogDTO>().ToList();
        }

        public override IEnumerable<CriterionCatalogDTO> GetPaging(KpiCriterionCatalogPaginFilterModelDTO pagingModel)
        {
            var query = this.PagingFilter(_repo.SelectAll(), pagingModel);
            var catl = this._repo.SelectAll();
            var res = from a in query
                      join b in catl on a.ParentId equals b.Id into gj
                      from rs in gj.DefaultIfEmpty()
                      select new { cata = a, ParentName = rs.CriterionTitle };
            return res.OrderBy(x => x.cata.Id).Skip(pagingModel.Start - 1).Take(pagingModel.Length).Select(x => new CriterionCatalogDTO
            {
                Id = x.cata.Id,
                CategoryId = x.cata.CategoryId,
                CategoryName = x.cata.CategoryName,
                Code = x.cata.Code,
                CriterionLevel = x.cata.CriterionLevel,
                CriterionTitle = x.cata.CriterionTitle,
                IsFolder = x.cata.IsFolder,
                IsMinus = x.cata.IsMinus,
                MaximumPoint = x.cata.MaximumPoint,
                MinimumPoint = x.cata.MinimumPoint,
                ParentId = x.cata.ParentId,
                ParentName = x.ParentName
            }).ToList();
        }

        protected override IQueryable<Kpi_CriterionCatalog> PagingFilter(IQueryable<Kpi_CriterionCatalog> query, KpiCriterionCatalogPaginFilterModelDTO pagingModel)
        {
            if (!string.IsNullOrWhiteSpace(pagingModel.Code))
            {
                query = query.Where(x => x.Code.Contains(pagingModel.Code));
            }
            if (!string.IsNullOrWhiteSpace(pagingModel.CriterionTitle))
            {
                query = query.Where(x => x.CriterionTitle.Contains(pagingModel.CriterionTitle));
            }
            return query;
        }
    }
}
