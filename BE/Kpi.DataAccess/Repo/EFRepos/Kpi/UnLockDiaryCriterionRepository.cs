using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;

namespace Kpi.DataAccess.Repo.EFRepos.Kpi
{
    public interface IUnLockDiaryCriterionRepository 
    {
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        IEnumerable<UnLockDiaryCriterionDTO> Search(UnLockDiaryCriterionFilterModelDTO filter);
    }
    public class UnLockDiaryCriterionRepository : BaseDataRepository<UnLockDiaryCriterionDTO>, IUnLockDiaryCriterionRepository
    {
        private readonly IGenericRepository<Org_Organization> _orgRepo;
        private readonly IGenericRepository<UnLockDiaryCriterionDTO> _repo;

        public UnLockDiaryCriterionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _orgRepo = _unitOfWork.GetDataRepository<Org_Organization>();
            _repo = _unitOfWork.GetDataRepository<UnLockDiaryCriterionDTO>();
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgs()
        {
            // get kpi belong to this config  
            var query = this._orgRepo.SelectAll();
            return query.OrderBy(x => x.Id).Select(x => new OrgInfoModelDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OrganizationTypeID = x.OrganizationTypeID,
                NodeID = x.NodeID,
                DirectoryPath = x.DirectoryPath
            }).ToList();
        }

        public IEnumerable<UnLockDiaryCriterionDTO> Search(UnLockDiaryCriterionFilterModelDTO filter)
        {
            var query = this._repo.SelectWhere(x => x.YearMonth == filter.yearMonth && (x.OrganizationId == filter.orgId || x.DirectoryPath.StartsWith(filter.directoryPath)));
            var lst = query.OrderBy(x => x.DirectoryPath).Select(x => new UnLockDiaryCriterionDTO
            {
                STT = x.STT,
                /*DirectoryPath = x.DirectoryPath,
                TongSoNhanVien = x.TongSoNhanVien,
                Tong = x.Tong,
                A = x.A,
                A_ = x.A_,
                B_ = x.B_,
                B = x.B,
                C = x.C,
                Organization = x.Organization,*/

            }).ToList();

            return lst;

        }
    }
}
