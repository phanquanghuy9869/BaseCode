using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using Kpi.Core.Helper;
using System.Linq.Expressions;

namespace Kpi.DataAccess.Repo.EFRepos.Orgs
{
    public interface IEmploymentHistoryRepository : IGridRepository<EmploymentHistoryDTO, EmploymentHistoryFilterModelDTO>
    {
    }

    public class EmploymentHistoryRepository : BaseGridRepository<EmpTransfer, EmploymentHistoryDTO, EmploymentHistoryFilterModelDTO>, IEmploymentHistoryRepository
    {
        protected IGenericRepository<Org_UserOrg> _usrRepo;
        protected IGenericRepository<Org_Organization> _orgRepo;
        protected IGenericRepository<Org_JobTitle> _jobTitleRepo;

        public EmploymentHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _usrRepo = unitOfWork.GetDataRepository<Org_UserOrg>();
            _orgRepo = unitOfWork.GetDataRepository<Org_Organization>();
            _jobTitleRepo = unitOfWork.GetDataRepository<Org_JobTitle>();
        }

        public int Count(EmploymentHistoryFilterModelDTO filter)
        {
            return _repo.SelectWhere(x => filter.UserId == x.UserId).Count();
        }

        public IEnumerable<EmploymentHistoryDTO> GetPaging(EmploymentHistoryFilterModelDTO filter)
        {
            var res = new List<EmploymentHistoryDTO>();
            // lay user
            var usr = _usrRepo.SelectWhere(x => x.Id == filter.UserId).FirstOrDefault();

            // danh sach dieu chuyen
            var query = _repo.SelectWhere(x => filter.UserId == x.UserId).OrderBy(x => x.TransferDate).Skip(filter.Start - 1).Take(filter.Length).ToList();
            if (query.Count > 0)
            {
                if (usr != null)
                {
                    var tmp0 = new EmploymentHistoryDTO();
                    tmp0.fromDate = usr.StartWorkDate;
                    tmp0.toDate = query[0].TransferDate;

                    var orgId0 = query[0].OldOrgId;
                    var org0 = _orgRepo.SelectWhere(x => x.Id == orgId0).FirstOrDefault();
                    tmp0.orgName = org0 != null ? org0.Name : "";

                    var jobTId0 = query[0].OldJobTitleId;
                    var jobTitle0 = _jobTitleRepo.SelectWhere(x => x.Id == jobTId0).FirstOrDefault();
                    tmp0.jobTitle = jobTitle0 != null ? jobTitle0.Title : "";

                    res.Add(tmp0);

                    for (int i = 0; i < query.Count; i++)
                    {
                        var tmp = new EmploymentHistoryDTO();
                        tmp.fromDate = query[i].TransferDate;
                        tmp.toDate = query[i].ToDate;

                        var orgId = query[i].NewOrgId;
                        var org = _orgRepo.SelectWhere(x => x.Id == orgId).FirstOrDefault();
                        tmp.orgName = org != null ? org.Name : "";

                        var jobTId = query[i].NewJobTitleId;
                        var jobTitle = _jobTitleRepo.SelectWhere(x => x.Id == jobTId).FirstOrDefault();
                        tmp.jobTitle = jobTitle != null ? jobTitle.Title : "";

                        res.Add(tmp);
                    }
                }
                else
                {
                    throw new Exception("Không tìm thấy nhân viên 'id=" + filter.UserId + "'");
                }

            }
            else
            {
                if (usr != null)
                {
                    var tmp = new EmploymentHistoryDTO();
                    tmp.fromDate = usr.StartWorkDate;
                    var org = _orgRepo.SelectWhere(x => x.Id == usr.OrgId).FirstOrDefault();
                    tmp.orgName = org != null ? org.Name : "";
                    var jobTitle = _jobTitleRepo.SelectWhere(x => x.Id == usr.JobTitleId).FirstOrDefault();
                    tmp.jobTitle = jobTitle != null ? jobTitle.Title : "";
                    res.Add(tmp);
                }
                else
                {
                    throw new Exception("Không tìm thấy nhân viên 'id=" + filter.UserId + "'");
                }
            }
            return res;
        }
    }
}
