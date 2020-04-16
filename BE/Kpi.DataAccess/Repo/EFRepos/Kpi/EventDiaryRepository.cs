using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.DataAccess.Repo.EFRepos.Kpi
{
    public interface IEventDiaryRepository
    {
        EventDiaryDTO GetMonthlyEventDiaryByManager(int yearMonth, int? orgId, string level1ManagerUserName, string empName);
        EventDiaryDTO GetMonthlyEventDiaryByHRManager(int yearMonth, int? orgId, string level1ManagerUserName, string empUserName);
        EventDiaryDTO GetMonthlyEventDiaryByEmp(int yearMonth, int? orgId, string empUserName);
        EventDiaryDTO GetMonthlyEventDiaryByKpiId(int kpiId);
        EventDiaryDTO GetMonthlyEventDiaryByManagerLv2(int yearMonth, int? orgId, string userName);
        EventDiaryDTO GetEventDiaryByKpiManager(int yearMonth, int? orgId);
        EventDiaryPagingModelDTO GetPaging(EventDiaryFilterModelDTO filter);
        EventDiaryDTO GetEventDiaryByManagerLv2(int yearMonth, int? orgId, string level1ManagerUserName, string level2ManagerUserName, string empName);
        IEnumerable<UserOrgDTO> GetLevel1UserByLevel2Manager(string userName, int? orgId);
        PagingModelDTO GetPagingExistedEventDiaryByYearMonth(EventDiaryFilterModelDTO filter);
        PagingModelDTO GetPagingMissingEventDiaryByYearMonth(EventDiaryFilterModelDTO filter);
        IEnumerable<UserOrgDTO> GetUsers();
        EventDiaryDTO GetEventDiaryByKpiManager(int yearMonth, int? orgId, string level1ManagerUserName, string empName);
        EventDiaryDTO GetEventByDivManager(int yearMonth, int? orgId, string level1ManagerUserName, string empName, string userName);
        void AddRangeDiaryKpi(IEnumerable<EventDiarySyncModelDTO> configs, string currentUser);
        void UpdateRangeDiaryKpiConfigChanged(IEnumerable<EventDiarySyncModelDTO> configs, string currentUser);
        void CompleteNotify(string userName, int evntId);
    }

    public class EventDiaryRepository : IEventDiaryRepository, IDisposable
    {
        private readonly IGenericRepository<Kpi_EventDiary> _eventDiaryGRepo;
        private readonly IGenericRepository<Sys_EventDiaryConfig> _eventDiaryConfigGRepo;
        private readonly IGenericRepository<Kpi_DiaryCriterionDetail> _diaryDetailGRepo;
        private readonly IGenericRepository<Org_UserOrg> _userOrgGRepo;
        private readonly IGenericRepository<Kpi_KpiEvaluation> _kpiEvalRepo;
        private readonly IGenericRepository<Org_Organization> _orgGRepo;
        private readonly IGenericRepository<DivMngPer> _divMngRepo;
        private readonly IGenericRepository<Kpi_JobTitleCriterionType> _jobCriterionGRepo;
        private readonly IGenericRepository<Kpi_CriterionCatalog> _kpiCriterionCatalogGRepo;
        private readonly IGenericRepository<Kpi_CriterionType> _kpiCriterionTypeGRepo;
        private readonly IGenericRepository<Kpi_CriterionTypeCatalog> _kpiCriterionTypeCatalogGRepo;
        private readonly IGenericRepository<Kpi_KpiCriterionDetail> _kpiCriterionDetailGRepo;
        private readonly IGenericRepository<AspNetUser> _userGRepo;
        private readonly IGenericRepository<Org_JobTitle> _jobTitleRepo;
        private readonly IUnitOfWork _uow;

        public EventDiaryRepository(IUnitOfWork unitOfWork)
        {
            this._uow = unitOfWork;
            this._eventDiaryGRepo = this._uow.GetDataRepository<Kpi_EventDiary>();
            this._diaryDetailGRepo = this._uow.GetDataRepository<Kpi_DiaryCriterionDetail>();
            this._eventDiaryConfigGRepo = this._uow.GetDataRepository<Sys_EventDiaryConfig>();
            this._userOrgGRepo = this._uow.GetDataRepository<Org_UserOrg>();
            this._kpiEvalRepo = this._uow.GetDataRepository<Kpi_KpiEvaluation>();
            this._orgGRepo = this._uow.GetDataRepository<Org_Organization>();
            this._divMngRepo = this._uow.GetDataRepository<DivMngPer>();
            this._jobCriterionGRepo = this._uow.GetDataRepository<Kpi_JobTitleCriterionType>();
            this._kpiCriterionCatalogGRepo = this._uow.GetDataRepository<Kpi_CriterionCatalog>();
            this._kpiCriterionTypeCatalogGRepo = this._uow.GetDataRepository<Kpi_CriterionTypeCatalog>();
            this._kpiCriterionDetailGRepo = this._uow.GetDataRepository<Kpi_KpiCriterionDetail>();
            this._jobTitleRepo = this._uow.GetDataRepository<Org_JobTitle>();
            this._userGRepo = this._uow.GetDataRepository<AspNetUser>();
            this._kpiCriterionTypeGRepo = this._uow.GetDataRepository<Kpi_CriterionType>();
        }

        /// <summary>
        /// Lấy nhật ký sự kiện theo manager
        /// </summary>
        public EventDiaryDTO GetMonthlyEventDiaryByManager(int yearMonth, int? orgId, string level1ManagerName, string empName)
        {
            if (yearMonth <= 0 || orgId <= 0)
            {
                return null;
            }

            var eventDiaryDTOList = this._eventDiaryGRepo.SelectWhere(x => x.YearMonth == yearMonth && ((orgId == null) || (x.OrgId == orgId)) &&
                ((!string.IsNullOrEmpty(level1ManagerName) && x.Level1ManagerFullName.Contains(level1ManagerName))
                    || string.IsNullOrEmpty(level1ManagerName)))
                         .Select(x => new EventDiaryDTO
                         {
                             Id = x.Id,
                             FromDate = x.FromDate,
                             ToDate = x.ToDate
                         }).ToList();
            var eventDiaryDTO = eventDiaryDTOList.FirstOrDefault();
            var eventIdList = eventDiaryDTOList.Select(x => x.Id).ToList();

            if (eventDiaryDTO != null)
            {
                // nếu nksk ko tồn tại thì lấy info từ bảng config

                var diaryDetails = (from dt in this._diaryDetailGRepo.SelectWhere(x => x.EventDiaryId != null && eventIdList.Contains(x.EventDiaryId.Value) &&
                     ((!string.IsNullOrEmpty(empName) && x.UserFullName.Contains(empName)) || string.IsNullOrEmpty(empName)))
                                    join cat in _kpiCriterionCatalogGRepo.SelectAll() on dt.CriterionCatalogId equals cat.Id into gj
                                    from tcat in gj.DefaultIfEmpty()
                                    select new DiaryCriterionDetailDTO()
                                    {
                                        Id = dt.Id,
                                        CriterionCatalogId = dt.CriterionCatalogId,
                                        CriterionCatalogCode = dt.CriterionCatalogCode,
                                        CriterionCatalogName = dt.CriterionCatalogName,
                                        CriterionCatalogFolderId = dt.CriterionCatalogFolderId,
                                        UserName = dt.UserName,
                                        UserFullName = dt.UserFullName,
                                        CriterionDate = dt.CriterionDate,
                                        CriterionDayOfMonth = dt.CriterionDayOfMonth,
                                        KpiPoint = dt.KpiPoint,
                                        EventDiaryId = dt.EventDiaryId,
                                        KpiMonthNumber = dt.KpiMonthNumber,
                                        Comment = dt.Comment,
                                        CreatedDate = dt.CreatedDate,
                                        CreatedByUser = dt.CreatedByUser,
                                        CreatedByUserFullName = dt.CreatedByUserFullName,
                                        CreatedByUserTitle = dt.CreatedByUserTitle,
                                        IsDeleted = dt.IsDeleted,
                                        CriterionCatalogNameEn = tcat.CriterionTitleEn
                                    }).ToList();
                eventDiaryDTO.Details = diaryDetails;
                eventDiaryDTO.IdList = eventIdList;
                //throw new ObjectNotFoundException("Nhật ký sự kiện không tồn tại");
            }
            return eventDiaryDTO;
        }

        public EventDiaryDTO GetEventDiaryByKpiManager(int yearMonth, int? orgId, string level1ManagerUserName, string empUserName)
        {
            if (yearMonth <= 0 || orgId <= 0)
            {
                return null;
            }

            var eventDiaryDTOList = this._eventDiaryGRepo.SelectWhere(x => x.YearMonth == yearMonth && ((orgId == null) || (x.OrgId == orgId)) &&
                ((!string.IsNullOrEmpty(level1ManagerUserName) && x.Level1ManagerUserName == level1ManagerUserName)
                    || string.IsNullOrEmpty(level1ManagerUserName)))
                        .Select(x => new EventDiaryDTO
                        {
                            Id = x.Id,
                            FromDate = x.FromDate,
                            ToDate = x.ToDate
                        }).ToList();
            var eventDiaryDTO = eventDiaryDTOList.FirstOrDefault();
            var eventIdList = eventDiaryDTOList.Select(x => x.Id).ToList();

            if (eventDiaryDTO != null)
            {
                // nếu nksk ko tồn tại thì lấy info từ bảng config

                var diaryDetails = this._diaryDetailGRepo.SelectWhere(x => x.EventDiaryId != null && eventIdList.Contains(x.EventDiaryId.Value) &&
                    ((!string.IsNullOrEmpty(empUserName) && x.UserName == empUserName) || string.IsNullOrEmpty(empUserName)))
                        .ProjectTo<DiaryCriterionDetailDTO>().ToList();
                eventDiaryDTO.Details = diaryDetails;
                eventDiaryDTO.IdList = eventIdList;
                //throw new ObjectNotFoundException("Nhật ký sự kiện không tồn tại");
            }
            return eventDiaryDTO;
        }

        public EventDiaryDTO GetMonthlyEventDiaryByHRManager(int yearMonth, int? orgId, string level1ManagerUserName, string empUserName)
        {
            if (yearMonth <= 0 || orgId <= 0)
            {
                return null;
            }

            var eventDiaryDTOList = this._eventDiaryGRepo.SelectWhere(x => x.YearMonth == yearMonth && ((orgId == null) || (x.OrgId == orgId)) &&
                ((!string.IsNullOrEmpty(level1ManagerUserName) && x.Level1ManagerUserName == level1ManagerUserName)
                    || string.IsNullOrEmpty(level1ManagerUserName)))
                      .Select(x => new EventDiaryDTO
                      {
                          Id = x.Id,
                          FromDate = x.FromDate,
                          ToDate = x.ToDate
                      }).ToList();
            var eventDiaryDTO = eventDiaryDTOList.FirstOrDefault();
            var eventIdList = eventDiaryDTOList.Select(x => x.Id).ToList();

            if (eventDiaryDTO != null)
            {
                // nếu nksk ko tồn tại thì lấy info từ bảng config

                var diaryDetails = this._diaryDetailGRepo.SelectWhere(x => x.EventDiaryId != null && eventIdList.Contains(x.EventDiaryId.Value) &&
                    ((!string.IsNullOrEmpty(empUserName) && x.UserName == empUserName) || string.IsNullOrEmpty(empUserName)))
                        .ProjectTo<DiaryCriterionDetailDTO>().ToList();
                eventDiaryDTO.Details = diaryDetails;
                eventDiaryDTO.IdList = eventIdList;
                //throw new ObjectNotFoundException("Nhật ký sự kiện không tồn tại");
            }
            return eventDiaryDTO;
        }

        public EventDiaryDTO GetMonthlyEventDiaryByManagerEMP(EventDiaryDTO eventDiaryDTO)
        {
            if (eventDiaryDTO != null)
            {
                // nếu nksk ko tồn tại thì lấy info từ bảng config

                var diaryDetails = (from dt in this._diaryDetailGRepo.SelectWhere(x => x.EventDiaryId != null && x.EventDiaryId.Value == eventDiaryDTO.Id)
                                    join cat in _kpiCriterionCatalogGRepo.SelectAll() on dt.CriterionCatalogId equals cat.Id into gj
                                    from tcat in gj.DefaultIfEmpty()
                                    select new DiaryCriterionDetailDTO()
                                    {
                                        Id = dt.Id,
                                        CriterionCatalogId = dt.CriterionCatalogId,
                                        CriterionCatalogCode = dt.CriterionCatalogCode,
                                        CriterionCatalogName = dt.CriterionCatalogName,
                                        CriterionCatalogFolderId = dt.CriterionCatalogFolderId,
                                        UserName = dt.UserName,
                                        UserFullName = dt.UserFullName,
                                        CriterionDate = dt.CriterionDate,
                                        CriterionDayOfMonth = dt.CriterionDayOfMonth,
                                        KpiPoint = dt.KpiPoint,
                                        EventDiaryId = dt.EventDiaryId,
                                        KpiMonthNumber = dt.KpiMonthNumber,
                                        Comment = dt.Comment,
                                        CreatedDate = dt.CreatedDate,
                                        CreatedByUser = dt.CreatedByUser,
                                        CreatedByUserFullName = dt.CreatedByUserFullName,
                                        CreatedByUserTitle = dt.CreatedByUserTitle,
                                        IsDeleted = dt.IsDeleted,
                                        CriterionCatalogNameEn = tcat.CriterionTitleEn
                                    }).ToList();
                eventDiaryDTO.Details = diaryDetails;
                //throw new ObjectNotFoundException("Nhật ký sự kiện không tồn tại");
            }
            return eventDiaryDTO;
        }

        public EventDiaryDTO GetMonthlyEventDiaryByEmp(int yearMonth, int? orgId, string empUserName)
        {
            var kpi = _kpiEvalRepo.SelectWhere(x => x.UserName == empUserName && x.YearMonth == yearMonth).FirstOrDefault();
            if (kpi != null)
            {
                var evnt = _eventDiaryGRepo.SelectWhere(x => x.Id == kpi.EventDiaryId).ProjectTo<EventDiaryDTO>().FirstOrDefault();
                return this.GetMonthlyEventDiaryByManagerEMP(evnt);
            }
            return null;
        }

        public void Dispose()
        {
            if (this._uow != null)
            {
                this._uow.Dispose();
            }
        }

        public EventDiaryDTO GetMonthlyEventDiaryByKpiId(int kpiId)
        {
            EventDiaryDTO eventDiary = null;
            var kpi = this._kpiEvalRepo.SelectWhere(x => x.Id == kpiId).FirstOrDefault();
            if (kpi != null)
            {
                eventDiary = this._eventDiaryGRepo.SelectWhere(x => x.Id == kpi.EventDiaryId).ProjectTo<EventDiaryDTO>().FirstOrDefault();

                if (eventDiary != null)
                {
                    //var diaryDetails = this._diaryDetailGRepo.SelectWhere(x => x.UserName != null && x.UserName.ToLower() == kpi.UserName.ToLower()
                    //    && (x.EventDiaryId.HasValue && eventDiary.Id == x.EventDiaryId.Value)).ProjectTo<DiaryCriterionDetailDTO>().ToList();

                    //eventDiary.Details = diaryDetails;

                    var diaryDetails = from dt in this._diaryDetailGRepo.SelectWhere(x => x.UserName != null && x.UserName.ToLower() == kpi.UserName.ToLower()
                        && (x.EventDiaryId.HasValue && eventDiary.Id == x.EventDiaryId.Value))
                                       join cat in _kpiCriterionCatalogGRepo.SelectAll() on dt.CriterionCatalogId equals cat.Id into gj
                                       from tcat in gj.DefaultIfEmpty()
                                       select new { dt, CriterionCatalogNameEn = tcat.CriterionTitleEn };
                    var usrJobt = from u in _userOrgGRepo.SelectAll()
                                  join j in _jobTitleRepo.SelectAll() on u.JobTitleId equals j.Id
                                  select new { uId = u.Id.ToString(), jobTl = j.TitleEn };
                    var dtls = (from dt in diaryDetails
                                join jt in usrJobt on dt.dt.CreatedByUser equals jt.uId into gj
                                from tcat in gj.DefaultIfEmpty()
                                select new DiaryCriterionDetailDTO()
                                {
                                    Id = dt.dt.Id,
                                    CriterionCatalogId = dt.dt.CriterionCatalogId,
                                    CriterionCatalogCode = dt.dt.CriterionCatalogCode,
                                    CriterionCatalogName = dt.dt.CriterionCatalogName,
                                    CriterionCatalogFolderId = dt.dt.CriterionCatalogFolderId,
                                    UserName = dt.dt.UserName,
                                    UserFullName = dt.dt.UserFullName,
                                    CriterionDate = dt.dt.CriterionDate,
                                    CriterionDayOfMonth = dt.dt.CriterionDayOfMonth,
                                    KpiPoint = dt.dt.KpiPoint,
                                    EventDiaryId = dt.dt.EventDiaryId,
                                    KpiMonthNumber = dt.dt.KpiMonthNumber,
                                    Comment = dt.dt.Comment,
                                    CreatedDate = dt.dt.CreatedDate,
                                    CreatedByUser = dt.dt.CreatedByUser,
                                    CreatedByUserFullName = dt.dt.CreatedByUserFullName,
                                    CreatedByUserTitle = dt.dt.CreatedByUserTitle,
                                    IsDeleted = dt.dt.IsDeleted,
                                    CriterionCatalogNameEn = dt.CriterionCatalogNameEn,
                                    CreatedByUserTitleEn = tcat.jobTl
                                }).ToList();

                    eventDiary.Details = dtls;
                }
            }
            return eventDiary;
        }

        public EventDiaryDTO GetMonthlyEventDiaryByManagerLv2(int yearMonth, int? orgId, string userName)
        {
            var eventDiaryDTO = this._eventDiaryGRepo.SelectWhere(x => x.YearMonth == yearMonth && x.OrgId == orgId && x.Level2ManagerUserName == userName)
                   .ProjectTo<EventDiaryDTO>().FirstOrDefault();

            if (eventDiaryDTO != null)
            {
                // nếu nksk ko tồn tại thì lấy info từ bảng config

                var diaryDetails = this._diaryDetailGRepo.SelectWhere(x => x.EventDiaryId != null && x.EventDiaryId.Value == eventDiaryDTO.Id).ProjectTo<DiaryCriterionDetailDTO>().ToList();
                eventDiaryDTO.Details = diaryDetails;
                //throw new ObjectNotFoundException("Nhật ký sự kiện không tồn tại");
            }
            return eventDiaryDTO;
        }

        public EventDiaryDTO GetEventDiaryByKpiManager(int yearMonth, int? orgId)
        {
            var eventDiaryDTO = this._eventDiaryGRepo.SelectWhere(x => x.YearMonth == yearMonth && x.OrgId == orgId)
                   .ProjectTo<EventDiaryDTO>().FirstOrDefault();

            if (eventDiaryDTO != null)
            {
                // nếu nksk ko tồn tại thì lấy info từ bảng config
                var diaryDetails = this._diaryDetailGRepo.SelectWhere(x => x.EventDiaryId != null && x.EventDiaryId.Value == eventDiaryDTO.Id).ProjectTo<DiaryCriterionDetailDTO>().ToList();
                eventDiaryDTO.Details = diaryDetails;
                //throw new ObjectNotFoundException("Nhật ký sự kiện không tồn tại");
            }
            return eventDiaryDTO;
        }

        public EventDiaryPagingModelDTO GetPaging(EventDiaryFilterModelDTO filter)
        {
            var query = this._eventDiaryGRepo.SelectAll();
            query = this.Filter(query, filter);
            var paging = query.OrderBy(x => x.Id).Skip(filter.Start - 1).Take(filter.Length).ProjectTo<EventDiaryDTO>().ToList();
            var count = query.Count();
            return new EventDiaryPagingModelDTO
            {
                Paging = paging,
                Count = count,
            };
        }

        private IQueryable<Kpi_EventDiary> Filter(IQueryable<Kpi_EventDiary> query, EventDiaryFilterModelDTO filter)
        {
            var rs = query;
            if (filter.YearMonth > 0)
            {
                rs = rs.Where(x => x.YearMonth == filter.YearMonth);
            }

            if (filter.OrgId > 0)
            {
                var orgPath = this._orgGRepo.SelectWhere(x => x.Id == filter.OrgId).Select(x => x.DirectoryPath).FirstOrDefault();
                var childOrgs = this._orgGRepo.SelectWhere(x => x.DirectoryPath.StartsWith(orgPath)).Select(x => x.Id).ToList();
                rs = rs.Where(x => childOrgs.Contains(x.OrgId));
            }
            return rs;
        }

        /// <summary>
        /// huypq modified 2/3/2020, theo yêu cầu của user sẽ thêm trường hợp search all,        
        /// trong trường hợp search all, để ko ảnh hưởng tới code frontend, sẽ coi kết quả là 1 event diary lớn (mặc định lấy event diary đầu tiên), có các details là tổng hợp của các event diary con
        /// #272
        /// </summary>
        public EventDiaryDTO GetEventDiaryByManagerLv2(int yearMonth, int? orgId, string level1ManagerUserName, string level2ManagerUserName, string empName)
        {
            if (yearMonth <= 0 || orgId <= 0)
            {
                return null;
            }

            var eventDiaryDTOList = this._eventDiaryGRepo.SelectWhere(x => x.YearMonth == yearMonth && ((orgId == null) || (x.OrgId == orgId)) &&
                ((!string.IsNullOrEmpty(level1ManagerUserName) && x.Level1ManagerUserName == level1ManagerUserName)
                    || string.IsNullOrEmpty(level1ManagerUserName)) && (x.Level2ManagerUserName == level2ManagerUserName))
                       .Select(x => new EventDiaryDTO
                       {
                           Id = x.Id,
                           FromDate = x.FromDate,
                           ToDate = x.ToDate
                       }).ToList();
            var eventDiaryDTO = eventDiaryDTOList.FirstOrDefault();
            var eventIdList = eventDiaryDTOList.Select(x => x.Id).ToList();

            if (eventDiaryDTO != null)
            {
                // nếu nksk tồn tại thì lấy info từ bảng config
                var diaryDetails = from dt in this._diaryDetailGRepo.SelectWhere(x => x.EventDiaryId != null && eventIdList.Contains(x.EventDiaryId.Value) && ((!string.IsNullOrEmpty(empName) && x.UserFullName.Contains(empName)) || string.IsNullOrEmpty(empName)))
                                   join cat in _kpiCriterionCatalogGRepo.SelectAll() on dt.CriterionCatalogId equals cat.Id into gj
                                   from tcat in gj.DefaultIfEmpty()
                                   select new { dt, CriterionCatalogNameEn = tcat.CriterionTitleEn };
                var usrJobt = from u in _userOrgGRepo.SelectAll()
                              join j in _jobTitleRepo.SelectAll() on u.JobTitleId equals j.Id
                              select new { uId = u.Id.ToString(), jobTl = j.TitleEn };
                var dtls = (from dt in diaryDetails
                            join jt in usrJobt on dt.dt.CreatedByUser equals jt.uId into gj
                            from tcat in gj.DefaultIfEmpty()
                            select new DiaryCriterionDetailDTO()
                            {
                                Id = dt.dt.Id,
                                CriterionCatalogId = dt.dt.CriterionCatalogId,
                                CriterionCatalogCode = dt.dt.CriterionCatalogCode,
                                CriterionCatalogName = dt.dt.CriterionCatalogName,
                                CriterionCatalogFolderId = dt.dt.CriterionCatalogFolderId,
                                UserName = dt.dt.UserName,
                                UserFullName = dt.dt.UserFullName,
                                CriterionDate = dt.dt.CriterionDate,
                                CriterionDayOfMonth = dt.dt.CriterionDayOfMonth,
                                KpiPoint = dt.dt.KpiPoint,
                                EventDiaryId = dt.dt.EventDiaryId,
                                KpiMonthNumber = dt.dt.KpiMonthNumber,
                                Comment = dt.dt.Comment,
                                CreatedDate = dt.dt.CreatedDate,
                                CreatedByUser = dt.dt.CreatedByUser,
                                CreatedByUserFullName = dt.dt.CreatedByUserFullName,
                                CreatedByUserTitle = dt.dt.CreatedByUserTitle,
                                IsDeleted = dt.dt.IsDeleted,
                                CriterionCatalogNameEn = dt.CriterionCatalogNameEn,
                                CreatedByUserTitleEn = tcat.jobTl
                            }).ToList();

                eventDiaryDTO.Details = dtls;
                eventDiaryDTO.IdList = eventIdList;
                //throw new ObjectNotFoundException("Nhật ký sự kiện không tồn tại");
            }
            return eventDiaryDTO;
        }

        public IEnumerable<UserOrgDTO> GetLevel1UserByLevel2Manager(string userName, int? orgId)
        {
            var ret = new List<UserOrgDTO>();
            var cfgs = _eventDiaryConfigGRepo.SelectWhere(x => x.OrgId == orgId && x.Level2ManagerUserName == userName).ToList();
            if (cfgs.Count > 0)
            {
                // lay danh sach quan ly truc tiep
                foreach (var cfg in cfgs)
                {
                    var usr = _userOrgGRepo.SelectWhere(x => x.UserName == cfg.Level1ManagerUserName).ProjectTo<UserOrgDTO>().FirstOrDefault();
                    if (usr != null)
                    {
                        ret.Add(usr);
                    }
                }
            }
            return ret;
        }

        public PagingModelDTO GetPagingExistedEventDiaryByYearMonth(EventDiaryFilterModelDTO filter)
        {
            var query = (from ed in this._eventDiaryGRepo.SelectAll()
                         join ec in this._eventDiaryConfigGRepo.SelectWhere(x => x.IsActive == true) on ed.EventDiaryConfigId equals ec.Id
                         select new EventDiarySyncModelDTO
                         {
                             Id = ed.Id,
                             OrgId = ed.OrgId,
                             OrgName = ed.OrgName,
                             Level1ManagerFullName = ed.Level1ManagerFullName,
                             Level1ManagerUserName = ed.Level1ManagerUserName,
                             Level2ManagerFullName = ed.Level2ManagerFullName,
                             Level2ManagerUserName = ed.Level2ManagerUserName,
                             Code = ec.Code,
                             FromDate = ed.FromDate,
                             ToDate = ed.ToDate,
                             YearMonth = ed.YearMonth,
                             EventDiaryConfigId = ed.EventDiaryConfigId
                         });
            query = this.FilterExistedEventDiaryByYearMonth(query, filter);
            var paging = query.OrderBy(x => x.Id).Skip(filter.Start - 1).Take(filter.Length).ToList();
            var count = query.Count();
            return new PagingModelDTO
            {
                Paging = paging,
                Count = count,
            };
        }

        private IQueryable<EventDiarySyncModelDTO> FilterExistedEventDiaryByYearMonth(IQueryable<EventDiarySyncModelDTO> query, EventDiaryFilterModelDTO filter)
        {
            var rs = query;
            if (filter.YearMonth > 0)
            {
                rs = rs.Where(x => x.YearMonth == filter.YearMonth);
            }

            if (filter.OrgId > 0)
            {
                rs = rs.Where(x => x.OrgId == filter.OrgId);
            }

            if (!string.IsNullOrEmpty(filter.Level1ManagerUsername))
            {
                rs = rs.Where(x => x.Level1ManagerUserName == filter.Level1ManagerUsername);
            }

            return rs;
        }

        private IQueryable<EventDiarySyncModelDTO> FilterMissingEventDiaryByYearMonth(IQueryable<EventDiarySyncModelDTO> query, EventDiaryFilterModelDTO filter)
        {
            var rs = query;
            if (filter.OrgId > 0)
            {
                rs = rs.Where(x => x.OrgId == filter.OrgId);
            }

            if (!string.IsNullOrEmpty(filter.Level1ManagerUsername))
            {
                rs = rs.Where(x => x.Level1ManagerUserName == filter.Level1ManagerUsername);
            }

            return rs;
        }

        public PagingModelDTO GetPagingMissingEventDiaryByYearMonth(EventDiaryFilterModelDTO filter)
        {
            var query = (from ec in this._eventDiaryConfigGRepo.SelectWhere(x => x.IsActive == true)
                         join ed in this._eventDiaryGRepo.SelectWhere(x => x.YearMonth == filter.YearMonth) on ec.Id equals ed.EventDiaryConfigId into edx
                         from edn in edx.DefaultIfEmpty()
                         select new EventDiarySyncModelDTO
                         {
                             Id = edn != null ? edn.Id : 0,
                             OrgId = ec.OrgId.Value,
                             OrgName = ec.OrgName,
                             Level1ManagerFullName = ec.Level1ManagerFullName,
                             Level1ManagerUserName = ec.Level1ManagerUserName,
                             Level2ManagerFullName = ec.Level2ManagerFullName,
                             Level2ManagerUserName = ec.Level2ManagerUserName,
                             Code = ec.Code,
                             EventDiaryConfigId = ec.Id,
                             FromDate = (edn != null) ? edn.FromDate : null,
                             ToDate = (edn != null) ? edn.ToDate : null,
                             YearMonth = (edn != null) ? edn.YearMonth : null,
                         }).Where(x => x.Id == 0);
            query = this.FilterMissingEventDiaryByYearMonth(query, filter);
            var paging = query.OrderBy(x => x.Id).Skip(filter.Start - 1).Take(filter.Length).ToList();
            var count = query.Count();
            return new PagingModelDTO
            {
                Paging = paging,
                Count = count,
            };
        }

        public IEnumerable<UserOrgDTO> GetUsers()
        {
            // lay danh sach nhan vien dang con lam viec o cty
            return _userOrgGRepo.SelectWhere(x => x.Status != "NV").ProjectTo<UserOrgDTO>().ToList();
        }

        #region config, eventdiary, kpi change

        /// <summary>
        /// Tạo nksk và kpi theo kì + config nksk
        /// </summary>
        public void AddRangeDiaryKpi(IEnumerable<EventDiarySyncModelDTO> configs, string userName)
        {
            using (var ctx = this._uow.BeginTransaction())
            {
                // tạo diaries
                var diaries = this.CreateEventDiaries(configs);

                // tạo, chỉnh sửa kpi
                this.CreateOrUpdateKpiByDiaryNewConfig(diaries, configs, userName);
                ctx.Commit();
            }
        }

        /// <summary>
        /// Đã có nksk, chỉ update lại thông tin nksk và kpi
        /// Các trường hợp
        /// Bớt user ra khỏi config nksk => set config của nksk và kpi => null
        /// Thêm user , nếu chưa có kpi => tạo mới, đã có kpi => update lại (Giống trường hợp tạo mới)
        /// Đổi thông tin config nksk => đổi thông tin nksk => đổi thông tin kpi
        /// Các bước tiến hành : 1 => update lại nksk, 2 => update lại user
        /// </summary>
        public void UpdateRangeDiaryKpiConfigChanged(IEnumerable<EventDiarySyncModelDTO> configs, string currentUser)
        {
            using (var ctx = this._uow.BeginTransaction())
            {
                var evDiaries = this.UpdateEventDiaryConfigChanged(configs);
                this.UpdateKpiConfigChanged(/*configs,*/ currentUser, evDiaries);
                ctx.Commit();
            }
        }

        /// <summary>
        /// Update lại thông tin nksk theo config
        /// </summary>
        private IEnumerable<Kpi_EventDiary> UpdateEventDiaryConfigChanged(IEnumerable<EventDiarySyncModelDTO> configs)
        {
            var ym = configs.FirstOrDefault().YearMonth;
            var fromDate = configs.FirstOrDefault().FromDate;
            var toDate = configs.FirstOrDefault().ToDate;

            // get new config info
            var cfIds = configs.Select(x => x.EventDiaryConfigId).ToList();
            var updatedConfigs = this._eventDiaryConfigGRepo.SelectWhere(x => cfIds.Contains(x.Id)).ToList();

            var properties = new List<Expression<Func<Kpi_EventDiary, object>>> { x => x.OrgId, x => x.OrgName, x => x.Level1ManagerUserName, x => x.Level1ManagerFullName
                    , x => x.Level2ManagerUserName, x => x.Level2ManagerFullName };

            var eventDiaries = new List<Kpi_EventDiary>();
            foreach (var c in configs)
            {
                var updatedConfig = updatedConfigs.FirstOrDefault(x => x.Id == c.EventDiaryConfigId);
                if (updatedConfig == null)
                {
                    continue;
                }

                var ed = new Kpi_EventDiary
                {
                    Id = c.Id,
                    OrgId = updatedConfig.OrgId.GetValueOrDefault(),
                    OrgName = updatedConfig.OrgName,
                    Level1ManagerUserName = updatedConfig.Level1ManagerUserName,
                    Level1ManagerFullName = updatedConfig.Level1ManagerFullName,
                    Level2ManagerFullName = updatedConfig.Level2ManagerFullName,
                    Level2ManagerUserName = updatedConfig.Level2ManagerUserName,
                    EventDiaryConfigId = updatedConfig.Id,
                    FromDate = fromDate,
                    ToDate = toDate,
                    YearMonth = ym
                };
                this._eventDiaryGRepo.UpdateByProperties(ed, properties);
                eventDiaries.Add(ed);
            }
            this._uow.SaveChanges();
            return eventDiaries;
        }

        /// <summary>
        /// Update lại thông tin KPI
        /// </summary>
        private void UpdateKpiConfigChanged(/*IEnumerable<EventDiarySyncModelDTO> configs,*/ string currentUserName, IEnumerable<Kpi_EventDiary> eventDiaries)
        {
            var listEditedEventDiaries = eventDiaries.Select(x => x.Id).ToList();
            var configs = eventDiaries.Select(x => new EventDiarySyncModelDTO
            {
                Id = x.Id,
                OrgId = x.OrgId,
                OrgName = x.OrgName,
                Level1ManagerUserName = x.Level1ManagerUserName,
                Level1ManagerFullName = x.Level1ManagerFullName,
                Level2ManagerUserName = x.Level2ManagerUserName,
                Level2ManagerFullName = x.Level2ManagerFullName,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                YearMonth = x.YearMonth,
                EventDiaryConfigId = x.EventDiaryConfigId
            });
            var fromDate = configs.FirstOrDefault().FromDate;
            var toDate = configs.FirstOrDefault().ToDate;

            // Lấy những kpi thuộc nksk này
            var eventKpiList = this._kpiEvalRepo.SelectWhere(x => listEditedEventDiaries.Contains(x.EventDiaryId.Value)).ToList();

            // Lấy những User thuộc config
            var configsList = configs.Select(x => x.EventDiaryConfigId).ToList();
            var userList = this._userOrgGRepo.SelectWhere(x => configsList.Contains(x.EventDiaryConfigId.Value)).ToList();

            this.DeleteEmpKpiFromConfig(configs, eventKpiList, userList, currentUserName);
            //var diaries = 
            this.CreateOrUpdateKpiByDiaryNewConfig(eventDiaries, configs, currentUserName);
            //this.UpdateEmpKpiFromConfig(configs);
        }

        /// <summary>
        /// Những user nào có configId trong userOrg = null thì set 
        /// </summary>
        private void DeleteEmpKpiFromConfig(IEnumerable<EventDiarySyncModelDTO> configs, List<Kpi_KpiEvaluation> kpiList, List<Org_UserOrg> userList, string currentUser)
        {
            var userIdList = userList.Select(x => x.UserName).ToList();
            // Tách những kpi nào của user ko nằm trong danh sách user của configs này
            var noUserKpiList = kpiList.FindAll(x => !userIdList.Contains(x.UserName));
            if (noUserKpiList == null || !noUserKpiList.Any())
            {
                return;
            }

            foreach (var k in noUserKpiList)
            {
                k.UpdatedByUser = currentUser;
                k.UpdatedDate = DateTime.Now;
                k.EventDiaryConfigId = null;
                k.EventDiaryId = null;
                k.Level1ManagerFullName = string.Empty;
                k.Level1ManagerJobTitle = string.Empty;
                k.Level1ManagerJobTitleId = 0;
                k.Level1ManagerOrgId = 0;
                k.Level1ManagerOrgName = string.Empty;
                k.Level1ManagerUserName = string.Empty;
                k.Level2ManagerUserName = string.Empty;
                k.Level2ManagerJobTitle = string.Empty;
                k.Level2ManagerOrgName = string.Empty;
                k.Level2ManagerFullName = string.Empty;
                this._kpiEvalRepo.Update(k);
                this._uow.SaveChanges();
            }
        }

        private void AddEmpKpiFromConfig(IEnumerable<EventDiarySyncModelDTO> configs)
        {

        }

        private void UpdateEmpKpiFromConfig(IEnumerable<EventDiarySyncModelDTO> configs)
        {

        }

        /// <summary>
        /// Tạo nksk theo config + kì
        /// </summary>
        private IEnumerable<Kpi_EventDiary> CreateEventDiaries(IEnumerable<EventDiarySyncModelDTO> configs)
        {
            var edcEntities = configs.Select(x => new Kpi_EventDiary
            {
                OrgId = x.OrgId,
                OrgName = x.OrgName,
                Level1ManagerUserName = x.Level1ManagerUserName,
                Level1ManagerFullName = x.Level1ManagerFullName,
                Level2ManagerUserName = x.Level2ManagerUserName,
                Level2ManagerFullName = x.Level2ManagerFullName,
                YearMonth = x.YearMonth,
                EventDiaryConfigId = x.EventDiaryConfigId,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
            }).ToList();
            var rs = this._eventDiaryGRepo.AddRange(edcEntities);
            this._uow.SaveChanges();
            return rs;
        }

        /// <summary>
        /// trường hợp event diary đã đủ hết, nhưng chưa có kpi (hoặc đã có kpi nhưng ở config khác chưa chuyển sang config mới)
        /// update lại thông tin kpi theo nksk trong khoảng kỳ đã chọn
        /// Bao gồm toàn bộ các trường hợp, Thêm mới nv (thêm mới kpi), bớt nv (bỏ kpi ra khỏi nksk và nksk config), update thông tin kpi theo thông tin nksk
        /// Cover cả trường hợp thêm config và sửa config
        /// </summary>
        private void CreateOrUpdateKpiByDiaryNewConfig(IEnumerable<Kpi_EventDiary> diaries, IEnumerable<EventDiarySyncModelDTO> configs, string currentUserName)
        {
            var edConfigs = diaries.Select(x => x.EventDiaryConfigId).Distinct().ToList();
            // lấy list user thuộc nksk này
            var listUser = this.GetUserByEventDiaryConfig(edConfigs);
            var listUserName = listUser.Select(x => x.UserName).ToList();

            // update kpi sang diary mới
            // Lấy những kpi của user đã có cấu hình đánh giá cũ, những kpi này sẽ chuyển sang cấu hình mới chứ ko thêm mới
            var ym = configs.FirstOrDefault().YearMonth;
            var existedKpi = this._kpiEvalRepo.SelectWhere(k => listUserName.Contains(k.UserName) && k.YearMonth == ym).ToList();

            // sửa kpi cho những user đã có kpi cả thuộc và không thuộc config này
            var existedKpiUser = listUser.Where(x => existedKpi.Any(k => k.UserName == x.UserName));
            this.UpdateKpiInfo(existedKpiUser, diaries, existedKpi, configs, currentUserName);

            // Thêm mới kpi cho những user chưa có kpi
            var noKpiUser = listUser.Where(x => !existedKpi.Any(k => k.UserName == x.UserName));
            this.AddKpi(noKpiUser, diaries);

        }

        /// <summary>
        /// Update thông tin tất cả kpi theo thông tin nksk, bao gồm các kpi hiện vẫn đang thuộc nksk này và các kpi đang thuộc nksk khác
        /// </summary>
        private void UpdateKpiInfo(IEnumerable<UserInfoModel> existedUser, IEnumerable<Kpi_EventDiary> diaries,
            IEnumerable<Kpi_KpiEvaluation> existedKpis, IEnumerable<EventDiarySyncModelDTO> configs, string currentUserName)
        {
            foreach (var u in existedUser)
            {
                var newDiary = diaries.FirstOrDefault(x => x.EventDiaryConfigId == u.EventDiaryConfigId);
                if (newDiary == null) continue;

                var config = configs.FirstOrDefault(x => x.EventDiaryConfigId == u.EventDiaryConfigId);
                if (config == null) continue;

                var kpi = existedKpis.FirstOrDefault(x => x.UserName == u.UserName);
                if (kpi == null) continue;

                var level1Mng = this._userOrgGRepo.SelectWhere(x => x.UserName == config.Level1ManagerUserName).FirstOrDefault();
                var level2Mng = this._userOrgGRepo.SelectWhere(x => x.UserName == config.Level2ManagerUserName).FirstOrDefault();

                kpi.OrganizationId = u.OrgId;
                kpi.Organization = u.OrgName;
                kpi.EmployeeJobTitle = u.JobTitle;
                kpi.EventDiaryConfigId = config.EventDiaryConfigId;
                kpi.EventDiaryId = newDiary.Id;
                kpi.Level1ManagerUserId = this._userGRepo.SelectWhere(x => x.UserName == config.Level1ManagerUserName).Select(x => x.Id).FirstOrDefault();
                kpi.Level1ManagerUserName = level1Mng.UserName;
                kpi.Level1ManagerFullName = level1Mng.UserFullName;
                kpi.Level1ManagerJobTitle = level1Mng.JobTitle;
                kpi.Level1ManagerOrgId = level1Mng.OrgId;
                kpi.Level1ManagerOrgName = level1Mng.OrgName;

                //kpi.Level2ManagerUserId = this._userGRepo.SelectWhere(x => x.UserName == config.Level2ManagerUserName).Select(x => x.Id).FirstOrDefault();
                kpi.Level2ManagerUserName = level2Mng.UserName;
                kpi.Level2ManagerFullName = level2Mng.UserFullName;
                kpi.Level2ManagerJobTitle = level2Mng.JobTitle;
                kpi.Level2ManagerOrgName = level2Mng.OrgName;
                kpi.UpdatedByUser = currentUserName;
                kpi.UpdatedDate = DateTime.Now;
                //kpi.Level1Manager
                ///xxx

                this._kpiEvalRepo.Update(kpi);
                this._uow.SaveChanges();
            }
        }

        /// <summary>
        /// Lấy user theo config
        /// </summary>
        private IEnumerable<UserInfoModel> GetUserByEventDiaryConfig(IEnumerable<int> edConfigs)
        {
            var query = (from uo in this._userOrgGRepo.SelectWhere(x => edConfigs.Contains(x.EventDiaryConfigId.Value))
                         join u in this._userGRepo.SelectAll() on uo.UserName equals u.UserName
                         join ec in this._eventDiaryConfigGRepo.SelectAll() on uo.EventDiaryConfigId equals ec.Id
                         //join jc in this._jobCriterionGRepo.SelectAll() on uo.JobTitleId equals jc.JobTitleId
                         join c in this._kpiCriterionTypeGRepo.SelectAll() on uo.KpiType equals c.Code
                         select new UserInfoModel
                         {
                             UserId = u.Id,
                             OrgId = uo.OrgId,
                             UserName = uo.UserName,
                             UserFullName = uo.UserFullName,
                             UserEmail = uo.UserEmail,
                             JobTitle = uo.JobTitle,
                             JobTitleId = uo.JobTitleId.Value,
                             Level1ManagerUserName = ec.Level1ManagerUserName,
                             Level1ManagerFullName = ec.Level1ManagerFullName,
                             Level1ManagerOrgName = ec.Level1ManagerOrgName,
                             Level1ManagerJobTitle = ec.Level1ManagerJobTitle,
                             Level2ManagerOrgName = ec.Level2ManagerOrgName,
                             Level2ManagerJobTitle = ec.Level2ManagerJobTitle,
                             Level2ManagerUserName = ec.Level2ManagerUserName,
                             Level2ManagerFullName = ec.Level2ManagerFullName,
                             EventDiaryConfigId = uo.EventDiaryConfigId,
                             OrgName = uo.OrgName,
                             CriterionType = c.Id,
                             CriterionTypeName = c.Name,
                             CriterionTypeNameEn = c.NameEn,
                             KpiType = c.Code
                         });
            return query.ToList();
        }

        // tạo kpi cho những user chưa có kpi nhưng đã có diaries
        private void AddKpi(IEnumerable<UserInfoModel> users, IEnumerable<Kpi_EventDiary> diaries)
        {
            foreach (var u in users)
            {
                if (u.EventDiaryConfigId == null || u.EventDiaryConfigId == 0)
                {
                    continue;
                }
                var userDiaries = diaries.Where(x => x.EventDiaryConfigId == u.EventDiaryConfigId);
                if (userDiaries == null)
                {
                    continue;
                }
                var kpis = userDiaries.Select(diary => new Kpi_KpiEvaluation
                {
                    UserName = u.UserName,
                    EventDiaryId = diary.Id,
                    EmployeeName = u.UserFullName,
                    EmployeeJobId = u.JobTitleId.GetValueOrDefault(),
                    EmployeeJobTitle = u.JobTitle,
                    //Level1ManagerOrgId = u.Level1ManagerOrgId.GetValueOrDefault(),
                    KpiMonthNumber = diary.ToDate.Value.Month,
                    KpiMonth = DateTime.ParseExact(diary.YearMonth.ToString(), "yyyyMM", System.Globalization.CultureInfo.InvariantCulture),
                    YearMonth = diary.YearMonth,
                    KpiPeriodConfigId = diary.EventDiaryConfigId,
                    CriterionType = u.CriterionType,
                    CriterionTypeName = u.CriterionTypeName,
                    CriterionTypeNameEn = u.CriterionTypeNameEn,
                    CreatedDate = DateTime.Now,
                    CreatedByUser = string.Empty,
                    StatusId = 1,
                    StatusName = "Khởi tạo",
                    IsDeleted = false,
                    Level1ManagerUserName = u.Level1ManagerUserName,
                    Level1ManagerFullName = u.Level1ManagerFullName,
                    Level1ManagerJobTitleId = u.Level1ManagerJobTileId,
                    Level1ManagerJobTitle = u.Level1ManagerJobTitle,
                    Level1ManagerOrgName = u.Level1ManagerOrgName,
                    Level1ManagerUserId = u.UserId,
                    Level2ManagerUserName = u.Level2ManagerUserName,
                    Level2ManagerFullName = u.Level2ManagerFullName,
                    Level2ManagerJobTitle = u.Level2ManagerJobTitle,
                    Level2ManagerOrgName = u.Level2ManagerOrgName,
                    Organization = u.OrgName,
                    OrganizationId = u.OrgId,
                    EventDiaryConfigId = diary.EventDiaryConfigId,
                    EmpKpiClassification = "A",
                    EmpKpiPoint = 100,
                    Level1ManagerKpiPoint = 100,
                    Level1ManagerKpiClassification = "A",
                    CodeUser = u.Code
                });

                foreach (var kpi in kpis)
                {
                    var kpiEntity = this._kpiEvalRepo.Add(kpi);
                    this._uow.SaveChanges();

                    // add kpi criterion detail
                    var criterionCatalogs = this.GetCriterionCatalogByCriterionType(kpi.CriterionType);
                    var kpiCriterionDetails = criterionCatalogs.Select(x => new Kpi_KpiCriterionDetail
                    {
                        CriterionTitle = x.CriterionTitle,
                        MinimumPoint = x.MinimumPoint,
                        MaximumPoint = x.MaximumPoint,
                        EmployeeEvaluatePoint = x.MaximumPoint,
                        ManagerEvaluatePoint = x.MaximumPoint,
                        Month = kpi.KpiMonthNumber,
                        Year = kpi.YearMonth,
                        UserId = u.UserId,
                        OrgId = u.OrgId,
                        KpiEvaluateId = kpiEntity.Id,
                        KpiCatalogId = x.Id,
                        CreatedDate = DateTime.Now,
                    });
                    this._kpiCriterionDetailGRepo.AddRange(kpiCriterionDetails);
                    this._uow.SaveChanges();
                }
            }
        }
        #endregion

        private IEnumerable<CriterionCatalogDTO> GetCriterionCatalogByCriterionType(int type)
        {
            var qr = (from c in this._kpiCriterionCatalogGRepo.SelectWhere(x => x.IsFolder)
                      join ct in this._kpiCriterionTypeCatalogGRepo.SelectWhere(x => x.CriterionTypeId == type) on c.Id equals ct.CriterionCatalogId
                      select new CriterionCatalogDTO
                      {
                          Id = c.Id,
                          Code = c.Code,
                          CriterionTitle = c.CriterionTitle,
                          MinimumPoint = 0,
                          MaximumPoint = ct.StartPoint.Value,
                          ParentId = c.ParentId,
                          IsFolder = c.IsFolder,
                          CategoryId = c.CategoryId,
                          CategoryName = c.CategoryName,
                          CriterionLevel = c.CriterionLevel,
                      });
            return qr.ToList();
        }

        public EventDiaryDTO GetEventByDivManager(int yearMonth, int? orgId, string level1ManagerUserName, string empName, string userName)
        {
            var usr = this._userOrgGRepo.SelectWhere(x => x.UserName == userName).FirstOrDefault();
            if (usr != null)
            {
                // danh sach phan quyen ql khoi
                var divMngPers = this._divMngRepo.SelectWhere(x => x.UserId == usr.Id);

                if (yearMonth <= 0)
                {
                    return null;
                }
                level1ManagerUserName = string.IsNullOrWhiteSpace(level1ManagerUserName) ? null : level1ManagerUserName;

                var eventDiaryDTOList = this._eventDiaryGRepo.SelectWhere(x => x.YearMonth == yearMonth && ((orgId == null) || (x.OrgId == orgId))
                    && ((!string.IsNullOrEmpty(level1ManagerUserName) && x.Level1ManagerUserName == level1ManagerUserName)
                        || string.IsNullOrEmpty(level1ManagerUserName)))
                        .Join(divMngPers, p => p.OrgId, d => d.OrgId, (p, d) => p)
                          //.ProjectTo<EventDiaryDTO>()
                          //.FirstOrDefault();
                          .Select(x => new EventDiaryDTO
                          {
                              Id = x.Id,
                              FromDate = x.FromDate,
                              ToDate = x.ToDate
                          }).ToList();
                var eventDiaryDTO = eventDiaryDTOList.FirstOrDefault();
                var eventIdList = eventDiaryDTOList.Select(x => x.Id).ToList();

                if (eventDiaryDTO != null)
                {
                    var diaryDetails = from dt in this._diaryDetailGRepo.SelectWhere(x => x.EventDiaryId != null && eventIdList.Contains(x.EventDiaryId.Value) && ((!string.IsNullOrEmpty(empName) && x.UserName == empName) || string.IsNullOrEmpty(empName)))
                                       join cat in _kpiCriterionCatalogGRepo.SelectAll() on dt.CriterionCatalogId equals cat.Id into gj
                                       from tcat in gj.DefaultIfEmpty()
                                       select new { dt, CriterionCatalogNameEn = tcat.CriterionTitleEn };
                    var usrJobt = from u in _userOrgGRepo.SelectAll()
                                  join j in _jobTitleRepo.SelectAll() on u.JobTitleId equals j.Id
                                  select new { uId = u.Id.ToString(), jobTl = j.TitleEn };
                    var dtls = (from dt in diaryDetails
                                join jt in usrJobt on dt.dt.CreatedByUser equals jt.uId into gj
                                from tcat in gj.DefaultIfEmpty()
                                select new DiaryCriterionDetailDTO()
                                {
                                    Id = dt.dt.Id,
                                    CriterionCatalogId = dt.dt.CriterionCatalogId,
                                    CriterionCatalogCode = dt.dt.CriterionCatalogCode,
                                    CriterionCatalogName = dt.dt.CriterionCatalogName,
                                    CriterionCatalogFolderId = dt.dt.CriterionCatalogFolderId,
                                    UserName = dt.dt.UserName,
                                    UserFullName = dt.dt.UserFullName,
                                    CriterionDate = dt.dt.CriterionDate,
                                    CriterionDayOfMonth = dt.dt.CriterionDayOfMonth,
                                    KpiPoint = dt.dt.KpiPoint,
                                    EventDiaryId = dt.dt.EventDiaryId,
                                    KpiMonthNumber = dt.dt.KpiMonthNumber,
                                    Comment = dt.dt.Comment,
                                    CreatedDate = dt.dt.CreatedDate,
                                    CreatedByUser = dt.dt.CreatedByUser,
                                    CreatedByUserFullName = dt.dt.CreatedByUserFullName,
                                    CreatedByUserTitle = dt.dt.CreatedByUserTitle,
                                    IsDeleted = dt.dt.IsDeleted,
                                    CriterionCatalogNameEn = dt.CriterionCatalogNameEn,
                                    CreatedByUserTitleEn = tcat.jobTl
                                }).ToList();
                    eventDiaryDTO.Details = dtls;
                    eventDiaryDTO.IdList = eventIdList;
                }
                return eventDiaryDTO;
            }
            return new EventDiaryDTO();
        }

        public void CompleteNotify(string userName, int evntId)
        {
            INotificationRepository notifyRepo = new NotificationRepository(this._uow);
            var evnt = this._eventDiaryGRepo.SelectWhere(x => x.Id == evntId).FirstOrDefault();
            if (evnt != null)
            {
                var kpis = this._kpiEvalRepo.SelectWhere(x => x.EventDiaryId == evnt.Id).Select(x => x.UserName).ToList();
                if (kpis.Count > 0)
                {
                    // tao thong bao cho tung nguoi
                    foreach (var item in kpis)
                    {
                        var year = evnt.YearMonth.ToString().Substring(0, 4);
                        var month = evnt.YearMonth.ToString().Substring(4);
                        var notif = new NotificationDTO()
                        {
                            FromUserName = userName,
                            ToUserName = item,
                            Notes = "Quản lý trực tiếp: Hoàn thành đánh giá THCV, kỳ " + month + "/" + year,
                            NotesEn = "Manager: KPI Assertment completed, period " + month + "/" + year,
                            UserDate = DateTime.Now
                        };
                        notifyRepo.Add(notif);
                    }

                    // tao thong bao cho quan ly cap tren
                    var evntCfg = this._eventDiaryConfigGRepo.SelectWhere(x => x.Id == evnt.EventDiaryConfigId).FirstOrDefault();
                    if (evntCfg != null)
                    {
                        var year = evnt.YearMonth.ToString().Substring(0, 4);
                        var month = evnt.YearMonth.ToString().Substring(4);
                        var notif = new NotificationDTO()
                        {
                            FromUserName = userName,
                            ToUserName = evntCfg.Level2ManagerUserName,
                            Notes = "Quản lý trực tiếp: Hoàn thành đánh giá THCV, kỳ " + month + "/" + year,
                            NotesEn = "Manager: KPI Assertment completed, period " + month + "/" + year,
                            UserDate = DateTime.Now
                        };
                        notifyRepo.Add(notif);
                    }
                }
            }
        }
    }
}
