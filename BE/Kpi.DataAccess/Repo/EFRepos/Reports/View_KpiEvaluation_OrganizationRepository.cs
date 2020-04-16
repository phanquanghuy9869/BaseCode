using System.Collections.Generic;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using System.Linq;
using System;

namespace Kpi.DataAccess.Repo.EFRepos.Reports
{
    public interface IView_KpiEvaluation_OrganizationRepository
    {
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        IEnumerable<View_KpiEvaluation_OrganizationDTO> Search(View_KpiEvaluation_OrganizationFilterModelDTO filter);
    }

    public class View_KpiEvaluation_OrganizationRepository : BaseDataRepository<View_KpiEvaluation_OrganizationDTO>, IView_KpiEvaluation_OrganizationRepository
    {
        private readonly IGenericRepository<Org_Organization> _orgRepo;
        private readonly IGenericRepository<View_KpiEvaluation_Organization> _repo;

        public View_KpiEvaluation_OrganizationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _orgRepo = _unitOfWork.GetDataRepository<Org_Organization>();
            _repo = _unitOfWork.GetDataRepository<View_KpiEvaluation_Organization>();
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

        public IEnumerable<View_KpiEvaluation_OrganizationDTO> Search(View_KpiEvaluation_OrganizationFilterModelDTO filter)
        {
            var query = this._repo.SelectWhere(x => x.YearMonth == filter.yearMonth && (x.OrganizationId == filter.orgId || x.DirectoryPath.StartsWith(filter.directoryPath)));
            if (filter.status > 0)
            {
                /* query = this._repo.SelectWhere(x => x.YearMonth == filter.yearMonth &&
                 (x.OrganizationId == filter.orgId || x.DirectoryPath.StartsWith(filter.directoryPath)) &&
                 x.StatusId == filter.status && x.Level1ManagerUserId == filter.newLevel1MngUserName);*/
                query = query.Where(x => x.StatusId == filter.status);
            }
            if (!string.IsNullOrWhiteSpace(filter.newLevel1MngUserName))
            {
                query = query.Where(x => x.Level2ManagerUserName == filter.newLevel1MngUserName);
            }
            if (!string.IsNullOrWhiteSpace(filter.newLevel2MngUserName))
            {
                query = query.Where(x => x.Level2ManagerUserName == filter.newLevel2MngUserName);
            }
            if (!string.IsNullOrWhiteSpace(filter.submitNote))
            {
                query = query.Where(x => x.SubmitNote.Contains(filter.submitNote));
            }

            var lst = query.OrderBy(x => x.NumberOrder).ThenBy(x => x.DirectoryPath).ThenBy(x => x.DirectoryPath).Select(x => new View_KpiEvaluation_OrganizationDTO
            {
                Id = x.Id,
                DirectoryPath = x.DirectoryPath,
                EmployeeName = x.EmployeeName,
                EmployeeJobTitle = x.EmployeeJobTitle,
                Organization = x.Organization,
                EmpKpiPoint = x.EmpKpiPoint,
                StatusName = x.StatusName,
                EmpKpiClassification = x.EmpKpiClassification,
                FinalKpiPoint = x.FinalKpiPoint,
                FinalKpiClassification = x.FinalKpiClassification,
                FinalKpiComment = x.FinalKpiComment,
                Code = x.Code,
                Level1ManagerKpiPoint = x.Level1ManagerKpiPoint,
                Level1ManagerKpiClassification = x.Level1ManagerKpiClassification,
                HrKpiPoint = x.HrKpiPoint,
                HrKpiPointClassification = x.HrKpiPointClassification,
                reportClassification = x.reportClassification,
                reportPoint = x.reportPoint,
                SubmitNote = x.SubmitNote
            }).ToList();

            var ret = new List<View_KpiEvaluation_OrganizationDTO>();

            if (lst.Count > 0)
            {
                // ten phong tim kiem
                var queryOrg = this._orgRepo.SelectWhere(x => x.Id == filter.orgId).FirstOrDefault();
                if (queryOrg != null)
                {
                    ret.Add(new View_KpiEvaluation_OrganizationDTO()
                    {
                        EmployeeName = queryOrg.Name,
                        OrganizationId = queryOrg.Id,
                        Id = -1, // phong ban
                        ColSpan = 18
                    });
                }

                var currNo = 1;
                // duong dan thu muc hien tai
                var currOrgPath = filter.directoryPath;
                foreach (var item in lst)
                {
                    if (item.DirectoryPath != currOrgPath)
                    {
                        currOrgPath = item.DirectoryPath;
                        ret.Add(new View_KpiEvaluation_OrganizationDTO()
                        {
                            EmployeeName = item.Organization,
                            OrganizationId = item.OrganizationId,
                            Id = -1, // phong ban
                            ColSpan = 18
                        });
                    }
                    item.No = currNo;
                    ret.Add(item);
                    currNo++;
                }
            }
            return ret;
        }

    }
}
