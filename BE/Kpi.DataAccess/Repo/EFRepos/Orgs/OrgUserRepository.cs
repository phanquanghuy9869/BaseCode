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
using AutoMapper;

namespace Kpi.DataAccess.Repo.EFRepos.Orgs
{
    public interface IOrgUserRepository : IGridRepository<UserInfoModel, OrgInfoModelFilterModelDTO>
    {
        UserInfoModel Get(string username);
        IEnumerable<UserOrgDTO> GetEDUsersByOrg(int orgId);
        IEnumerable<UserOrgDTO> GetEDUsersByEvent(int eventDiaryId);
        IEnumerable<UserOrgDTO> GetEDUsersByEvents(List<int> eventDiaryIds);
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        IEnumerable<UserInfoModel> GetUsers();
        IEnumerable<Org_JobTitleDTO> GetJobTitles();
        bool AddUser(UserOrgDTO model);
        bool UpdateUser(UserOrgDTO model);
        IEnumerable<AspNetRole> GetRoles();
        OrganizationDTO GetOrgByUser(string username);
        bool AddUserNoTrans(UserOrgDTO model);
        bool UpdateUserNoTrans(UserOrgDTO entity);
        IEnumerable<OrgInfoModelDTO> GetOrgsByLevel2Manager(string username);
        IEnumerable<OrgInfoModelDTO> GetOrgsByLevel1Manager(string username);
        IEnumerable<UserOrgDTO> GetLevel1ManagerUserOrg();
        IEnumerable<UserOrgDTO> GetLevel2ManagerUserOrg();
        IEnumerable<OrgInfoModelDTO> GetAllOrgs();
        IEnumerable<UserInfoModel> GetAllUsers();
        IEnumerable<Org_JobTitleDTO> GetAllJobTitles();
        bool DeleteUser(UserOrgDTO entity);
        IEnumerable<UserInfoModel> SearchPagingDivisionManager(UserOrgFilterModelDTO filter, List<string> users);
        void saveDivisionManagerPermission(DivMngPerUser model);
        DivMngPerUser GetDivisionManagerPermission(int userId);
        bool IsVIP(string username);
        UserOrgDTO GetLevel1ManagerByUserName(string username, int yearMonth);
    }

    public class OrgUserRepository : BaseGridRepository<Org_UserOrg, UserInfoModel, OrgInfoModelFilterModelDTO>, IOrgUserRepository
    {
        private readonly IGenericRepository<Org_Organization> _orgRepo;
        private readonly IGenericRepository<View_UserOrg_GetPaging> _viewUsrRepo;
        private readonly IGenericRepository<Org_JobTitle> _jobTitleRepo;
        private readonly IGenericRepository<AspNetRole> _aspNetRoleRepo;
        private readonly IGenericRepository<AspNetUser> _aspNetUserRepo;
        private readonly IGenericRepository<Kpi_KpiEvaluation> _kpiEvaluationGRepo;
        private readonly IGenericRepository<Sys_EventDiaryConfig> _eventDiaryConfigGRepo;
        private readonly IGenericRepository<DivMngPer> _divMngRepo;
        private readonly IGenericRepository<Org_UserOrg> _Org_UserOrgRepo;

        public OrgUserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _orgRepo = _unitOfWork.GetDataRepository<Org_Organization>();
            _viewUsrRepo = _unitOfWork.GetDataRepository<View_UserOrg_GetPaging>();
            _jobTitleRepo = _unitOfWork.GetDataRepository<Org_JobTitle>();
            _aspNetRoleRepo = _unitOfWork.GetDataRepository<AspNetRole>();
            _kpiEvaluationGRepo = _unitOfWork.GetDataRepository<Kpi_KpiEvaluation>();
            _eventDiaryConfigGRepo = _unitOfWork.GetDataRepository<Sys_EventDiaryConfig>();
            _divMngRepo = _unitOfWork.GetDataRepository<DivMngPer>();
            _Org_UserOrgRepo = _unitOfWork.GetDataRepository<Org_UserOrg>();
            _aspNetUserRepo = _unitOfWork.GetDataRepository<AspNetUser>();
        }

        public bool AddUser(UserOrgDTO entityDTo)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = AutoMapperHelper.Map<UserOrgDTO, Org_UserOrg>(entityDTo);
                //var max = _repo.SelectAll().Max(x=>x.Id);
                //entity.Id = max + 1;
                var result = _repo.Add(entity);
                _unitOfWork.SaveChanges();
                transaction.Commit();
            }
            return true;
        }

        public bool UpdateUser(UserOrgDTO entityDTo)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = AutoMapperHelper.Map<UserOrgDTO, Org_UserOrg>(entityDTo);
                var user = _repo.SelectWhere(x => x.Id == entity.Id).FirstOrDefault();

                AutoMapperHelper.MapSameType<Org_UserOrg>(entity, user, typeof(Org_UserOrg));
                _repo.Update(user);
                _unitOfWork.SaveChanges();
                transaction.Commit();
            }
            return true;
        }

        public UserInfoModel Get(string username)
        {
            var u = _repo.SelectWhere(x => x.UserName == username).ProjectTo<UserOrgDTO>().FirstOrDefault();
            var ret = AutoMapperHelper.Map<UserOrgDTO, UserInfoModel>(u);
            ret.IsActived = this._aspNetUserRepo.SelectWhere(x => x.UserName == ret.UserName).Select(x => x.IsActived).FirstOrDefault();
            //if (ret != null)
            //{
            //    if (ret.Level1ManagerId != null)
            //    {
            //        var mng = Get(ret.Level1ManagerId.Value);
            //        if (mng != null)
            //        {
            //            ret.Level1ManagerFullName = mng.UserFullName;
            //            ret.Level1ManagerJobTileId = mng.Level1ManagerJobTileId;
            //            ret.Level1ManagerJobTitle = mng.Level1ManagerJobTitle;
            //            ret.Level1ManagerOrgId = mng.Level1ManagerOrgId;
            //            ret.Level1ManagerOrgName = mng.Level1ManagerOrgName;
            //            ret.Level1ManagerUserName = mng.Level1ManagerUserName;
            //        }
            //    }
            //    if (ret.Level2ManagerId != null)
            //    {
            //        var mng = Get(ret.Level2ManagerId.Value);
            //        if (mng != null)
            //        {
            //            ret.Level2ManagerFullName = mng.UserFullName;
            //            ret.Level2ManagerJobTitle = mng.Level1ManagerJobTitle;
            //            ret.Level2ManagerOrgName = mng.Level1ManagerOrgName;
            //            ret.Level2ManagerUserName = mng.Level1ManagerUserName;
            //        }
            //    }
            //}
            return ret;
        }

        public override void Update(UserInfoModel entityDto)
        {
            base.Update(entityDto);
        }

        public override IEnumerable<UserInfoModel> GetPaging(OrgInfoModelFilterModelDTO filter)
        {
            var userOrg = _viewUsrRepo.SelectWhere(x => ((x.UserFullName.Contains(filter.Name) && filter.Name != null) || filter.Name == null || filter.Name.Trim() == "") &&
                (x.OrgId == filter.orgId || filter.orgId == null || filter.orgId < 0));

            //// huypq modified 24/3/2020 thêm tìm kiếm theo job status
            //if (!string.IsNullOrEmpty(filter.JobStatus))
            //{
            //    query = query.Where(x => x.Status == filter.JobStatus);
            //}

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                userOrg = userOrg.Where(x => x.UserName == filter.PhoneNumber);
            }

            var query = (from u in this._aspNetUserRepo.SelectAll()
                         join uo in userOrg on u.UserName equals uo.UserName
                         select new UserInfoModel
                         {
                             Id = uo.Id,
                             JobTitle = uo.JobTitle,
                             UserFullName = uo.UserFullName,
                             UserEmail = uo.UserEmail,
                             OrgId = uo.OrgId,
                             OrgName = uo.OrgName,
                             Status = uo.Status,
                             Level1ManagerFullName = uo.Level1ManagerName,
                             Level2ManagerFullName = uo.Level2ManagerName,
                             FirstKpiDate = uo.FirstKpiDate,
                             DOB = uo.DOB,
                             StartWorkDate = uo.StartWorkDate,
                             Code = uo.Code,
                             PhoneNumber = u.PhoneNumber,
                             IdCardNumber = uo.IdCardNumber,
                             IsActived = u.IsActived,
                             EDCUpdateDate = uo.EDCUpdateDate
                         });

            var rs = query.OrderBy(x => x.Id).Skip(filter.Start - 1).Take(filter.Length).ToList();
            return rs;
        }

        public override int Count(OrgInfoModelFilterModelDTO filter)
        {
            // huypq modified 24/3/2020 thêm tìm kiếm theo job status
            var query = _viewUsrRepo.SelectWhere(x => ((x.UserFullName.Contains(filter.Name) && filter.Name != null) || filter.Name == null || filter.Name.Trim() == "") &&
               (x.OrgId == filter.orgId || filter.orgId == null || filter.orgId < 0));

            if (!string.IsNullOrEmpty(filter.JobStatus))
            {
                query = query.Where(x => x.Status == filter.JobStatus);
            }

            return query.Count();
        }

        public IEnumerable<UserOrgDTO> GetEDUsersByOrg(int orgId)
        {
            return this._repo.SelectWhere(x => x.OrgId == orgId).Where(x => x.IsOrgManager == null || x.IsOrgManager == false)
                .Select(x => new UserOrgDTO { UserName = x.UserName, UserFullName = x.UserFullName, OrgName = x.OrgName }).ToList();
        }

        public IEnumerable<UserOrgDTO> GetEDUsersByEvent(int eventDiaryId)
        {
            var query = (from uo in this._repo.SelectAll()
                         join k in this._kpiEvaluationGRepo.SelectWhere(x => x.EventDiaryId == eventDiaryId) on uo.UserName equals k.UserName
                         select new UserOrgDTO
                         {
                             UserName = uo.UserName,
                             UserFullName = uo.UserFullName,
                             OrgName = uo.OrgName,
                             CurrentKpi = k.Id,
                             CurrentKpiStatusId = k.StatusId,
                             CurrentEventDiaryId = k.EventDiaryId
                         });
            return query.ToList();
        }

        public IEnumerable<UserOrgDTO> GetEDUsersByEvents(List<int> eventDiaryIds)
        {
            var query = (from uo in this._repo.SelectAll()
                         join k in this._kpiEvaluationGRepo.SelectWhere(x => eventDiaryIds.Contains(x.EventDiaryId.Value)) on uo.UserName equals k.UserName
                         select new UserOrgDTO
                         {
                             UserName = uo.UserName,
                             UserFullName = uo.UserFullName,
                             OrgName = uo.OrgName,
                             CurrentKpi = k.Id,
                             CurrentKpiStatusId = k.StatusId,
                             CurrentEventDiaryId = k.EventDiaryId
                         });
            return query.ToList();
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
                DirectoryPath = x.DirectoryPath,
                ParentId = x.ParentId
            }).ToList();
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgsByLevel2Manager(string username)
        {
            var query = this._eventDiaryConfigGRepo.SelectWhere(x => x.Level2ManagerUserName == username && x.OrgName != null).Distinct();
            var qr1 = (from evOrg in query
                       join org in _orgRepo.SelectAll() on evOrg.OrgId equals org.Id into gj
                       from torg in gj.DefaultIfEmpty()
                       select new OrgInfoModelDTO { Id = torg.Id, Name = torg.Name, NameEn = torg.NameEn }).ToList();
            return qr1;
        }

        public OrganizationDTO GetOrgByUser(string username)
        {
            var query = (from uo in this._repo.SelectWhere(x => x.UserName == username).Take(1)
                         join ec in this._eventDiaryConfigGRepo.SelectAll() on uo.EventDiaryConfigId equals ec.Id
                         select new OrganizationDTO { Id = ec.OrgId.Value, Name = ec.OrgName }
                         ).FirstOrDefault();
            if (query != null)
            {
                var org = _orgRepo.SelectWhere(x => x.Id == query.Id).FirstOrDefault();
                query.NameEn = org != null ? org.NameEn : "";
            }
            return query;
            //return this._repo.SelectWhere(x => x.UserName == username).Select(x => new OrganizationDTO { Id = x.OrgId, Name = x.OrgName }).FirstOrDefault();
        }

        public IEnumerable<UserInfoModel> GetUsers()
        {
            // get kpi belong to this config  
            var query = this._repo.SelectAll();
            return query.OrderBy(x => x.Id).Select(x => new UserInfoModel
            {
                Id = x.Id,
                JobTitle = x.JobTitle,
                UserFullName = x.UserFullName,
                OrgId = x.OrgId,
                OrgName = x.OrgName,
                UserName = x.UserName,
                IsOrgManager = x.IsOrgManager,
            }).ToList();
        }

        public IEnumerable<Org_JobTitleDTO> GetJobTitles()
        {
            // get kpi belong to this config  
            var query = this._jobTitleRepo.SelectAll();
            return query.OrderBy(x => x.Id).Select(x => new Org_JobTitleDTO
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
            }).ToList();
        }

        public IEnumerable<Org_Organization> GetAllOrgs()
        {
            // get kpi belong to this config  
            var query = this._orgRepo.SelectAll();
            return query.OrderBy(x => x.Id).ToList();
        }

        public IEnumerable<AspNetRole> GetRoles()
        {
            var query = this._aspNetRoleRepo.SelectAll();
            return query.OrderBy(x => x.Id).ToList();
        }

        public bool AddUserNoTrans(UserOrgDTO model)
        {
            var entity = AutoMapperHelper.Map<UserOrgDTO, Org_UserOrg>(model);

            // chuc danh
            var jobTitle = GetJobTitles().FirstOrDefault(t => t.Id == entity.JobTitleId);
            if (jobTitle != null)
            {
                entity.JobTitle = jobTitle.Title;
            }

            // ten phong ban
            var orgName = GetAllOrgs().FirstOrDefault(t => t.Id == entity.OrgId);
            if (orgName != null)
            {
                entity.OrgName = orgName.Name;
            }

            var result = _repo.Add(entity);
            _unitOfWork.SaveChanges();
            return true;
        }

        public bool UpdateUserNoTrans(UserOrgDTO entityDTo)
        {
            var user = _repo.SelectWhere(x => x.Id == entityDTo.Id).FirstOrDefault();

            user.UserFullName = entityDTo.UserFullName;
            user.Status = entityDTo.Status;
            user.FirstKpiDate = entityDTo.FirstKpiDate;
            user.KpiType = entityDTo.KpiType;
            user.Level1ManagerId = entityDTo.Level1ManagerId;
            user.Level2ManagerId = entityDTo.Level2ManagerId;
            user.UserName = entityDTo.UserName;
            user.DOB = entityDTo.DOB;
            user.StartWorkDate = entityDTo.StartWorkDate;
            user.Code = entityDTo.Code;
            user.IdCardNumber = entityDTo.IdCardNumber;
            user.IdCardDate = entityDTo.IdCardDate;
            user.IdCardLocation = entityDTo.IdCardLocation;
            user.IsOrgManager = entityDTo.IsOrgManager;
            user.UserEmail = entityDTo.UserEmail;
            user.PhoneNumber = entityDTo.PhoneNumber;

            var currentOrg = this._orgRepo.SelectWhere(x => x.Id == entityDTo.OrgId).Select(x => new OrganizationDTO { Id = x.Id, Name = x.Name }).FirstOrDefault();
            user.OrgId = currentOrg.Id;
            user.OrgName = currentOrg.Name;            

            // neu cap nhat tu dieu chuyen thì cho cap nhat chuc danh phong ban
            //if (fromEmpTransfer)
            //{
            //    // chuc danh
            //    user.JobTitleId = entityDTo.JobTitleId;
            //    var jobTitle = GetJobTitles().FirstOrDefault(t => t.Id == user.JobTitleId);
            //    if (jobTitle != null)
            //    {
            //        user.JobTitle = jobTitle.Title;
            //    }

            //    // ten phong ban
            //    user.OrgId = entityDTo.OrgId;
            //    var orgName = GetAllOrgs().FirstOrDefault(t => t.Id == user.OrgId);
            //    if (orgName != null)
            //    {
            //        user.OrgName = orgName.Name;
            //    }
            //    this._repo.UpdateByProperties(user,
            //    new List<Expression<Func<Org_UserOrg, object>>> { x => x.UserFullName, x => x.JobTitleId, x => x.OrgId, x => x.Status, x => x.FirstKpiDate, x => x.KpiType
            //    , x => x.Level1ManagerId, x => x.Level2ManagerId, x => x.UserName, x => x.DOB, x => x.StartWorkDate, x => x.Code, x=> x.IsOrgManager
            //    , x => x.IdCardNumber, x => x.IdCardDate, x => x.IdCardLocation, x => x.OrgName, x => x.JobTitle});
            //}
            //else
            //{
            //user.KpiType = "QL";

            // cap nhat tinh trang nghi viec
            UpdateUserStatus(user);

            this._repo.UpdateByProperties(user,
                new List<Expression<Func<Org_UserOrg, object>>> { x => x.UserFullName, x => x.Status, x => x.FirstKpiDate, x => x.KpiType, x => x.UserEmail, x => x.PhoneNumber
                , x => x.Level1ManagerId, x => x.Level2ManagerId, x => x.UserName, x => x.DOB, x => x.StartWorkDate, x => x.Code, x=> x.IsOrgManager
                , x => x.IdCardNumber, x => x.IdCardDate, x => x.IdCardLocation, x => x.OrgId, x => x.OrgName});

            //}

            //_repo.Update(user);
            _unitOfWork.SaveChanges();
            return true;
        }

        private void UpdateUserStatus(Org_UserOrg user)
        {
            var isDeleted = user.Status == "NV";

            var kpis = this._kpiEvaluationGRepo.SelectWhere(x => x.UserName == user.UserName).ToList();
            if (kpis.Count > 0)
            {
                foreach (var item in kpis)
                {
                    item.IsDeleted = isDeleted;
                    this._kpiEvaluationGRepo.UpdateByProperties(item,
                        new List<Expression<Func<Kpi_KpiEvaluation, object>>> { x => x.IsDeleted });
                }
            }

            //huypq modifed 05-02-2020, bỏ hàm này do luôn bị timeout (có thể do bảng user bị lock)
            //update user ở controller sử dụng thư viện của owin

            //var aspNetUsrs = this._aspNetUserRepo.SelectWhere(x => x.UserName == user.UserName).ToList();
            //if (aspNetUsrs.Count > 0)
            //{
            //    foreach (var item in aspNetUsrs)
            //    {
            //        item.IsDeleted = isDeleted;
            //        this._aspNetUserRepo.UpdateByProperties(item,
            //            new List<Expression<Func<AspNetUser, object>>> { x => x.IsDeleted });
            //    }
            //}
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgsByLevel1Manager(string username)
        {
            var query = this._eventDiaryConfigGRepo.SelectWhere(x => x.Level1ManagerUserName == username && x.OrgName != null).Distinct();
            var qr1 = (from evOrg in query
                       join org in _orgRepo.SelectAll() on evOrg.OrgId equals org.Id into gj
                       from torg in gj.DefaultIfEmpty()
                       select new OrgInfoModelDTO { Id = torg.Id, Name = torg.Name, NameEn = torg.NameEn }).ToList();
            return qr1;
        }

        public IEnumerable<UserOrgDTO> GetLevel1ManagerUserOrg()
        {
            var query = (from uo in this._repo.SelectAll()
                         join uoL1 in this._repo.SelectAll() on uo.Level1ManagerId equals uoL1.Id
                         select new UserOrgDTO
                         {
                             UserName = uoL1.UserName,
                             UserFullName = uoL1.UserFullName
                         }).Distinct();
            return query.ToList();
        }

        public IEnumerable<UserOrgDTO> GetLevel2ManagerUserOrg()
        {
            var query = (from uo in this._repo.SelectAll()
                         join uoL2 in this._repo.SelectAll() on uo.Level2ManagerId equals uoL2.Id
                         select new UserOrgDTO
                         {
                             UserName = uoL2.UserName,
                             UserFullName = uoL2.UserFullName
                         }).Distinct();
            return query.ToList();
        }

        IEnumerable<OrgInfoModelDTO> IOrgUserRepository.GetAllOrgs()
        {
            var query = this._orgRepo.SelectAll().OrderBy(x => x.Id).ProjectTo<OrgInfoModelDTO>().ToList();
            return query;
        }

        public IEnumerable<UserInfoModel> GetAllUsers()
        {
            var query = this._repo.SelectAll().OrderBy(x => x.Id).Select(x => new UserInfoModel()
            {
                Code = x.Code,
                //CurrentKpi = x.CurrentKpi,
                //CurrentKpiStatusId = x.CurrentKpiStatusId,
                DOB = x.DOB,
                EDCUpdateDate = x.EDCUpdateDate,
                EventDiaryConfigId = x.EventDiaryConfigId,
                FirstKpiDate = x.FirstKpiDate,
                Id = x.Id,
                IdCardDate = x.IdCardDate,
                IdCardLocation = x.IdCardLocation,
                IdCardNumber = x.IdCardNumber,
                IsOrgManager = x.IsOrgManager,
                JobTitle = x.JobTitle,
                JobTitleId = x.JobTitleId,
                KpiType = x.KpiType,
                //Level1ManagerFullName = x.Level1ManagerFullName,
                Level1ManagerId = x.Level1ManagerId,
                ///Level1ManagerUserName = x.Level1ManagerUserName,
                //Level2ManagerFullName = x.Level2ManagerFullName,
                Level2ManagerId = x.Level2ManagerId,
                ///=Level2ManagerUserName = x.Level2ManagerUserName,
                OrgId = x.OrgId,
                OrgName = x.OrgName,
                StartWorkDate = x.StartWorkDate,
                Status = x.Status,
                UserEmail = x.UserEmail,
                UserFullName = x.UserFullName,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber
            }).ToList();

            return query;
        }

        public IEnumerable<Org_JobTitleDTO> GetAllJobTitles()
        {
            var query = this._jobTitleRepo.SelectAll().OrderBy(x => x.Id).ProjectTo<Org_JobTitleDTO>();
            return query;
        }

        public bool DeleteUser(UserOrgDTO entity)
        {
            return false;
        }

        public IEnumerable<UserInfoModel> SearchPagingDivisionManager(UserOrgFilterModelDTO filter, List<string> users)
        {
            var userQ = this._repo.SelectWhere(x => users.Any(t => t == x.UserName));

            var query = _viewUsrRepo.SelectWhere(x => ((x.UserFullName.Contains(filter.Name) && filter.Name != null) || filter.Name == null || filter.Name.Trim() == "") &&
               (x.OrgId == filter.orgId || filter.orgId == null || filter.orgId < 0))
               .Join(userQ, v => v.Id, u => u.Id, (v, u) => v);

            var res = query.OrderBy(x => x.Id)
             .Select(x => new UserInfoModel
             {
                 Id = x.Id,
                 JobTitle = x.JobTitle,
                 UserName = x.UserName,
                 UserFullName = x.UserFullName,
                 UserEmail = x.UserEmail,
                 OrgId = x.OrgId,
                 OrgName = x.OrgName,
                 Status = x.Status,
                 Level1ManagerFullName = x.Level1ManagerName,
                 Level2ManagerFullName = x.Level2ManagerName,
                 FirstKpiDate = x.FirstKpiDate,
                 DOB = x.DOB,
                 StartWorkDate = x.StartWorkDate,
                 Code = x.Code
             }).ToList();
            return res;
        }

        public void saveDivisionManagerPermission(DivMngPerUser model)
        {
            var orgs = this._orgRepo.SelectAll().OrderBy(x => x.Id).Select(x => x.Id).ToList();
            var divPers = this._divMngRepo.SelectWhere(x => x.UserId == model.UserId).ToList();

            if (this._repo.SelectWhere(x => x.Id == model.UserId).FirstOrDefault() == null)
            {
                throw new Exception("Quản lý không tồn tại - id=" + model.UserId);
            }

            using (var trans = _unitOfWork.BeginTransaction())
            {
                foreach (var item in model.Details)
                {
                    if (!orgs.Exists(x => x == item.OrgId))
                    {
                        throw new Exception("Phòng ban/đơn vị không tồn tại - id=" + item.OrgId);
                    }

                    var entity = new DivMngPer() { UserId = model.UserId, OrgId = item.OrgId };
                    if (!divPers.Exists(x => x.OrgId == entity.OrgId))
                    {
                        _divMngRepo.Add(entity);
                    }

                    _unitOfWork.SaveChanges();
                }

                foreach (var item in divPers)
                {
                    if (!model.Details.Exists(x => x.OrgId == item.OrgId))
                    {
                        _divMngRepo.Delete(item);
                    }

                    _unitOfWork.SaveChanges();
                }

                trans.Commit();
            }
        }

        public DivMngPerUser GetDivisionManagerPermission(int userId)
        {
            var ret = new DivMngPerUser();
            var user = this._repo.SelectWhere(x => x.Id == userId).FirstOrDefault();
            if (user != null)
            {
                ret.UserId = userId;
                ret.UserFullName = user.UserFullName;
                ret.UserName = user.UserName;
                var org = this._orgRepo.SelectWhere(x => x.Id == user.OrgId).FirstOrDefault();
                ret.OrgName = org != null ? org.Name : "";
                var job = this._jobTitleRepo.SelectWhere(x => x.Id == user.JobTitleId).FirstOrDefault();
                ret.JobTitle = job != null ? job.Title : "";

                var details = this._divMngRepo.SelectWhere(x => x.UserId == user.Id).ProjectTo<DivMngPerDTO>().ToList();
                ret.Details = new List<DivMngPerDTO>();
                foreach (var item in details)
                {
                    var org1 = this._orgRepo.SelectWhere(x => x.Id == item.OrgId).FirstOrDefault();
                    item.OrgName = org1 != null ? org1.Name : "";
                    ret.Details.Add(item);
                }
            }
            return ret;
        }

        public bool IsVIP(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            var isVip = this._repo.Any(x => x.UserName == username && x.IsOrgManager != null && x.IsOrgManager.Value);
            return isVip;
        }

        public UserOrgDTO GetLevel1ManagerByUserName(string username, int yearMonth)
        {
            var kpi = this._kpiEvaluationGRepo.SelectWhere(x => x.UserName == username && x.YearMonth == yearMonth).FirstOrDefault();
            if (kpi != null)
            {
                var eventCfg = this._eventDiaryConfigGRepo.SelectWhere(x => x.Id == kpi.EventDiaryConfigId).FirstOrDefault();
                if (eventCfg != null)
                {
                    var user = this._Org_UserOrgRepo.SelectWhere(x => x.UserName == eventCfg.Level1ManagerUserName).ProjectTo<UserOrgDTO>().FirstOrDefault();
                    return user;
                }
            }
            return null;
        }
    }
}
