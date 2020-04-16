using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using Kpi.Core.Helper;
using System.Linq.Expressions;
using Kpi.DataAccess.Repo.EFRepos.Kpi;

namespace Kpi.DataAccess.Repo.EFRepos.Orgs
{
    public interface IEmpTransferRepository : IGridRepository<EmpTransferDTO, EmpTransferFilterModelDTO>
    {
        IEnumerable<Org_Organization> GetAllOrgs();
    }

    public class EmpTransferRepository : BaseGridRepository<EmpTransfer, EmpTransferDTO, OrgInfoModelFilterModelDTO>, IEmpTransferRepository
    {
        private readonly IGenericRepository<Org_Organization> _orgRepo;
        private readonly IGenericRepository<Org_UserOrg> _userRepo;
        private readonly IGenericRepository<Org_JobTitle> _jobTitleRepo;
        private readonly IGenericRepository<Sys_EventDiaryConfig> _evntCfgRepo;
        private readonly IEvenDiaryConfigRepository _evntReposi;

        public EmpTransferRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _evntReposi = new EventDiaryConfigRepository(unitOfWork);
            _orgRepo = _unitOfWork.GetDataRepository<Org_Organization>();
            _userRepo = _unitOfWork.GetDataRepository<Org_UserOrg>();
            _jobTitleRepo = _unitOfWork.GetDataRepository<Org_JobTitle>();
            _evntCfgRepo = _unitOfWork.GetDataRepository<Sys_EventDiaryConfig>();
        }

        public int Count(EmpTransferFilterModelDTO filter)
        {
            // gan null cho date
            filter.FromDate = filter.FromDate.HasValue && filter.FromDate.Value.Year < 2019 ? null : filter.FromDate;
            filter.ToDate = filter.ToDate.HasValue && filter.ToDate.Value.Year < 2019 ? null : filter.ToDate;

            // danh sach user theo ten
            var filterdUsers = _userRepo.SelectWhere(x => (!string.IsNullOrEmpty(filter.EmpName) && x.UserFullName.Contains(filter.EmpName)) || string.IsNullOrEmpty(filter.EmpName));

            var transfers = _repo.SelectWhere(x => ((filter.FromDate != null && x.TransferDate >= filter.FromDate) || filter.FromDate == null)
             && ((filter.ToDate != null && x.TransferDate <= filter.ToDate) || filter.ToDate == null));

            return transfers.Join(filterdUsers, r => r.UserId, u => u.Id, (r, u) => new { trans = r, EmpFullName = u.UserFullName }).Count();
        }

        public IEnumerable<EmpTransferDTO> GetPaging(EmpTransferFilterModelDTO filter)
        {
            var jobTitles = _jobTitleRepo.SelectAll();
            var users = _userRepo.SelectAll();
            var orgs = _orgRepo.SelectAll();

            // gan null cho date
            filter.FromDate = filter.FromDate.HasValue && filter.FromDate.Value.Year < 2019 ? null : filter.FromDate;
            filter.ToDate = filter.ToDate.HasValue && filter.ToDate.Value.Year < 2019 ? null : filter.ToDate;

            // danh sach user theo ten
            var filterdUsers = _userRepo.SelectWhere(x => (!string.IsNullOrEmpty(filter.EmpName) && x.UserFullName.Contains(filter.EmpName)) || string.IsNullOrEmpty(filter.EmpName));

            // danh sach dieu chuyen
            var transfers = _repo.SelectWhere(x => ((filter.FromDate != null && x.TransferDate >= filter.FromDate) || filter.FromDate == null)
            && ((filter.ToDate != null && x.TransferDate <= filter.ToDate) || filter.ToDate == null));

            var query = from a in transfers
                        join b in filterdUsers on a.UserId equals b.Id
                        join c in jobTitles on a.OldJobTitleId equals c.Id into gc
                        from rc in gc.DefaultIfEmpty()
                        join d in jobTitles on a.NewJobTitleId equals d.Id into gd
                        from rd in gd.DefaultIfEmpty()
                        join e in users on a.OldLevel1MngId equals e.Id into ge
                        from re in ge.DefaultIfEmpty()
                        join f in users on a.NewLevel1MngId equals f.Id into gf
                        from rf in gf.DefaultIfEmpty()
                        join g in users on a.OldLevel2MngId equals g.Id into gg
                        from rg in gg.DefaultIfEmpty()
                        join h in users on a.NewLevel2MngId equals h.Id into gh
                        from rh in gh.DefaultIfEmpty()
                        join i in orgs on a.OldOrgId equals i.Id into gi
                        from ri in gi.DefaultIfEmpty()
                        join j in orgs on a.NewOrgId equals j.Id into gj
                        from rj in gj.DefaultIfEmpty()
                        select new
                        {
                            trans = a,
                            EmpFullName = b.UserFullName,
                            OldJobTitleName = rc.Title,
                            NewJobTitleName = rd.Title,
                            OldLv1MngName = re.UserFullName,
                            NewLv1MngName = rf.UserFullName,
                            OldLv2MngName = rg.UserFullName,
                            NewLv2MngName = rh.UserFullName,
                            OldOrgName = ri.Name,
                            NewOrgName = rj.Name
                        };

            var res = query.OrderBy(x => x.trans.Id).Skip(filter.Start - 1).Take(filter.Length)
             .Select(x => new EmpTransferDTO
             {
                 Id = x.trans.Id,
                 Code = x.trans.Code,
                 TransferDate = x.trans.TransferDate,
                 UserId = x.trans.UserId,
                 OldJobTitleId = x.trans.OldJobTitleId,
                 NewJobTitleId = x.trans.NewJobTitleId,
                 OldOrgId = x.trans.OldOrgId,
                 NewOrgId = x.trans.NewOrgId,
                 CreatedBy = x.trans.CreatedBy,
                 CreatedDate = x.trans.CreatedDate,
                 UpdatedBy = x.trans.UpdatedBy,
                 UpdatedDate = x.trans.UpdatedDate,
                 OldLevel1MngId = x.trans.OldLevel1MngId,
                 NewLevel1MngId = x.trans.NewLevel1MngId,
                 OldLevel2MngId = x.trans.OldLevel2MngId,
                 NewLevel2MngId = x.trans.NewLevel2MngId,
                 Note = x.trans.Note,
                 EmpFullName = x.EmpFullName,
                 OldJobTitleName = x.OldJobTitleName,
                 NewJobTitleName = x.NewJobTitleName,
                 OldOrgName = x.OldOrgName,
                 NewOrgName = x.NewOrgName,
                 OldLv1MngName = x.OldLv1MngName,
                 NewLv1MngName = x.NewLv1MngName,
                 OldLv2MngName = x.OldLv2MngName,
                 NewLv2MngName = x.NewLv2MngName

             }).ToList();
            return res;
        }

        public override EmpTransferDTO Add(EmpTransferDTO entityDTo)
        {
            using (var trans = _unitOfWork.BeginTransaction())
            {
                if (string.IsNullOrWhiteSpace(entityDTo.Code))
                {
                    throw new Exception("Chưa nhập mã");
                }

                var transfD = _repo.SelectWhere(x => !string.IsNullOrEmpty(x.Code) && x.Code.ToLower() == entityDTo.Code.ToLower()).FirstOrDefault();
                if (transfD != null)
                {
                    throw new Exception("Mã điều chuyển đã tồn tại '" + entityDTo.Code + "'");
                }

                var entity = AutoMapperHelper.Map<EmpTransferDTO, EmpTransfer>(entityDTo);

                var result = _repo.Add(entity);
                _unitOfWork.SaveChanges();

                // cap nhat fromdate, todate
                UpdateFromToDate(entity.UserId.Value);

                // cap nhat bang user
                var user = _userRepo.SelectWhere(x => x.Id == entityDTo.UserId).FirstOrDefault();
                if (user != null && entityDTo.NewOrgId.HasValue)
                {
                    user.OrgId = entityDTo.NewOrgId.Value;
                    var org = _orgRepo.SelectWhere(x => x.Id == user.OrgId).FirstOrDefault();
                    user.OrgName = org != null ? org.Name : "";

                    user.JobTitleId = entityDTo.NewJobTitleId.Value;
                    var jobTitle = _jobTitleRepo.SelectWhere(x => x.Id == user.JobTitleId).FirstOrDefault();
                    user.JobTitle = jobTitle != null ? jobTitle.Title : "";

                    user.Level1ManagerId = entityDTo.NewLevel1MngId.Value;
                    user.Level2ManagerId = entityDTo.NewLevel2MngId.Value;

                    // tim cau hinh nksk moi --> bo
                    //var newEvnCfgQuery = this._evntCfgRepo.SelectWhere(x => x.Level1ManagerUserId == user.Level1ManagerId && x.OrgId == user.OrgId).FirstOrDefault();
                    //if (newEvnCfgQuery != null)
                    //{
                    //    user.EventDiaryConfigId = newEvnCfgQuery.Id;

                    //    this._userRepo.UpdateByProperties(user,
                    //        new List<Expression<Func<Org_UserOrg, object>>> { x => x.OrgId, x => x.OrgName, x => x.JobTitleId, x => x.JobTitle, x => x.Level1ManagerId, x => x.Level2ManagerId
                    //        , x => x.EventDiaryConfigId });
                    //    _unitOfWork.SaveChanges();
                    //}
                    //else
                    //{
                    this._userRepo.UpdateByProperties(user,
                        new List<Expression<Func<Org_UserOrg, object>>> { x => x.OrgId, x => x.OrgName, x => x.JobTitleId, x => x.JobTitle, x => x.Level1ManagerId, x => x.Level2ManagerId });
                    _unitOfWork.SaveChanges();
                    //}
                }

                trans.Commit();
                return AutoMapperHelper.Map<EmpTransfer, EmpTransferDTO>(result);
            }
        }

        public override void Update(EmpTransferDTO entityDto)
        {
            if (string.IsNullOrWhiteSpace(entityDto.Code))
            {
                throw new Exception("Chưa nhập mã");
            }

            var transfD = _repo.SelectWhere(x => x.Id != entityDto.Id && !string.IsNullOrEmpty(x.Code) && x.Code.ToLower() == entityDto.Code.ToLower()).FirstOrDefault();
            if (transfD != null)
            {
                throw new Exception("Mã điều chuyển đã tồn tại '" + entityDto.Code + "'");
            }

            var transf = _repo.SelectWhere(x => x.Id == entityDto.Id).FirstOrDefault();
            if (transf != null)
            {
                using (var trans = _unitOfWork.BeginTransaction())
                {
                    transf.Code = entityDto.Code;
                    transf.UserId = entityDto.UserId;
                    transf.TransferDate = entityDto.TransferDate;
                    transf.NewJobTitleId = entityDto.NewJobTitleId;
                    transf.NewOrgId = entityDto.NewOrgId;
                    transf.UpdatedBy = entityDto.UpdatedBy;
                    transf.UpdatedDate = DateTime.Now;
                    transf.NewLevel1MngId = entityDto.NewLevel1MngId;
                    transf.NewLevel2MngId = entityDto.NewLevel2MngId;
                    transf.Note = entityDto.Note;
                    this._repo.UpdateByProperties(transf,
                        new List<Expression<Func<EmpTransfer, object>>> { x => x.Code, x => x.TransferDate, x => x.UserId
                    , x => x.NewJobTitleId, x => x.NewOrgId, x => x.UpdatedBy, x => x.UpdatedDate
                    , x => x.NewLevel1MngId, x => x.NewLevel2MngId, x => x.Note});

                    _unitOfWork.SaveChanges();

                    // cap nhat bang user
                    var user = _userRepo.SelectWhere(x => x.Id == entityDto.UserId).FirstOrDefault();
                    if (user != null && entityDto.NewOrgId.HasValue)
                    {
                        user.OrgId = entityDto.NewOrgId.Value;
                        var org = _orgRepo.SelectWhere(x => x.Id == user.OrgId).FirstOrDefault();
                        user.OrgName = org != null ? org.Name : "";

                        user.JobTitleId = entityDto.NewJobTitleId.Value;
                        var jobTitle = _jobTitleRepo.SelectWhere(x => x.Id == user.JobTitleId).FirstOrDefault();
                        user.JobTitle = jobTitle != null ? jobTitle.Title : "";

                        user.Level1ManagerId = entityDto.NewLevel1MngId.Value;
                        user.Level2ManagerId = entityDto.NewLevel2MngId.Value;

                        // tim cau hinh nksk moi -> bo
                        //var newEvnCfgQuery = this._evntCfgRepo.SelectWhere(x => x.Level1ManagerUserId == user.Level1ManagerId && x.OrgId == user.OrgId).FirstOrDefault();
                        //if (newEvnCfgQuery != null)
                        //{
                        //    user.EventDiaryConfigId = newEvnCfgQuery.Id;

                        //    this._userRepo.UpdateByProperties(user,
                        //        new List<Expression<Func<Org_UserOrg, object>>> { x => x.OrgId, x => x.OrgName, x => x.JobTitleId, x => x.JobTitle, x => x.Level1ManagerId, x => x.Level2ManagerId
                        //        , x => x.EventDiaryConfigId });
                        //    _unitOfWork.SaveChanges();
                        //}
                        //else
                        //{
                        this._userRepo.UpdateByProperties(user,
                            new List<Expression<Func<Org_UserOrg, object>>> { x => x.OrgId, x => x.OrgName, x => x.JobTitleId, x => x.JobTitle, x => x.Level1ManagerId, x => x.Level2ManagerId });
                        _unitOfWork.SaveChanges();
                        //}
                    }

                    // cap nhat fromdate, todate
                    UpdateFromToDate(transf.UserId.Value);
                    trans.Commit();
                }
            }
            else
            {
                throw new Exception("Không tìm thấy dữ liệu điều chuyển 'id=" + entityDto.Id + "'");
            }
        }

        private void UpdateFromToDate(int userId)
        {
            #region Lay du lieu cu, sua fromdate, todate
            var data = _repo.SelectWhere(x => x.UserId == userId).OrderBy(x => x.TransferDate).ToList();
            if (data.Count > 0)
            {
                var user = _userRepo.SelectWhere(x => x.Id == userId).FirstOrDefault();

                if (user != null)
                {
                    data[0].FromDate = user.StartWorkDate;
                    if (data.Count > 1)
                    {
                        data[0].ToDate = data[1].TransferDate;
                    }

                    this._repo.UpdateByProperties(data[0],
                         new List<Expression<Func<EmpTransfer, object>>> { x => x.FromDate, x => x.ToDate });
                    _unitOfWork.SaveChanges();

                }

                for (int i = 1; i < data.Count - 1; i++)
                {
                    data[i].FromDate = data[i - 1].TransferDate;
                    data[i].ToDate = data[i + 1].TransferDate;
                    this._repo.UpdateByProperties(data[i],
                         new List<Expression<Func<EmpTransfer, object>>> { x => x.FromDate, x => x.ToDate });
                    _unitOfWork.SaveChanges();
                }

                if (data.Count > 1)
                {
                    data[data.Count - 1].FromDate = data[data.Count - 2].TransferDate;
                    this._repo.UpdateByProperties(data[data.Count - 1],
                         new List<Expression<Func<EmpTransfer, object>>> { x => x.FromDate });
                    _unitOfWork.SaveChanges();
                }
            }
            #endregion
        }

        public IEnumerable<Org_Organization> GetAllOrgs()
        {
            try
            {
                var query = this._orgRepo.SelectAll();
                WriteLogFile("EmpTransferRepository->GetAllOrgs() : Lay du lieu thanh cong");
                return query.ToList();
            }
            catch (Exception ex)
            {
                WriteLogFile(ex.ToString());
                return null;
            }

        }

        private void WriteLogFile(string message)
        {
            var logFileName = DateTime.Now.ToString("dd-MM-yyyy") + "-log.txt";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", logFileName);
            System.IO.File.AppendAllText(path, Environment.NewLine + Environment.NewLine + DateTime.Now.ToString() + "*****>>"
                + message + Environment.NewLine + Environment.NewLine);
        }
    }
}
