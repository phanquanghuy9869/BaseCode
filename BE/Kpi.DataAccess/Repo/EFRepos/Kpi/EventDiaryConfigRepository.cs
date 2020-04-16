using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.Core.Extensions;
using Kpi.Core.Helper;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.DataAccess.Repo.EFRepos.Kpi
{
    public interface IEvenDiaryConfigRepository : IGridRepository<EventDiaryConfigDTO, ViewEventDiaryConfigFilterModelDTO>
    {
        IEnumerable<EventDiaryConfigDTO> GetConfigOrgByLevel1Manager(string username);
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        IEnumerable<UserInfoModel> GetUsers();
        IEnumerable<EventDiaryConfigDTO> GetConfigOrgByLevel2Manager(string username);
        IEnumerable<EventDiaryConfigDTO> GetConfigOrgByKpiManager();
        IEnumerable<EventDiaryConfigDTO> GetConfigOrgByDivManager(string userName);
        IEnumerable<UserDTO> GetConfigLevel1Manager();
    }

    public class EventDiaryConfigRepository : BaseGridRepository<Sys_EventDiaryConfig, EventDiaryConfigDTO, ViewEventDiaryConfigFilterModelDTO>, IEvenDiaryConfigRepository
    {
        private readonly IGenericRepository<View_EventDiaryConfig> _viewEventCfgRepo;
        private readonly IGenericRepository<Sys_EventDiaryConfig> _eventCfgRepo;
        private readonly IGenericRepository<Org_Organization> _orgRepo;
        private readonly IGenericRepository<Org_UserOrg> _orgUserRepo;
        private readonly IGenericRepository<Sys_KpiPeriodConfig> _kpiPeriodGRepo;
        private readonly IKpiEvaluationRepository _kpiRepo;
        private readonly IGenericRepository<Kpi_CriterionType> _criterionTypeGRepo;
        private readonly IGenericRepository<DivMngPer> _divMngRepo;

        public EventDiaryConfigRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _kpiRepo = new KpiEvaluationRepository(unitOfWork);
            _viewEventCfgRepo = _unitOfWork.GetDataRepository<View_EventDiaryConfig>();
            _orgRepo = _unitOfWork.GetDataRepository<Org_Organization>();
            _orgUserRepo = _unitOfWork.GetDataRepository<Org_UserOrg>();
            _eventCfgRepo = _unitOfWork.GetDataRepository<Sys_EventDiaryConfig>();
            _kpiPeriodGRepo = _unitOfWork.GetDataRepository<Sys_KpiPeriodConfig>();
            _criterionTypeGRepo = _unitOfWork.GetDataRepository<Kpi_CriterionType>();
            _divMngRepo = _unitOfWork.GetDataRepository<DivMngPer>();
        }

        public IEnumerable<EventDiaryConfigDTO> GetConfigOrgByLevel1Manager(string username)
        {
            var query = this._repo.SelectWhere(x => x.Level1ManagerUserName == username);
            var qr1 = (from evOrg in query
                       join org in _orgRepo.SelectAll() on evOrg.OrgId equals org.Id into gj
                       from torg in gj.DefaultIfEmpty()
                       select new EventDiaryConfigDTO { OrgId = torg.Id, OrgName = torg.Name, OrgNameEn = torg.NameEn }).ToList();
            return qr1;
        }

        public void Dispose()
        {
            if (this._unitOfWork != null)
            {
                this._unitOfWork.Dispose();
            }
        }

        public EventDiaryConfigDTO Get(int id)
        {
            Sys_EventDiaryConfig eventCfg = _eventCfgRepo.Get(x => x.Id == id);
            if (eventCfg != null)
            {
                var docEntity = AutoMapperHelper.Map<Sys_EventDiaryConfig, EventDiaryConfigDTO>(eventCfg);
                if (docEntity != null)
                {
                    docEntity.UserList = this._orgUserRepo.SelectWhere(x => x.EventDiaryConfigId == id).ProjectTo<UserOrgDTO>().ToList();
                }
                return docEntity;
            }
            return null;
        }

        public IEnumerable<EventDiaryConfigDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventDiaryConfigDTO> GetPaging(ViewEventDiaryConfigFilterModelDTO filter)
        {
            //var query = _eventCfgRepo.SelectWhere(x => ((x.Code.Contains(filter.Code) && filter.Code != null) || filter.Code != null || filter.Code.Trim() == "") &&
            //((x.Name.Contains(filter.Name) && filter.Name != null) || filter.Name != null || filter.Name.Trim() == ""));
            var lstChildrenOrgId = new List<int?>();
            if (filter.OrgId != null)
            {
                var org = _orgRepo.SelectWhere(x => x.Id == filter.OrgId).FirstOrDefault();
                if (org != null)
                {
                    var orgTmps = _orgRepo.SelectWhere(x => x.DirectoryPath != null && x.DirectoryPath.StartsWith(org.DirectoryPath)).ToList();
                    if (orgTmps != null)
                    {
                        foreach (var item in orgTmps)
                        {
                            lstChildrenOrgId.Add(item.Id);
                        }
                    }
                }
            }


            var query = _eventCfgRepo.SelectWhere(x => ((x.OrgId == filter.OrgId && filter.OrgId != null) || filter.OrgId == null || filter.OrgId == -1 || filter.OrgId == 0 || lstChildrenOrgId.Contains(x.OrgId))
            && ((x.Level1ManagerUserId == filter.Level1ManagerId && filter.Level1ManagerId != null) || filter.Level1ManagerId == null || filter.Level1ManagerId == 0 || filter.Level1ManagerId == -1)
            && ((x.Level2ManagerUserId == filter.Level2ManagerId && filter.Level2ManagerId != null) || filter.Level2ManagerId == null || filter.Level2ManagerId == 0 || filter.Level2ManagerId == -1));

            // huypq modified 24-03-2020, yêu cầu của HR tìm kiếm theo tên nhân viên
            if (!string.IsNullOrEmpty(filter.Username))
            {
                var userOrgTbl = this._orgUserRepo.SelectAll();
                query = query.Where(x => userOrgTbl.Any(uo => uo.EventDiaryConfigId == x.Id && uo.UserName == filter.Username));
            }

            return query.OrderBy(x => x.Id).Skip(filter.Start - 1).Take(filter.Length)
             .Select(x => new EventDiaryConfigDTO
             {
                 Id = x.Id,
                 Code = x.Code,
                 OrgId = x.OrgId,
                 OrgName = x.OrgName,
                 Level1ManagerFullName = x.Level1ManagerFullName,
                 Level2ManagerFullName = x.Level2ManagerFullName,
                 IsActive = x.IsActive
             }).ToList();
        }

        public EventDiaryConfigDTO Add(EventDiaryConfigDTO eventCfgDTo)
        {
            var edcEntity = AutoMapperHelper.Map<EventDiaryConfigDTO, Sys_EventDiaryConfig>(eventCfgDTo);

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                // kiem tra du lieu
                if (eventCfgDTo.OrgId == null || string.IsNullOrWhiteSpace(eventCfgDTo.Code) ||
                    _repo.SelectAll().FirstOrDefault(c => c.OrgId == eventCfgDTo.OrgId && c.Level1ManagerUserId == eventCfgDTo.Level1ManagerUserId) != null
                    || _repo.SelectAll().FirstOrDefault(c => c.Code.ToLower() == eventCfgDTo.Code.ToLower()) != null)
                {
                    throw new Exception("[**ERROR**]Chưa nhập đầy đủ thông tin hoặc vi phạm ràng buộc quản lý trực tiếp trong cấu hình");
                }

                // huypq modified 28/2/2020,  fix bug #262, khi add config chưa có chức vụ của level1manager, level2manager
                var level1Usr = _orgUserRepo.SelectWhere(u => u.Id == eventCfgDTo.Level1ManagerUserId).FirstOrDefault();
                if (level1Usr != null)
                {
                    edcEntity.Level1ManagerUserId = eventCfgDTo.Level1ManagerUserId;
                    edcEntity.Level1ManagerFullName = level1Usr.UserFullName;
                    edcEntity.Level1ManagerJobTitle = level1Usr.JobTitle;
                    edcEntity.Level1ManagerOrgName = level1Usr.OrgName;
                    edcEntity.Level1ManagerUserName = level1Usr.UserName;
                }
               
                var level2Usr = _orgUserRepo.SelectWhere(u => u.Id == eventCfgDTo.Level2ManagerUserId).FirstOrDefault();
                if (level2Usr != null)
                {
                    edcEntity.Level2ManagerUserId = eventCfgDTo.Level2ManagerUserId;
                    edcEntity.Level2ManagerFullName = level2Usr.UserFullName;
                    edcEntity.Level2ManagerJobTitle = level2Usr.JobTitle;
                    edcEntity.Level2ManagerOrgName = level2Usr.OrgName;
                    edcEntity.Level2ManagerUserName = level2Usr.UserName;
                }            

                // add ec
                edcEntity = _repo.Add(edcEntity);
                _unitOfWork.SaveChanges();

                List<UserOrgDTO> userOrgs = eventCfgDTo.UserList;

                // add task list
                if (userOrgs != null && userOrgs.Any())
                {
                    foreach (var item in userOrgs)
                    {
                        item.EventDiaryConfigId = edcEntity.Id;
                        var usrOrg = _orgUserRepo.SelectWhere(u => u.Id == item.Id).FirstOrDefault();
                        if (usrOrg != null)
                        {
                            usrOrg.EventDiaryConfigId = edcEntity.Id;
                            usrOrg.EDCUpdateDate = DateTime.Now;
                            usrOrg.Level1ManagerId = eventCfgDTo.Level1ManagerUserId;
                            usrOrg.Level2ManagerId = eventCfgDTo.Level2ManagerUserId;
                        }
                    }
                    _unitOfWork.SaveChanges();
                }

                // huypq modified 17-12-2019
                //this.CreateDiaryAndKpi(eventCfgDTo);
                transaction.Commit();
            }

            return AutoMapperHelper.Map<Sys_EventDiaryConfig, EventDiaryConfigDTO>(edcEntity);
        }

        // huypq 17/12/2019 issues 153
        private void CreateDiaryAndKpi(EventDiaryConfigDTO eventCfgDTo)
        {
            if (eventCfgDTo.ApplyDate == 0)
            {
                return;
            }
            var startedYM = eventCfgDTo.ApplyDate;
            // Lấy những kpi period config đã tồn tại, create ec + kpi dựa trên cái này
            var kpiPeriodConfigs = this._kpiPeriodGRepo.SelectWhere(x => x.YearMonth > startedYM).ProjectTo<KpiPeriodConfigDTO>().ToList();

            // tạo lần lượt cho từng kpiConfig để có thể sử dụng lại code
            foreach (var kpiConfig in kpiPeriodConfigs)
            {
                var eventDiaries = this._kpiRepo.CreateEventDiaryByConfig(new List<EventDiaryConfigDTO>() { eventCfgDTo }, kpiConfig);

                var criterionTypes = this._criterionTypeGRepo.SelectAll();

                foreach (var criterionType in criterionTypes)
                {
                    var users = this._kpiRepo.GetUsersInfoByUsername(eventCfgDTo.UserList.Select(x => x.UserName).ToList(), criterionType.Id);
                    foreach (var u in users)
                    {
                        var currentEventDiary = eventDiaries.FirstOrDefault(x => x.EventDiaryConfigId == u.EventDiaryConfigId);

                        // org cua user ko co diary => next
                        if (currentEventDiary == null)
                        {
                            continue;
                        }

                        //tạo kpi cho user này
                        this._kpiRepo.CreateUserKpi(kpiConfig, u, currentEventDiary, criterionType);
                    }
                }
            }
            _unitOfWork.SaveChanges();
        }

        // huypq 17/12/2019 issues 153
        private void CreateDiary()
        {

        }

        // huypq 17/12/2019 issues 153
        private void CreateKpi()
        {

        }

        public IEnumerable<EventDiaryConfigDTO> AddRange(IEnumerable<EventDiaryConfigDTO> entityDTOs)
        {
            throw new NotImplementedException();
        }

        public int Count(ViewEventDiaryConfigFilterModelDTO filter)
        {
            var lstChildrenOrgId = new List<int?>();
            if (filter.OrgId != null)
            {
                var org = _orgRepo.SelectWhere(x => x.Id == filter.OrgId).FirstOrDefault();
                if (org != null)
                {
                    var orgTmps = _orgRepo.SelectWhere(x => x.DirectoryPath != null && x.DirectoryPath.StartsWith(org.DirectoryPath)).ToList();
                    if (orgTmps != null)
                    {
                        foreach (var item in orgTmps)
                        {
                            lstChildrenOrgId.Add(item.Id);
                        }
                    }
                }
            }

            var ret = _eventCfgRepo.SelectWhere(x => ((x.OrgId == filter.OrgId && filter.OrgId != null) || filter.OrgId == null || filter.OrgId == 0 || lstChildrenOrgId.Contains(x.OrgId))
            && ((x.Level1ManagerUserId == filter.Level1ManagerId && filter.Level1ManagerId != null) || filter.Level1ManagerId == null || filter.Level1ManagerId == 0)
            && ((x.Level2ManagerUserId == filter.Level2ManagerId && filter.Level2ManagerId != null) || filter.Level2ManagerId == null || filter.Level2ManagerId == 0)).Count();
            return ret;
        }

        public void Update(EventDiaryConfigDTO eventCfgDTo)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var oldDto = _repo.SelectWhere(x => x.Id == eventCfgDTo.Id).FirstOrDefault();

                if (oldDto != null)
                {
                    oldDto.Id = eventCfgDTo.Id;
                    oldDto.Code = eventCfgDTo.Code;
                    oldDto.CreatedByUser = eventCfgDTo.CreatedByUser;
                    oldDto.CreatedDate = eventCfgDTo.CreatedDate;
                    oldDto.Description = eventCfgDTo.Description;

                    var level1Usr = _orgUserRepo.SelectWhere(u => u.Id == eventCfgDTo.Level1ManagerUserId).FirstOrDefault();
                    if (level1Usr != null)
                    {
                        oldDto.Level1ManagerUserId = eventCfgDTo.Level1ManagerUserId;
                        oldDto.Level1ManagerFullName = level1Usr.UserFullName;
                        oldDto.Level1ManagerJobTitle = level1Usr.JobTitle;
                        oldDto.Level1ManagerOrgName = level1Usr.OrgName;
                        oldDto.Level1ManagerUserName = level1Usr.UserName;
                    }
                    else
                    {
                        oldDto.Level1ManagerUserId = -1;
                        oldDto.Level1ManagerFullName = null;
                        oldDto.Level1ManagerJobTitle = null;
                        oldDto.Level1ManagerOrgName = null;
                        oldDto.Level1ManagerUserName = null;
                    }

                    var level2Usr = _orgUserRepo.SelectWhere(u => u.Id == eventCfgDTo.Level2ManagerUserId).FirstOrDefault();
                    if (level2Usr != null)
                    {
                        oldDto.Level2ManagerUserId = eventCfgDTo.Level2ManagerUserId;
                        oldDto.Level2ManagerFullName = level2Usr.UserFullName;
                        oldDto.Level2ManagerJobTitle = level2Usr.JobTitle;
                        oldDto.Level2ManagerOrgName = level2Usr.OrgName;
                        oldDto.Level2ManagerUserName = level2Usr.UserName;
                    }
                    else
                    {
                        oldDto.Level2ManagerUserId = -1;
                        oldDto.Level2ManagerFullName = null;
                        oldDto.Level2ManagerJobTitle = null;
                        oldDto.Level2ManagerOrgName = null;
                        oldDto.Level2ManagerUserName = null;
                    }

                    oldDto.Name = eventCfgDTo.Name;
                    oldDto.OrgId = eventCfgDTo.OrgId;
                    oldDto.OrgName = eventCfgDTo.OrgName;
                    oldDto.UpdatedByUser = eventCfgDTo.UpdatedByUser;
                    oldDto.UpdatedDate = eventCfgDTo.UpdatedDate;
                    oldDto.IsActive = eventCfgDTo.IsActive;

                    this._repo.UpdateByProperties(oldDto,
                        new List<Expression<Func<Sys_EventDiaryConfig, object>>> { x => x.Id, x => x.Code, x => x.CreatedByUser, x => x.CreatedDate, x => x.Description, x => x.Level1ManagerUserId
                , x => x.Level1ManagerFullName, x => x.Level1ManagerJobTitle, x => x.Level1ManagerOrgName, x => x.Level1ManagerUserName, x => x.Level2ManagerUserId, x => x.Level2ManagerFullName
                , x => x.Level2ManagerFullName, x => x.Level2ManagerJobTitle, x => x.Level2ManagerOrgName, x => x.Level2ManagerUserName, x => x.Name, x => x.OrgId
                 , x => x.OrgName, x => x.UpdatedByUser, x => x.UpdatedDate, x => x.IsActive});

                    this._unitOfWork.SaveChanges();



                    // xoa id user cu
                    List<UserOrgDTO> userOrgs = eventCfgDTo.UserList;
                    var oldUserList = _orgUserRepo.SelectWhere(u => u.EventDiaryConfigId == eventCfgDTo.Id).ToList();
                    var lstAddUser = new List<UserOrgDTO>();
                    if (userOrgs != null && userOrgs.Any())
                    {
                        foreach (var item in userOrgs)
                        {
                            if ((oldUserList.Count > 0 && !oldUserList.Exists(u => u.Id == item.Id)) || oldUserList.Count == 0)
                            {
                                lstAddUser.Add(item);
                            }
                        }
                    }

                    var lstDeleteUser = new List<Org_UserOrg>();
                    if (oldUserList.Count > 0)
                    {
                        foreach (var item in oldUserList)
                        {
                            if ((userOrgs != null && userOrgs.Any() && !userOrgs.Exists(u => u.Id == item.Id)) || userOrgs == null || !userOrgs.Any())
                            {
                                lstDeleteUser.Add(item);
                            }
                        }
                    }

                    if (lstDeleteUser.Count > 0)
                    {
                        foreach (var item in lstDeleteUser)
                        {
                            item.EventDiaryConfigId = null;
                            item.EDCUpdateDate = DateTime.Now;
                            item.Level1ManagerId = -1;
                            item.Level2ManagerId = -1;

                            this._orgUserRepo.UpdateByProperties(item,
                                new List<Expression<Func<Org_UserOrg, object>>> { x => x.EventDiaryConfigId, x => x.Level1ManagerId, x => x.Level2ManagerId, x => x.EDCUpdateDate });
                            _unitOfWork.SaveChanges();
                        }
                    }

                    if (lstAddUser.Count > 0)
                    {
                        foreach (var item in lstAddUser)
                        {
                            item.EventDiaryConfigId = eventCfgDTo.Id;
                            var usrOrg = _orgUserRepo.SelectWhere(u => u.Id == item.Id).FirstOrDefault();
                            if (usrOrg != null)
                            {
                                usrOrg.EventDiaryConfigId = eventCfgDTo.Id;
                                usrOrg.EDCUpdateDate = DateTime.Now;
                                usrOrg.Level1ManagerId = oldDto.Level1ManagerUserId;
                                usrOrg.Level2ManagerId = oldDto.Level2ManagerUserId;

                                this._orgUserRepo.UpdateByProperties(usrOrg,
                                    new List<Expression<Func<Org_UserOrg, object>>> { x => x.EventDiaryConfigId, x => x.Level1ManagerId, x => x.OrgName
                                    ,x => x.Level2ManagerId, x => x.EDCUpdateDate});
                                _unitOfWork.SaveChanges();
                            }
                        }
                    }

                    // huypq modified 09-01-2020
                    var updatedUser = oldUserList.Where(x => !lstDeleteUser.Contains(x));
                    foreach (var u in updatedUser)
                    {
                        u.EDCUpdateDate = DateTime.Now;
                        u.Level1ManagerId = oldDto.Level1ManagerUserId;
                        u.Level2ManagerId = oldDto.Level2ManagerUserId;
                        this._orgUserRepo.UpdateByProperties(u,
                            new List<Expression<Func<Org_UserOrg, object>>> { x => x.EventDiaryConfigId, x => x.Level1ManagerId, x => x.OrgName
                                    ,x => x.Level2ManagerId, x => x.EDCUpdateDate});
                        _unitOfWork.SaveChanges();
                    }
                }

                transaction.Commit();
            }
        }

        public void Delete(EventDiaryConfigDTO entityDto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgs()
        {
            // lay==
            var query = this._orgRepo.SelectAll();
            var test = query.ToList();
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

        public IEnumerable<UserInfoModel> GetUsers()
        {
            // lay danh sach user chua nghi viec
            var query = this._orgUserRepo.SelectWhere(u => u.Status != "NV");
            var test = query.ToList();
            return query.OrderBy(x => x.Id).Select(x => new UserInfoModel
            {
                Id = x.Id,
                JobTitle = x.JobTitle,
                UserFullName = x.UserFullName,
                OrgId = x.OrgId,
                OrgName = x.OrgName,
                UserName = x.UserName
            }).ToList();
        }

        public IEnumerable<EventDiaryConfigDTO> GetConfigOrgByLevel2Manager(string username)
        {
            //var ret = this._repo.SelectWhere(x => x.Level2ManagerUserName == username)
            //    .Select(x => new EventDiaryConfigDTO
            //    {
            //        OrgId = x.OrgId,
            //        OrgName = x.OrgName,
            //        Level1ManagerFullName = x.Level1ManagerFullName,
            //        Level1ManagerUserId = x.Level1ManagerUserId
            //    }).ToList();

            //if (ret.Count > 0)
            //{
            //    // danh sach phong ban theo ket qua tren
            //    var orgs = this._orgRepo.SelectAll().ToList().Where(o => ret.Exists(r => r.OrgId == o.Id)).ToList();

            //    foreach (var item in ret)
            //    {
            //        var org = orgs.FirstOrDefault(r => r.Id == item.OrgId);
            //        if (org != null)
            //        {
            //            item.OrgDirPath = org.DirectoryPath;
            //        }
            //    }
            //}

            var rs = (from ec in this._repo.SelectWhere(x => x.Level2ManagerUserName == username)
                      join o in this._orgRepo.SelectAll() on ec.OrgId equals o.Id
                      select new EventDiaryConfigDTO
                      {
                          OrgId = ec.OrgId,
                          OrgName = ec.OrgName,
                          Level1ManagerFullName = ec.Level1ManagerFullName,
                          Level1ManagerUserName = ec.Level1ManagerUserName,
                          OrgDirPath = o.DirectoryPath,
                          OrgNameEn = o.NameEn,
                      }).ToList();
            return rs;
        }

        public IEnumerable<EventDiaryConfigDTO> GetConfigOrgByKpiManager()
        {
            //var ret = this._repo.SelectAll()
            //       .Select(x => new EventDiaryConfigDTO
            //       {
            //           OrgId = x.OrgId,
            //           OrgName = x.OrgName,
            //           Level1ManagerFullName = x.Level1ManagerFullName,
            //           Level1ManagerUserId = x.Level1ManagerUserId
            //       }).ToList();

            //if (ret.Count > 0)
            //{
            //    // danh sach phong ban theo ket qua tren
            //    var orgs = this._orgRepo.SelectAll().ToList().Where(o => ret.Exists(r => r.OrgId == o.Id)).ToList();

            //    foreach (var item in ret)
            //    {
            //        var org = orgs.FirstOrDefault(r => r.Id == item.OrgId);
            //        if (org != null)
            //        {
            //            item.OrgDirPath = org.DirectoryPath;
            //        }
            //    }
            //}

            var rs = (from ec in this._repo.SelectAll()
                      join o in this._orgRepo.SelectAll() on ec.OrgId equals o.Id
                      select new EventDiaryConfigDTO
                      {
                          OrgId = ec.OrgId,
                          OrgName = ec.OrgName,
                          Level1ManagerFullName = ec.Level1ManagerFullName,
                          Level1ManagerUserName = ec.Level1ManagerUserName,
                          OrgDirPath = o.DirectoryPath
                      }).ToList();
            return rs;
        }

        public IEnumerable<EventDiaryConfigDTO> GetConfigOrgByDivManager(string userName)
        {
            var usr = this._orgUserRepo.SelectWhere(x => x.UserName == userName).FirstOrDefault();
            if (usr != null)
            {
                // danh sach phan quyen ql khoi
                var divMngPers = this._divMngRepo.SelectWhere(x => x.UserId == usr.Id);

                var rs = (from ec in this._repo.SelectAll()
                          join o in this._orgRepo.SelectAll() on ec.OrgId equals o.Id
                          join p in divMngPers on ec.OrgId equals p.OrgId
                          select new EventDiaryConfigDTO
                          {
                              OrgId = ec.OrgId,
                              OrgName = ec.OrgName,
                              Level1ManagerFullName = ec.Level1ManagerFullName,
                              Level1ManagerUserName = ec.Level1ManagerUserName,
                              OrgDirPath = o.DirectoryPath,
                              OrgNameEn = o.NameEn,
                          }).ToList();
                return rs;
            }
            return new List<EventDiaryConfigDTO>();
        }

        public IEnumerable<UserDTO> GetConfigLevel1Manager()
        {
            return this._repo.SelectWhere(c => c.IsActive == true).Select(x => new UserDTO { UserName = x.Level1ManagerUserName, Name = x.Level1ManagerFullName }).Distinct().ToList();
        }
    }
}
