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
using System.Data.Entity;

namespace Kpi.DataAccess.Repo.EFRepos.Kpi
{
    public class KpiDataModel : KpiDTO
    {
        public Kpi_Task Kpi { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
    }

    public interface IKpiEvaluationRepository : IGridRepository<KpiDTO, KpiFilterModelDTO>
    {
        void CreateKpiGlobal(KpiPeriodConfigDTO config);
        IEnumerable<KpiDTO> GetPagingMng(ManagerKpiFilterModelDTO filter);
        int CountMng(ManagerKpiFilterModelDTO filter);
        int CountMngLv2(ManagerKpiFilterModelDTO filter);
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        void ProcessKpiLevel2Mng(KpiDTO processKpi);
        void UpdateStatusUncompletedKpi(UncompeletedKpiFilterModelDTO filter);
        void UpdateKpiHRManagerPropose(KpiDTO kpiDTO);
        void UpdateKpiBussinessApplication(KpiDTO kpiDTO);
        void UpdateKpiLeaderPropose(KpiDTO kpiDTO, string userName);
        void UpdateKpiProcessRange(List<KpiDTO> kpiDTOList, string userName, bool isUnlock = false);
        IEnumerable<KpiDTO> GetPagingMngLv2(ManagerKpiFilterModelDTO filter);
        IEnumerable<Kpi_EventDiary> CreateEventDiaryByConfig(IEnumerable<EventDiaryConfigDTO> ecConfigs, KpiPeriodConfigDTO config);
        void CreateUserKpi(KpiPeriodConfigDTO config, UserInfoModel u, Kpi_EventDiary diary, Kpi_CriterionType criterionType);
        IEnumerable<UserInfoModel> GetUsersInfoByUsername(List<string> usernames, int criterionType);
        int CountDivisionManager(DivisionManagerKpiFilter filter);
        IEnumerable<KpiDTO> GetDivisionManagerKpiPaging(DivisionManagerKpiFilter filter);
        bool GetIsKpiValidForDivManager(int kpiId, string userName);
        IEnumerable<OrgInfoModelDTO> GetOrgsByDivManager(string userName);
        void ProcessRangeKpiLevel2Mng(IEnumerable<KpiDTO> kpiDtos);
        void UpdateRangeKpiBusinessApplication(IEnumerable<KpiDTO> kpiDTOs);
        void UpdateRangeKpiHRManagerPropose(IEnumerable<KpiDTO> kpiDTOs);
        IEnumerable<KpiDTO> GetHrManagerKpiPaging(DivisionManagerKpiFilter filter);
        int CountHrManagerKpi(DivisionManagerKpiFilter filter);
        TaskFilterRespondDTO FilterNoTasksKpi(IEnumerable<KpiDTO> kpiDtos);
    }

    public class KpiEvaluationRepository : BaseGridRepository<Kpi_KpiEvaluation, KpiDTO, KpiFilterModelDTO>, IKpiEvaluationRepository
    {
        #region property

        private readonly IGenericRepository<Kpi_KpiEvaluation> _kpiEvaluationGRepo;
        private readonly IGenericRepository<Kpi_Task> _kpiTaskGRepo;
        private readonly IGenericRepository<Kpi_KpiCriterionDetail> _kpiCriterionDetailGRepo;
        private readonly IGenericRepository<Kpi_CriterionCatalog> _kpiCriterionCatalogGRepo;
        private readonly IGenericRepository<Kpi_CriterionTypeCatalog> _kpiCriterionTypeCatalogGRepo;
        private readonly IGenericRepository<View_KpiPointEvaluation> _viewKpiPointEvaluationGRepo;
        private readonly IGenericRepository<AspNetUser> _userGRepo;
        private readonly IGenericRepository<Org_UserOrg> _userOrgGRepo;
        private readonly IGenericRepository<Kpi_JobTitleCriterionType> _jobCriterionGRepo;
        private readonly IGenericRepository<Org_Organization> _orgGRepo;
        private readonly IGenericRepository<Kpi_EventDiary> _eventDiaryGRepo;
        private readonly IGenericRepository<Kpi_CriterionType> _criterionTypeGRepo;
        private readonly IGenericRepository<Sys_EventDiaryConfig> _eventDiaryConfigGRepo;

        private readonly IGenericRepository<Process_BussinessApplication> _bussinessApplicationGRepo;
        private readonly IGenericRepository<Process_ProcessStatus> _processStatusGRepo;
        private readonly IGenericRepository<Process_ProcessTransition> _processTransitionGRepo;
        private readonly IBusinessApplicationRepository _businessApplicationRepo;
        private readonly IGenericRepository<Sys_KpiPeriodConfig> _kpiPeriodGRepo;
        private readonly IGenericRepository<DivMngPer> _divMngRepo;
        private readonly IGenericRepository<Org_JobTitle> _jobTitleRepo;
        private readonly IGenericRepository<Notification_Queue> _notiQueueRepo;
        private readonly IGenericRepository<Kpi_Task_File> _taskFileGRepo;

        #endregion

        #region CRUD

        public KpiEvaluationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _kpiEvaluationGRepo = _unitOfWork.GetDataRepository<Kpi_KpiEvaluation>();
            _kpiTaskGRepo = _unitOfWork.GetDataRepository<Kpi_Task>();
            _kpiCriterionDetailGRepo = _unitOfWork.GetDataRepository<Kpi_KpiCriterionDetail>();
            _kpiCriterionCatalogGRepo = _unitOfWork.GetDataRepository<Kpi_CriterionCatalog>();
            _kpiCriterionTypeCatalogGRepo = _unitOfWork.GetDataRepository<Kpi_CriterionTypeCatalog>();

            _viewKpiPointEvaluationGRepo = _unitOfWork.GetDataRepository<View_KpiPointEvaluation>();
            _userGRepo = _unitOfWork.GetDataRepository<AspNetUser>();
            _userOrgGRepo = _unitOfWork.GetDataRepository<Org_UserOrg>();
            _orgGRepo = _unitOfWork.GetDataRepository<Org_Organization>();
            _eventDiaryGRepo = _unitOfWork.GetDataRepository<Kpi_EventDiary>();
            _jobCriterionGRepo = _unitOfWork.GetDataRepository<Kpi_JobTitleCriterionType>();
            _criterionTypeGRepo = _unitOfWork.GetDataRepository<Kpi_CriterionType>();
            _eventDiaryConfigGRepo = _unitOfWork.GetDataRepository<Sys_EventDiaryConfig>();

            _bussinessApplicationGRepo = _unitOfWork.GetDataRepository<Process_BussinessApplication>();
            _processStatusGRepo = _unitOfWork.GetDataRepository<Process_ProcessStatus>();
            _processTransitionGRepo = _unitOfWork.GetDataRepository<Process_ProcessTransition>();
            this._businessApplicationRepo = new BusinessApplicationRepository(this._unitOfWork);
            this._kpiPeriodGRepo = this._unitOfWork.GetDataRepository<Sys_KpiPeriodConfig>();
            _divMngRepo = _unitOfWork.GetDataRepository<DivMngPer>();
            _jobTitleRepo = _unitOfWork.GetDataRepository<Org_JobTitle>();
            _notiQueueRepo = _unitOfWork.GetDataRepository<Notification_Queue>();
            _taskFileGRepo = _unitOfWork.GetDataRepository<Kpi_Task_File>();
        }

        public override KpiDTO Add(KpiDTO kpiDTo)
        {
            //var kpiEntity = AutoMapperHelper.Map<KpiDTO, Kpi_KpiEvaluation>(kpiDTo);

            //using (var transaction = _unitOfWork.BeginTransaction())
            //{
            //    // add kpi evaluation
            //    kpiEntity = _repo.Add(kpiEntity);
            //    _unitOfWork.SaveChanges();

            //    List<TaskDTO> taskList = kpiDTo.TaskList;

            //    // add task list
            //    if (taskList != null && taskList.Any())
            //    {
            //        foreach (var item in taskList)
            //        {
            //            item.KpiEvaluationId = kpiEntity.Id;
            //        }
            //        var task = AutoMapperHelper.Map<TaskDTO, Kpi_Task, List<TaskDTO>, List<Kpi_Task>>(taskList);
            //        _kpiTaskGRepo.AddRange(task);

            //        _unitOfWork.SaveChanges();
            //    }

            //    // add criterion list
            //    List<KpiCriterionDetailDTO> kpiCriterionDetailList = kpiDTo.KpiCriterionDetailList;
            //    if (kpiCriterionDetailList != null && kpiCriterionDetailList.Any())
            //    {
            //        foreach (var item in kpiCriterionDetailList)
            //        {
            //            item.KpiEvaluateId = kpiEntity.Id;
            //        }
            //        var criterionDetail = AutoMapperHelper.Map<KpiCriterionDetailDTO, Kpi_KpiCriterionDetail, List<KpiCriterionDetailDTO>, List<Kpi_KpiCriterionDetail>>(kpiCriterionDetailList);
            //        _kpiCriterionDetailGRepo.AddRange(criterionDetail);
            //    }

            //    _unitOfWork.SaveChanges();

            //    //Anhnt add
            //    this.AddBussinessApplication(kpiDTo);

            //    transaction.Commit();
            //}

            //return AutoMapperHelper.Map<Kpi_KpiEvaluation, KpiDTO>(kpiEntity);
            throw new NotImplementedException();
        }

        public override void Update(KpiDTO entityDto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                entityDto.KpiCriterionDetailList = entityDto.KpiCriterionDetailList.Where(x => x.Id > 0).ToList();

                var kpiEntity = this.UpdateKpiEvaluation(entityDto);

                // gan id cua phieu danh gia
                foreach (TaskDTO item in entityDto.TaskList)
                {
                    item.KpiEvaluationId = entityDto.Id;
                }
                this.UpdateKpiTask(entityDto);

                this.UpdateKpiCriterionDetail(entityDto.KpiCriterionDetailList);

                //Anhnt add
                //huypq modified 23/11/19
                this._businessApplicationRepo.AddBussinessApplication(entityDto, kpiEntity);

                transaction.Commit();
            }
        }

        public override KpiDTO Get(int id)
        {
            Kpi_KpiEvaluation kpiEvaluation = _kpiEvaluationGRepo.Get(x => x.Id == id);
            var docEntity = AutoMapperHelper.Map<Kpi_KpiEvaluation, KpiDTO>(kpiEvaluation);

            docEntity.TaskList = (from t in this._kpiTaskGRepo.SelectWhere(x => x.KpiEvaluationId == id)
                                  join tf in this._taskFileGRepo.SelectAll() on t.Id equals tf.TaskId into tfdx
                                  from tfx in tfdx.DefaultIfEmpty()
                                  select new KpiDataModel
                                  {
                                      Kpi = t,
                                      FileName = (tfx != null) ? tfx.FileName : string.Empty,
                                      FileId = (tfx != null) ? tfx.FileId : 0,
                                  }).ToList()
                           .GroupBy(x => x.Kpi)
                           .Select(g =>
                           {
                               var kpiDTO = AutoMapperHelper.Map<Kpi_Task, TaskDTO>(g.Key);
                               kpiDTO.Files = g.Where(x => x.FileId > 0).Select(x => new FileDTO { Id = x.FileId, FileName = x.FileName }).ToList();
                               return kpiDTO;
                           }).ToList();

            // trang thai tieng anh
            var status = this._processStatusGRepo.SelectWhere(x => x.Id == docEntity.StatusId).FirstOrDefault();
            if (status != null)
            {
                docEntity.StatusNameEn = status.TitleEn;
            }

            var details = this._kpiCriterionDetailGRepo.SelectWhere(x => x.KpiEvaluateId == id);
            var critCatalog = this._kpiCriterionCatalogGRepo.SelectAll();
            var dtl = from dt in details
                      join cat in critCatalog on dt.KpiCatalogId equals cat.Id into gj
                      from tcat in gj.DefaultIfEmpty()
                      select new KpiCriterionDetailDTO
                      {
                          CreatedByUser = dt.CreatedByUser,
                          CreatedDate = dt.CreatedDate,
                          CriterionIndex = dt.CriterionIndex,
                          CriterionTitle = dt.CriterionTitle,
                          CriterionTitleEn = tcat.CriterionTitleEn,
                          EmployeeEvaluateComment = dt.EmployeeEvaluateComment,
                          EmployeeEvaluatePoint = dt.EmployeeEvaluatePoint,
                          EmployeeLastUpdatedDate = dt.EmployeeLastUpdatedDate,
                          Id = dt.Id,
                          IsDeleted = dt.IsDeleted,
                          KpiCatalogId = dt.KpiCatalogId,
                          KpiEvaluateId = dt.KpiEvaluateId,
                          ManagerEvaluateComment = dt.ManagerEvaluateComment,
                          ManagerEvaluatePoint = dt.ManagerEvaluatePoint,
                          ManagerLastUpdatedDate = dt.ManagerLastUpdatedDate,
                          MaximumPoint = dt.MaximumPoint,
                          MinimumPoint = dt.MinimumPoint,
                          Month = dt.Month,
                          OrgId = dt.OrgId,
                          UserId = dt.UserId,
                          Year = dt.Year
                      };
            docEntity.KpiCriterionDetailList = dtl.ToList();
            return docEntity;
        }

        private Kpi_KpiEvaluation UpdateKpiEvaluation(KpiEvaluationDTO entityDto)
        {
            var properties = new List<Expression<Func<Kpi_KpiEvaluation, object>>>
            {
                (x => x.EmpKpiPoint),
                (x => x.EmpKpiClassification),
                (x => x.HrKpiPoint),
                (x => x.HrKpiPointClassification),
                (x => x.HrKpiPointComment),
                (x => x.FinalKpiComment),
                (x => x.UpdatedDate),
                (x => x.UpdatedByUser)
            };
            var kpiEntity = new Kpi_KpiEvaluation
            {
                Id = entityDto.Id,
                EmpKpiPoint = entityDto.EmpKpiPoint,
                EmpKpiClassification = entityDto.EmpKpiClassification,
                HrKpiPoint = entityDto.HrKpiPoint,
                HrKpiPointClassification = entityDto.HrKpiPointClassification,
                HrKpiPointComment = entityDto.HrKpiPointComment,
                FinalKpiComment = entityDto.FinalKpiComment,
                UpdatedByUser = entityDto.UpdatedByUser,
                UpdatedDate = entityDto.UpdatedDate,
            };
            this._kpiEvaluationGRepo.UpdateByProperties(kpiEntity, properties);
            this._unitOfWork.SaveChanges();
            return kpiEntity;
        }

        /// <summary>
        /// Delete task, update task, add task
        /// </summary>
        private void UpdateKpiTask(KpiDTO entityDto)
        {
            var tasks = entityDto.TaskList;

            if (tasks == null || !tasks.Any())
            {
                return;
            }

            var deletedTask = tasks.Where(x => x.IsUIDeleted).Select(x => x.Id).ToList();
            if (deletedTask != null && deletedTask.Any())
            {
                this._kpiTaskGRepo.Delete(x => deletedTask.Contains(x.Id));
                _unitOfWork.SaveChanges();
            }

            var addedTask = tasks.Where(x => x.Id == 0).ToList();
            var updatedTask = tasks.Where(x => x.Id > 0 && !x.IsUIDeleted).ToList();
            List<TaskDTO> modifiedTask = /*new List<TaskDTO>()*/ null;
            if (addedTask != null && addedTask.Any())
            {
                //var addedTaskEntities = AutoMapperHelper.Map<TaskDTO, Kpi_Task, IEnumerable<TaskDTO>, IEnumerable<Kpi_Task>>(addedTask).ToList();
                //addedTaskEntities = this._kpiTaskGRepo.AddRange(addedTaskEntities).ToList();
                //_unitOfWork.SaveChanges();
                foreach (var item in addedTask)
                {
                    var tskEntity = AutoMapperHelper.Map<TaskDTO, Kpi_Task>(item);
                    tskEntity = this._kpiTaskGRepo.Add(tskEntity);
                    this._unitOfWork.SaveChanges();
                    item.Id = tskEntity.Id;
                }
                modifiedTask = addedTask;
            }
            else
            {
                modifiedTask = new List<TaskDTO>();
            }

            if (updatedTask != null && updatedTask.Any())
            {
                var updatedTaskEntities = AutoMapperHelper.Map<TaskDTO, Kpi_Task, IEnumerable<TaskDTO>, IEnumerable<Kpi_Task>>(updatedTask);
                foreach (var t in updatedTaskEntities)
                {
                    t.KpiEvaluationId = entityDto.Id;
                    this._kpiTaskGRepo.Update(t);
                }
                _unitOfWork.SaveChanges();
                modifiedTask.AddRange(updatedTask);
            }

            this.UpdateTaskFile(modifiedTask);
        }

        /// <summary>
        /// Add file for task
        /// </summary>
        private void UpdateTaskFile(List<TaskDTO> taskEntities)
        {
            if (!taskEntities.Any())
            {
                return;
            }

            // task file sẽ add thêm vào
            var addTaskFile = new List<Kpi_Task_File>();
            var deleteTaskFile = new List<Kpi_Task_File>();
            for (int i = 0; i < taskEntities.Count; i++)
            {
                var task = taskEntities[i];
                var files = task.Files;
                if (!files.Any())
                {
                    continue;
                }
                var recentlyAddedFiles = files.Where(x => x.IsRecentlyCreated);
                if (recentlyAddedFiles.Any())
                {
                    var kpiAddedTaskFiles = recentlyAddedFiles.Select(x => new Kpi_Task_File { TaskId = task.Id, FileId = x.Id, FileName = x.FileName });
                    addTaskFile.AddRange(kpiAddedTaskFiles);
                }

                var recentlyDeletedFiles = files.Where(x => x.IsRecentlyDeleted);
                if (recentlyDeletedFiles.Any())
                {
                    var kpiDeletedTaskFiles = recentlyDeletedFiles.Select(x => new Kpi_Task_File { TaskId = task.Id, FileId = x.Id, FileName = x.FileName });
                    deleteTaskFile.AddRange(kpiDeletedTaskFiles);
                }
            }
            this._taskFileGRepo.AddRange(addTaskFile);
            this._unitOfWork.SaveChanges();
            var deletedTaskIds = deleteTaskFile.Select(x => x.FileId).ToList();
            this._taskFileGRepo.Delete(x => deletedTaskIds.Contains(x.FileId));
            this._unitOfWork.SaveChanges();
        }

        private void UpdateKpiCriterionDetail(List<KpiCriterionDetailDTO> details)
        {
            if (details == null || !details.Any())
            {
                return;
            }

            var detailEntities = AutoMapperHelper.Map<KpiCriterionDetailDTO, Kpi_KpiCriterionDetail, List<KpiCriterionDetailDTO>, List<Kpi_KpiCriterionDetail>>(details);
            foreach (var d in detailEntities)
            {
                this._kpiCriterionDetailGRepo.Update(d);
            }
            _unitOfWork.SaveChanges();
        }

        private Process_BussinessApplication AddBussinessApplication(KpiDTO kpiDTo)
        {
            Kpi_KpiEvaluation kpiEvaluation = _kpiEvaluationGRepo.Get(x => x.Id == kpiDTo.Id);
            Process_ProcessTransition processTransitionDTO = _processTransitionGRepo.Get(x => x.SourceProcessID == kpiDTo.StatusId && x.ActionName == kpiDTo.ActionName);
            if (processTransitionDTO == null) return null;

            kpiEvaluation.StatusId = processTransitionDTO.TargetProcessID;
            kpiEvaluation.StatusName = (_processStatusGRepo.Get(x => x.Id == processTransitionDTO.TargetProcessID)).Title;

            var properties = new List<Expression<Func<Kpi_KpiEvaluation, object>>>
            {
                (x => x.StatusId),
                (x => x.StatusName),
            };
            var kpiEntity = new Kpi_KpiEvaluation
            {
                Id = kpiDTo.Id,
                StatusId = kpiEvaluation.StatusId,
                StatusName = kpiEvaluation.StatusName
            };
            this._kpiEvaluationGRepo.UpdateByProperties(kpiEntity, properties);

            Process_BussinessApplication business_BussinessApplication = new Process_BussinessApplication
            {
                AfterStatus = processTransitionDTO.TargetProcessID,
                BeforeStatus = processTransitionDTO.SourceProcessID,
                BusinessTransitionID = kpiEvaluation.Id,
                HandleByUserTitleName = kpiDTo.EmployeeJobTitle,
                HandledByOrganization = kpiDTo.OrganizationId,
                HandledByUser = kpiDTo.EmployeeName,
                ObjectTypeCode = "Kpi_KpiEvaluation",
                HandledByUserTitle = kpiDTo.EmployeeJobId,
                HandledDate = kpiDTo.CreatedDate,
                RegencyTitle = processTransitionDTO.ActionName,
                ActionID = kpiDTo.ActionId + "",
                Note = kpiEvaluation.CriterionTypeName,
                ObjectID = kpiEvaluation.Id,
                OrganizationName = kpiDTo.Organization
            };

            business_BussinessApplication = _bussinessApplicationGRepo.Add(business_BussinessApplication);
            _unitOfWork.SaveChanges();
            return business_BussinessApplication;
        }
        #endregion

        #region Paging

        public override IEnumerable<KpiDTO> GetPaging(KpiFilterModelDTO pagingModel)
        {
            // HuyPQ modified 20/11/2019, fix hàm count không được filter
            // push tất cả query vào hàm filter, chạy chung cho cả GetPaging và Count
            IQueryable<Kpi_KpiEvaluation> query = this._repo.SelectWhere(x => x.EventDiaryId != null && !x.IsDeleted);
            query = this.Filter(query, pagingModel);

            //<<<<<<< HEAD
            //            var ret = query.OrderBy(x => x.EmployeeName).Skip(pagingModel.Start - 1).Take(pagingModel.Length)
            //             .Select(x => new KpiDTO
            //             {
            //                 Id = x.Id,
            //                 KpiMonth = x.KpiMonth,
            //                 EmployeeName = x.EmployeeName,
            //                 Level1ManagerFullName = x.Level1ManagerFullName,
            //                 Level2ManagerFullName = x.Level2ManagerFullName,
            //                 StatusName = x.StatusName,
            //                 StatusId = x.StatusId,
            //                 Organization = x.Organization,
            //                 OrganizationId = x.OrganizationId,
            //                 Level1ManagerKpiPoint = x.Level1ManagerKpiPoint,
            //                 Level1ManagerKpiClassification = x.Level1ManagerKpiClassification,
            //                 HrKpiPoint = x.HrKpiPoint,
            //                 HrKpiPointClassification = x.HrKpiPointClassification,
            //                 FinalKpiPoint = x.FinalKpiPoint,
            //                 FinalKpiClassification = x.FinalKpiClassification,
            //                 StatusNameEn = x.StatusNameEn,
            //                 YearMonth = x.YearMonth
            //             }).ToList();

            //            return ret;
            //=======
            return GetPagingResult(pagingModel, query);
            //>>>>>>> develop_kpi
        }

        public override int Count(KpiFilterModelDTO pagingModel)
        {
            // HuyPQ modified 20/11/2019, fix hàm count không được filter
            // push tất cả query vào hàm filter, chạy chung cho cả GetPaging và Count
            IQueryable<Kpi_KpiEvaluation> query = this._repo.SelectWhere(x => x.EventDiaryId != null && !x.IsDeleted);
            query = this.Filter(query, pagingModel);
            return query.Count();
        }

        private IEnumerable<KpiDTO> GetPagingResult(BaseGridFilterModelDTO pagingModel, IQueryable<Kpi_KpiEvaluation> query)
        {
            // lay cot trang thai tieng anh
            var q = from kpi in query
                    join stat in this._processStatusGRepo.SelectAll() on kpi.StatusId equals stat.Id into gj
                    from gr in gj.DefaultIfEmpty()
                    select new { kpi, TitleEn = gr.TitleEn };

            var ret = q.OrderBy(x => x.kpi.EmployeeName).Skip(pagingModel.Start - 1).Take(pagingModel.Length)
             .Select(x => new KpiDTO
             {
                 Id = x.kpi.Id,
                 KpiMonth = x.kpi.KpiMonth,
                 EmployeeName = x.kpi.EmployeeName,
                 Level1ManagerFullName = x.kpi.Level1ManagerFullName,
                 Level2ManagerFullName = x.kpi.Level2ManagerFullName,
                 StatusName = x.kpi.StatusName,
                 StatusId = x.kpi.StatusId,
                 Organization = x.kpi.Organization,
                 OrganizationId = x.kpi.OrganizationId,
                 Level1ManagerKpiPoint = x.kpi.Level1ManagerKpiPoint,
                 Level1ManagerKpiClassification = x.kpi.Level1ManagerKpiClassification,
                 HrKpiPoint = x.kpi.HrKpiPoint,
                 HrKpiPointClassification = x.kpi.HrKpiPointClassification,
                 FinalKpiPoint = x.kpi.FinalKpiPoint,
                 FinalKpiClassification = x.kpi.FinalKpiClassification,
                 StatusNameEn = x.TitleEn,
                 YearMonth = x.kpi.YearMonth,

             }).ToList();

            return ret;
        }
        #endregion

        #region mng

        /// <summary>
        /// Get paging by manager
        /// </summary>
        /// huypq modified 6/12/19, 1 quản lý cấp 1 có thể có nhiều EventDiaryConfig, vì vậy cần viết lại query
        public IEnumerable<KpiDTO> GetPagingMng(ManagerKpiFilterModelDTO filter)
        {
            var query = this._repo.SelectWhere(x => x.Level1ManagerUserName == filter.Level1ManagerUserName && x.EventDiaryId != null && !x.IsDeleted);
            query = this.FilterMng(query, filter);

            return GetPagingResult(filter, query);
        }

        /// <summary>
        /// Get Paging cho quản lý cấp 1
        /// huypq modified 6/12/19, 1 quản lý cấp 1 có thể có nhiều EventDiaryConfig, vì vậy cần viết lại query
        public int CountMng(ManagerKpiFilterModelDTO filter)
        {
            //var eventDiaryCfId = this._eventDiaryConfigGRepo.SelectWhere(x => x.Level1ManagerUserName == filter.Level1ManagerUserName && x.OrgId == filter.OrgId).Select(x => x.Id).FirstOrDefault();

            // get kpi belong to this config  
            var query = this._repo.SelectWhere(x => x.Level1ManagerUserName == filter.Level1ManagerUserName && x.EventDiaryId != null && !x.IsDeleted);
            query = this.FilterMng(query, filter);
            return query.Count();
        }

        /// <summary>
        /// Get Paging cho quản lý cấp 2
        /// huypq modified 6/12/19, 1 quản lý cấp 2 có thể có nhiều EventDiaryConfig, vì vậy cần viết lại query
        /// </summary>
        public IEnumerable<KpiDTO> GetPagingMngLv2(ManagerKpiFilterModelDTO filter)
        {
            var query = this._repo.SelectWhere(x => x.Level2ManagerUserName == filter.Level2ManagerUserName && x.EventDiaryId != null && !x.IsDeleted);
            query = this.FilterMng(query, filter);

            return GetPagingResult(filter, query);
        }

        /// huypq modified 6/12/19, 1 quản lý cấp 1 có thể có nhiều EventDiaryConfig, vì vậy cần viết lại query
        public int CountMngLv2(ManagerKpiFilterModelDTO filter)
        {
            var query = this._repo.SelectWhere(x => x.Level2ManagerUserName == filter.Level2ManagerUserName && x.EventDiaryId != null && !x.IsDeleted);
            query = this.FilterMng(query, filter);
            return query.Count();
        }

        /// <summary>
        /// Get paging by manager
        /// </summary>
        public IEnumerable<OrgInfoModelDTO> GetOrgs()
        {
            // get kpi belong to this config  
            var query = this._orgGRepo.SelectAll();
            var ret =
             query.OrderBy(x => x.Id).Select(x => new OrgInfoModelDTO
             {
                 Id = x.Id,
                 Name = x.Name,
                 Description = x.Description,
                 OrganizationTypeID = x.OrganizationTypeID,
                 NodeID = x.NodeID,
                 DirectoryPath = x.DirectoryPath
             }).ToList();
            return ret;
        }

        /// <summary>
        /// Chuyển trạng thái
        /// </summary>
        public void ProcessKpiLevel2Mng(KpiDTO kpiDto)
        {
            var yearMonth = kpiDto.KpiMonth.GetYearMonth();
            var daySendEvalation = this._kpiPeriodGRepo.SelectWhere(x => x.YearMonth == yearMonth).Select(x => x.DaySendEvalation).FirstOrDefault();
            if (daySendEvalation == null)
            {
                daySendEvalation = 28;
            }
            var daySendEvalationDatetime = new DateTime(kpiDto.KpiMonth.Year, kpiDto.KpiMonth.Month, daySendEvalation.GetValueOrDefault());

            var kpiEntity = new Kpi_KpiEvaluation
            {
                Id = kpiDto.Id,
                UpdatedByUser = kpiDto.UpdatedByUser,
                UpdatedDate = kpiDto.UpdatedDate,
                SentHRDate = kpiDto.SentHRDate,
                SubmitNote = daySendEvalationDatetime.EndOfDay() < DateTime.Now ? "Nộp muộn" : string.Empty,
            };

            var rs = this._businessApplicationRepo.AddBussinessApplication(kpiDto, kpiEntity);

            if (rs == null)
            {
                return;
            }
            this._repo.UpdateByProperties(kpiEntity, new List<Expression<Func<Kpi_KpiEvaluation, object>>>() { x => x.UpdatedByUser, x => x.UpdatedDate, x => x.SentHRDate, x => x.SubmitNote });
            this._unitOfWork.SaveChanges();

            // update detail
            var details = this._kpiCriterionDetailGRepo.SelectWhere(x => x.KpiEvaluateId == kpiDto.Id).ToList();
            foreach (var kDetail in details)
            {
                kDetail.ManagerLastUpdatedDate = DateTime.Now;
                this._kpiCriterionDetailGRepo.Update(kDetail);
            }
            this._unitOfWork.SaveChanges();
        }

        public void ProcessRangeKpiLevel2Mng(IEnumerable<KpiDTO> kpiDtos)
        {
            using (var ctx = this._unitOfWork.BeginTransaction())
            {
                foreach (var kpi in kpiDtos)
                {
                    this.ProcessKpiLevel2Mng(kpi);
                }
                ctx.Commit();
            }
        }

        public TaskFilterRespondDTO FilterNoTasksKpi(IEnumerable<KpiDTO> kpiDtos)
        {
            var taskTbl = this._kpiTaskGRepo.SelectAll();
            var kpiIdList = kpiDtos.Select(x => x.Id).ToList();

            var rs = new TaskFilterRespondDTO();

            // lấy những kpi ko tồn tại task hoặc có task nhưng ko có result
            var noTaskKpiList = this._kpiEvaluationGRepo.SelectWhere(k => kpiIdList.Contains(k.Id)
                    && (!taskTbl.Any(t => t.KpiEvaluationId == k.Id) || taskTbl.Any(t => t.KpiEvaluationId == k.Id && (t.Result == null || t.Result == ""))))
                        .ProjectTo<KpiDTO>().ToList();
            rs.NoTaskKpiList = noTaskKpiList;
            //var tasksTest = this._kpiTaskGRepo.SelectWhere(t => kpiIdList.Contains(t.KpiEvaluationId) && (t.Result == null || t.Result == ""));
            rs.TaskKpiList = kpiDtos.Where(x => !rs.NoTaskKpiList.Any(y => y.Id == x.Id));
            return rs;
        }

        private IQueryable<Kpi_KpiEvaluation> FilterMng(IQueryable<Kpi_KpiEvaluation> query, ManagerKpiFilterModelDTO filter)
        {
            var result = query;
            if (filter.YearMonth > 0)
            {
                result = result.Where(x => x.YearMonth == filter.YearMonth);
            }

            if (filter.OrgId > 0)
            {
                result = result.Where(x => x.OrganizationId == filter.OrgId);
            }

            if (!String.IsNullOrEmpty(filter.empName))
            {
                result = result.Where(x => x.UserName == filter.empName);
            }

            if (filter.StatusIds != null && filter.StatusIds.Count > 0)
            {
                result = result.Where(x => filter.StatusIds.Contains(x.StatusId));
            }
            return result;
        }

        public int CountDivisionManager(DivisionManagerKpiFilter filter)
        {
            var query = this._repo.SelectWhere(x => x.EventDiaryId != null && !x.IsDeleted);
            query = this.FilterDivisionMng(query, filter);
            return query.Count();
        }

        public IEnumerable<KpiDTO> GetDivisionManagerKpiPaging(DivisionManagerKpiFilter filter)
        {
            var query = this._repo.SelectWhere(x => x.EventDiaryId != null && !x.IsDeleted);
            query = this.FilterDivisionMng(query, filter);

            return GetPagingResult(filter, query);
        }

        private IQueryable<Kpi_KpiEvaluation> FilterDivisionMng(IQueryable<Kpi_KpiEvaluation> query, DivisionManagerKpiFilter filter)
        {
            var result = query;

            if (filter.OrgId > 0)
            {
                result = result.Where(x => x.OrganizationId == filter.OrgId);

            }

            if (filter.YearMonth > 0)
            {
                result = result.Where(x => x.YearMonth == filter.YearMonth);
            }

            if (!String.IsNullOrEmpty(filter.empName))
            {
                result = result.Where(x => x.UserName == filter.empName);
            }

            if (!String.IsNullOrEmpty(filter.Level1ManagerUserName))
            {
                result = result.Where(x => x.Level1ManagerUserName == filter.Level1ManagerUserName);
            }

            if (filter.StatusIds != null && filter.StatusIds.Count > 0)
            {
                result = result.Where(x => filter.StatusIds.Contains(x.StatusId));
            }

            // danh sach phong ban user nay duoc xem du lieu
            var user = this._userOrgGRepo.SelectWhere(x => x.UserName == filter.UserName).FirstOrDefault();
            if (user != null)
            {
                var orgs = this._divMngRepo.SelectWhere(x => x.UserId == user.Id);
                result = result.Where(x => orgs.Any(t => t.OrgId == x.OrganizationId));
            }
            else
            {
                // tra ve filter false
                result = result.Where(x => 0 == 1);
            }

            return result;
        }
        #endregion

        #region Hr

        private IQueryable<Kpi_KpiEvaluation> FilterUncompletedKpi(IQueryable<Kpi_KpiEvaluation> query, UncompeletedKpiFilterModelDTO filter)
        {
            var rs = query;
            if (filter.YearMonth > 0)
            {
                rs = rs.Where(x => x.YearMonth == filter.YearMonth);
            }
            else
            {
                var currentYM = DateTime.Now.GetYearMonth();
                rs = rs.Where(x => x.YearMonth == currentYM);
            }

            var uncompletedStatus = new List<int> { 1, 2 };
            rs = rs.Where(x => uncompletedStatus.Contains(x.StatusId));

            if (filter.OrgIds != null && filter.OrgIds.Any())
            {
                rs = rs.Where(x => filter.OrgIds.Contains(x.OrganizationId));
            }
            return rs;
        }

        /// <summary>
        /// Lấy dữ liệu 
        /// </summary>
        public void UpdateStatusUncompletedKpi(UncompeletedKpiFilterModelDTO filter)
        {
            // get kpi
            // huypq modified 11/12/19 bổ sung query theo yearmonth và orgId truyền từ client
            var uncompletedKpi = this.FilterUncompletedKpi(this._kpiEvaluationGRepo.SelectAll(), filter)
                .Select(x => new KpiDTO
                {
                    Id = x.Id,
                    StatusId = x.StatusId,
                    ActionId = filter.ActionId,
                    EmployeeJobTitle = x.EmployeeJobTitle,
                    OrganizationId = x.OrganizationId,
                    EmployeeName = x.EmployeeName,
                    EmployeeJobId = x.EmployeeJobId,
                    CreatedDate = x.CreatedDate,
                    Organization = x.Organization
                }).ToArray();

            if (uncompletedKpi == null || !uncompletedKpi.Any())
            {
                throw new ObjectNotFoundException("Không tìm thấy kpi để lấy dữ liệu");
            }

            var properties = new List<Expression<Func<Kpi_KpiEvaluation, object>>>() { x => x.UpdatedByUser, x => x.UpdatedDate, x => x.SubmitNote, x => x.SentHRDate };
            // update
            using (var ts = this._unitOfWork.BeginTransaction())
            {
                foreach (var kpiDTO in uncompletedKpi)
                {
                    kpiDTO.UpdatedDate = DateTime.Now;
                    kpiDTO.UpdatedByUser = filter.Username;
                    kpiDTO.SentHRDate = DateTime.Now;

                    var kpiEntity = new Kpi_KpiEvaluation
                    {
                        Id = kpiDTO.Id,
                        UpdatedByUser = kpiDTO.UpdatedByUser,
                        UpdatedDate = kpiDTO.UpdatedDate,
                        SentHRDate = kpiDTO.SentHRDate,
                        SubmitNote = "Nộp muộn",
                    };
                    var ba = this._businessApplicationRepo.AddBussinessApplication(kpiDTO, kpiEntity);
                    if (ba != null)
                    {
                        this._repo.UpdateByProperties(kpiEntity, properties);
                    }
                    this._unitOfWork.SaveChanges();
                }
                ts.Commit();
            }
        }

        /// <summary>
        /// Chỉ update status và business application, không thay đổi trường gì khác
        /// </summary>
        public void UpdateKpiBussinessApplication(KpiDTO kpiDTO)
        {
            var kpiEntity = new Kpi_KpiEvaluation
            {
                Id = kpiDTO.Id
            };
            this._businessApplicationRepo.AddBussinessApplication(kpiDTO, kpiEntity);
        }

        public void UpdateRangeKpiBusinessApplication(IEnumerable<KpiDTO> kpiDTOs)
        {
            using (var ctx = this._unitOfWork.BeginTransaction())
            {
                foreach (var kpiDTO in kpiDTOs)
                {
                    this.UpdateKpiBussinessApplication(kpiDTO);
                }
                ctx.Commit();
            }
        }

        /// <summary>
        /// Update kpi hr manager comment + business application
        /// </summary>
        public void UpdateKpiHRManagerPropose(KpiDTO kpiDTO)
        {
            using (var ts = this._unitOfWork.BeginTransaction())
            {
                var kpiEntity = new Kpi_KpiEvaluation
                {
                    Id = kpiDTO.Id,
                    HrKpiPoint = kpiDTO.HrKpiPoint,
                    HrKpiPointClassification = kpiDTO.HrKpiPointClassification,
                    HrKpiPointComment = kpiDTO.HrKpiPointComment,
                    UpdatedByUser = kpiDTO.UpdatedByUser,
                    UpdatedDate = kpiDTO.UpdatedDate
                };

                var properties = new List<Expression<Func<Kpi_KpiEvaluation, object>>>
                {(x => x.HrKpiPoint),
                 (x => x.HrKpiPointClassification),
                 (x => x.HrKpiPointComment),
                 (x => x.UpdatedByUser),
                 (x => x.UpdatedDate),
                };
                this._kpiEvaluationGRepo.UpdateByProperties(kpiEntity, properties);
                this._unitOfWork.SaveChanges();
                this._businessApplicationRepo.AddBussinessApplication(kpiDTO, kpiEntity);

                ts.Commit();
            }
        }

        public void UpdateRangeKpiHRManagerPropose(IEnumerable<KpiDTO> kpiDTOs)
        {
            foreach (var kpi in kpiDTOs)
            {
                this.UpdateKpiHRManagerPropose(kpi);
            }
        }

        /// <summary>
        /// Update kpi leader comment + bussiness application
        /// </summary>
        public void UpdateKpiLeaderPropose(KpiDTO kpiDTO, string userName)
        {
            using (var ts = this._unitOfWork.BeginTransaction())
            {
                var kpiEntity = new Kpi_KpiEvaluation
                {
                    Id = kpiDTO.Id,
                    FinalKpiPoint = kpiDTO.FinalKpiPoint,
                    FinalKpiClassification = kpiDTO.FinalKpiClassification,
                    FinalKpiComment = kpiDTO.FinalKpiComment,
                    UpdatedByUser = kpiDTO.UpdatedByUser,
                    UpdatedDate = kpiDTO.UpdatedDate
                };

                var properties = new List<Expression<Func<Kpi_KpiEvaluation, object>>>
                {(x => x.FinalKpiPoint),
                 (x => x.FinalKpiClassification),
                 (x => x.FinalKpiComment),
                 (x => x.UpdatedByUser),
                 (x => x.UpdatedDate),
                };
                this._kpiEvaluationGRepo.UpdateByProperties(kpiEntity, properties);
                this._unitOfWork.SaveChanges();
                this._businessApplicationRepo.AddBussinessApplication(kpiDTO, kpiEntity);

                // thong bao
                INotificationRepository notifyRepo = new NotificationRepository(this._unitOfWork);
                var year = "";
                var month = "";
                if (kpiDTO.YearMonth != null)
                {
                    year = kpiDTO.YearMonth.ToString().Substring(0, 4);
                    month = kpiDTO.YearMonth.ToString().Substring(4);
                }
                else
                {
                    var kpi = this._repo.SelectWhere(x => x.Id == kpiDTO.Id).FirstOrDefault();
                    if (kpi != null)
                    {
                        year = kpiDTO.YearMonth.ToString().Substring(0, 4);
                        month = kpiDTO.YearMonth.ToString().Substring(4);
                    }
                }
                var notif = new NotificationDTO()
                {
                    FromUserName = userName,
                    ToUserName = kpiDTO.UserName,
                    Notes = "Đánh giá THCV, kỳ " + month + "/" + year + " đã hoàn thành.",
                    NotesEn = "KPI Assertment, period " + month + "/" + year + " has completed.",
                    UserDate = DateTime.Now
                };
                notifyRepo.Add(notif);

                ts.Commit();
            }
        }

        /// <summary>
        /// Hoàn thành multiple văn bản
        /// </summary>
        public void UpdateKpiProcessRange(List<KpiDTO> kpiDTOList, string userName, bool isUnlock = false)
        {
            using (var ts = this._unitOfWork.BeginTransaction())
            {
                foreach (var kpiDTO in kpiDTOList)
                {
                    var kpiEntity = new Kpi_KpiEvaluation
                    {
                        Id = kpiDTO.Id,
                    };

                    // huypq modified 16-3-2020 issue 282
                    if (kpiDTO.ActionId == 7)
                    {
                        var kpiQueryDTO = this._kpiEvaluationGRepo.SelectWhere(x => x.Id == kpiDTO.Id).Select(x => new KpiDTO
                        {
                            Id = x.Id,
                            HrKpiPoint = x.HrKpiPoint,
                            HrKpiPointClassification = x.HrKpiPointClassification,
                            Level1ManagerKpiPoint = x.Level1ManagerKpiPoint,
                            Level1ManagerKpiClassification = x.Level1ManagerKpiClassification,
                        }).FirstOrDefault();

                        if (!kpiQueryDTO.FinalKpiPoint.HasValue || kpiQueryDTO.FinalKpiPoint.Value == 0 )
                        {
                            if (kpiQueryDTO.HrKpiPoint > 0)
                            {
                                kpiQueryDTO.FinalKpiPoint = kpiQueryDTO.HrKpiPoint;
                                kpiQueryDTO.FinalKpiClassification = kpiQueryDTO.HrKpiPointClassification;
                            }
                            else
                            {
                                kpiQueryDTO.FinalKpiPoint = kpiQueryDTO.Level1ManagerKpiPoint;
                                kpiQueryDTO.FinalKpiClassification = kpiQueryDTO.Level1ManagerKpiClassification;
                            }

                            kpiEntity = new Kpi_KpiEvaluation
                            {
                                Id = kpiQueryDTO.Id,
                                FinalKpiPoint = kpiQueryDTO.FinalKpiPoint,
                                FinalKpiClassification = kpiQueryDTO.FinalKpiClassification
                            };
                            var properties = new List<Expression<Func<Kpi_KpiEvaluation, object>>>
                        {
                            (x => x.FinalKpiPoint),
                            (x => x.FinalKpiClassification),
                        };
                            this._kpiEvaluationGRepo.UpdateByProperties(kpiEntity, properties);
                            this._unitOfWork.SaveChanges();
                        }
                    }

                    this._businessApplicationRepo.AddBussinessApplication(kpiDTO, kpiEntity);

                    // tao thong bao hoan thanh neu khong phai mo khoa
                    if (!isUnlock)
                    {
                        INotificationRepository notifyRepo = new NotificationRepository(this._unitOfWork);
                        var year = DateTime.Now.Year.ToString();
                        var month = DateTime.Now.Month.ToString();
                        if (kpiDTO.YearMonth != null)
                        {
                            year = kpiDTO.YearMonth.ToString().Substring(0, 4);
                            month = kpiDTO.YearMonth.ToString().Substring(4);
                        }
                        //else
                        //{
                        //    var kpi = this._repo.SelectWhere(x => x.Id == kpiDTO.Id).FirstOrDefault();
                        //    if (kpi != null)
                        //    {
                        //        year = kpiDTO.YearMonth.ToString().Substring(0, 4);
                        //        month = kpiDTO.YearMonth.ToString().Substring(4);
                        //    }
                        //}
                        var notif = new NotificationDTO()
                        {
                            FromUserName = userName,
                            ToUserName = kpiDTO.UserName,
                            Notes = "Đánh giá THCV, kỳ " + month + "/" + year + " đã hoàn thành.",
                            NotesEn = "KPI Assertment, period " + month + "/" + year + " has completed.",
                            UserDate = DateTime.Now
                        };
                        notifyRepo.Add(notif);
                    }                    
                }
                ts.Commit();
            }
        }

        public IEnumerable<KpiDTO> GetHrManagerKpiPaging(DivisionManagerKpiFilter filter)
        {
            var query = this._repo.SelectWhere(x => x.EventDiaryId != null && !x.IsDeleted);
            query = this.FilterHrManager(query, filter);

            return GetPagingResult(filter, query);
        }

        public int CountHrManagerKpi(DivisionManagerKpiFilter filter)
        {
            var query = this._repo.SelectWhere(x => x.EventDiaryId != null && !x.IsDeleted);
            query = this.FilterHrManager(query, filter);
            return query.Count();
        }

        private IQueryable<Kpi_KpiEvaluation> FilterHrManager(IQueryable<Kpi_KpiEvaluation> query, DivisionManagerKpiFilter filter)
        {
            var result = query;
            if (filter.YearMonth > 0)
            {
                result = result.Where(x => x.YearMonth == filter.YearMonth);
            }

            if (!filter.IsVip)
            {
                if (filter.OrgIds != null && filter.OrgIds.Count > 0)
                {
                    result = result.Where(x => filter.OrgIds.Contains(x.OrganizationId));
                }
                else
                {
                    if (filter.OrgId > 0)
                    {
                        result = result.Where(x => x.OrganizationId == filter.OrgId);
                    }
                }

                if (!String.IsNullOrEmpty(filter.empName))
                {
                    result = result.Where(x => x.UserName == filter.empName);
                }

                if (!String.IsNullOrEmpty(filter.Level1ManagerUserName))
                {
                    result = result.Where(x => x.Level1ManagerUserName == filter.Level1ManagerUserName);
                }

                if (!String.IsNullOrEmpty(filter.Level2ManagerUserName))
                {
                    result = result.Where(x => x.Level2ManagerUserName == filter.Level2ManagerUserName);
                }
            }
            else
            {
                var usr = this._userOrgGRepo.SelectWhere(x => x.IsOrgManager == true);
                result = result.Join(usr, k => k.UserName, u => u.UserName, (k, u) => k);
            }

            if (filter.StatusIds != null && filter.StatusIds.Count > 0)
            {
                result = result.Where(x => filter.StatusIds.Contains(x.StatusId));
            }
            return result;
        }

        #endregion

        private IQueryable<Kpi_KpiEvaluation> Filter(IQueryable<Kpi_KpiEvaluation> query, KpiFilterModelDTO filter)
        {
            var result = query;

            // huypq modified 11/12/19, Hr + hr manager use this
            if (filter.OrgIds != null && filter.OrgIds.Any())
            {
                result = result.Where(x => filter.OrgIds.Contains(x.OrganizationId));
            }
            // HR KPI
            else if (filter.orgId != null)
            {
                // orgId = -1 : tat ca
                if (filter.orgId == -1)
                {
                    //query = _repo.SelectAll();
                }
                else
                {
                    result = result.Where(x => x.OrganizationId == filter.orgId);
                }
            }
            else // User KPI
            {
                result = query.Where(x => x.UserName == filter.UserName);
            }

            if (filter.YearMonth > 0)
            {
                result = result.Where(x => x.YearMonth == filter.YearMonth);
            }

            if (filter.StatusIds != null && filter.StatusIds.Count > 0)
            {
                result = result.Where(x => filter.StatusIds.Contains(x.StatusId));
            }

            if (!string.IsNullOrWhiteSpace(filter.Level1ManagerName))
            {
                result = result.Where(x => x.Level1ManagerFullName.Contains(filter.Level1ManagerName));
            }

            if (!string.IsNullOrWhiteSpace(filter.EmployeeName))
            {
                result = result.Where(x => x.EmployeeName.Contains(filter.EmployeeName));
            }

            // huypq modified 16-12-2019 dung cho unlock kpi
            if (!string.IsNullOrEmpty(filter.Level1ManagerUserName))
            {
                result = result.Where(x => x.Level1ManagerUserName == filter.Level1ManagerUserName);
            }

            if (!string.IsNullOrEmpty(filter.Level2ManagerUserName))
            {
                result = result.Where(x => x.Level2ManagerUserName == filter.Level2ManagerUserName);
            }

            if (!string.IsNullOrEmpty(filter.EmpUserName))
            {
                result = result.Where(x => x.UserName == filter.EmpUserName);
            }

            if (!string.IsNullOrEmpty(filter.Note))
            {
                result = result.Where(x => x.SubmitNote.Contains(filter.Note));
            }

            return result;
        }

        #region Create Kpi Global
        /// <summary>
        /// * Tạo các kpi cho tất cả nhân viên khi config tháng (KpiEvaluation)
        /// * Tạo nhật ký sự kiện cho tất cả phòng ban, (EventDiary) 
        /// * Tạo kpi period config để lưu lại lịch sử tạo
        /// * Tạo KpiEvaluation và KpiCriterionDetail theo từng CriterionType (Với mỗi CriterionType tạo KpiCriterionDetail và KpiEvaluation cho những user có 
        /// jobTile avaiable với criterion type ấy)
        /// </summary>
        public void CreateKpiGlobal(KpiPeriodConfigDTO config)
        {
            using (var ts = this._unitOfWork.BeginTransaction())
            {
                // Tạo nhật kí sự kiện 
                var eventDiaries = this.CreateEventDiaries(config);

                // Tạo kpi period config
                this.CreateKpiPeriodConfig(config);

                //var criterionTypes = this._criterionTypeGRepo.SelectWhere(x => config.CriterionTypes.Contains(x.Id)).ToList();
                // huypq modified 14-02-2020 Tao tat ca cac loai criterion
                var criterionTypes = this._criterionTypeGRepo.SelectAll().ToList();

                foreach (var criterionType in criterionTypes)
                {
                    var users = this.GetUsersByCriterionType(criterionType.Id);

                    foreach (var u in users)
                    {
                        var currentEventDiary = eventDiaries.FirstOrDefault(x => x.EventDiaryConfigId == u.EventDiaryConfigId);

                        // org cua user ko co diary => next
                        if (currentEventDiary == null)
                        {
                            continue;
                        }

                        //tạo kpi cho user này
                        this.CreateUserKpi(config, u, currentEventDiary, criterionType);
                    }
                }

                ts.Commit();
            }
        }

        public IEnumerable<UserInfoModel> GetUsersInfoByUsername(List<string> usernames, int criterionType)
        {
            var query = (from u in this._userGRepo.SelectAll()
                         join uo in this._userOrgGRepo.SelectWhere(x => usernames.Contains(x.UserName)) on u.UserName equals uo.UserName
                         join ec in this._eventDiaryConfigGRepo.SelectAll() on uo.EventDiaryConfigId equals ec.Id
                         join jc in this._jobCriterionGRepo.SelectWhere(x => x.CriterionTypeId == criterionType) on uo.JobTitleId equals jc.JobTitleId
                         select new UserInfoModel
                         {
                             UserId = u.Id,
                             OrgId = ec.OrgId.Value,
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
                             OrgName = ec.OrgName
                         });
            return query != null ? query.ToList() : new List<UserInfoModel>();
        }

        /// <summary>
        /// Lấy user theo criterionType
        /// </summary>
        /// <param name="criterionType">Loại đánh giá</param>
        private IEnumerable<UserInfoModel> GetUsersByCriterionType(int criterionType)
        {
            var kpiType = _criterionTypeGRepo.SelectWhere(x => x.Id == criterionType).Select(x => x.Code).FirstOrDefault();

            // huypq modified 4/2/2020 #248, query status = CT
            var query = (from u in this._userGRepo.SelectAll()
                         join uo in this._userOrgGRepo.SelectWhere(x => x.KpiType == kpiType && x.Status == "CT") on u.UserName equals uo.UserName
                         join ec in this._eventDiaryConfigGRepo.SelectAll() on uo.EventDiaryConfigId equals ec.Id
                         //join jc in this._jobCriterionGRepo.SelectWhere(x => x.CriterionTypeId == criterionType) on uo.JobTitleId equals jc.JobTitleId
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
                             Code = uo.Code
                         });
            var testQuery = query.ToString();
            return query != null ? query.ToList() : new List<UserInfoModel>();
        }

        /// <summary>
        /// Tao event diary
        /// </summary>
        public IEnumerable<Kpi_EventDiary> CreateEventDiaries(KpiPeriodConfigDTO config/*, int? orgId*/)
        {
            /// huypq modified 17/12/19. Thêm org id để query theo org
            var ecConfigs = _eventDiaryConfigGRepo.SelectWhere(x => x.IsActive != null && x.IsActive.Value).ProjectTo<EventDiaryConfigDTO>().ToList();
            return this.CreateEventDiaryByConfig(ecConfigs, config);
        }

        public IEnumerable<Kpi_EventDiary> CreateEventDiaryByConfig(IEnumerable<EventDiaryConfigDTO> ecConfigs, KpiPeriodConfigDTO config)
        {
            var eventDiaries = ecConfigs.Select(x => new Kpi_EventDiary
            {
                OrgId = x.OrgId.GetValueOrDefault(),
                OrgName = x.OrgName,
                Level1ManagerFullName = x.Level1ManagerFullName,
                Level1ManagerUserName = x.Level1ManagerUserName,
                Level2ManagerFullName = x.Level2ManagerFullName,
                Level2ManagerUserName = x.Level2ManagerUserName,
                FromDate = config.FromDate,
                ToDate = config.ToDate,
                YearMonth = config.YearMonth,
                EventDiaryConfigId = x.Id
            }).ToList();

            var rs = this._eventDiaryGRepo.AddRange(eventDiaries);
            this._unitOfWork.SaveChanges();
            return rs;
        }

        /// <summary>
        /// Mục đích lưu lại lịch sử tạo
        /// </summary>
        private Sys_KpiPeriodConfig CreateKpiPeriodConfig(KpiPeriodConfigDTO config)
        {
            var kpiPeriodEtt = AutoMapperHelper.Map<KpiPeriodConfigDTO, Sys_KpiPeriodConfig>(config);
            var rs = this._kpiPeriodGRepo.Add(kpiPeriodEtt);
            this._unitOfWork.SaveChanges();
            return rs;
        }

        /// <summary>
        /// Add kpi evaluation and it's kpi detail
        /// </summary>
        public void CreateUserKpi(KpiPeriodConfigDTO config, UserInfoModel u, Kpi_EventDiary diary, Kpi_CriterionType criterionType)
        {
            var firstStatus = this._businessApplicationRepo.GetFirstKpiProcess();

            // tạo kpi evaluation
            var kpi = new Kpi_KpiEvaluation
            {
                UserName = u.UserName,
                EventDiaryId = diary.Id,
                EmployeeName = u.UserFullName,
                EmployeeJobId = u.JobTitleId.Value,
                EmployeeJobTitle = u.JobTitle,
                Level1ManagerOrgId = u.Level1ManagerOrgId.GetValueOrDefault(),
                KpiMonthNumber = config.MonthNumber,
                KpiMonth = DateTime.ParseExact(config.YearMonth.ToString(), "yyyyMM", System.Globalization.CultureInfo.InvariantCulture),
                YearMonth = config.YearMonth,
                KpiPeriodConfigId = config.Id,
                CriterionType = criterionType.Id,
                CriterionTypeName = criterionType.Name,
                CreatedDate = DateTime.Now,
                CreatedByUser = string.Empty,
                StatusId = firstStatus.Id,
                StatusName = firstStatus.Title,
                // IsActived = false,
                IsDeleted = false,
                Level1ManagerUserName = u.Level1ManagerUserName,
                Level1ManagerFullName = u.Level1ManagerFullName,
                Level1ManagerJobTitleId = u.Level1ManagerJobTileId,
                Level1ManagerJobTitle = u.Level1ManagerJobTitle,
                Level1ManagerOrgName = u.Level1ManagerOrgName,
                Level1ManagerUserId = u.UserId,
                //Level1ManagerOr
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
            };

            // nghilq 13/1/2019 tao cac cot tieng anh
            var stat = _processStatusGRepo.SelectWhere(x => x.Id == kpi.StatusId).FirstOrDefault();
            kpi.StatusNameEn = stat != null ? stat.TitleEn : "";

            var criterType = _criterionTypeGRepo.SelectWhere(x => x.Id == kpi.CriterionType).FirstOrDefault();
            kpi.CriterionTypeNameEn = criterType != null ? criterType.NameEn : "";

            var org = _orgGRepo.SelectWhere(x => x.Id == kpi.OrganizationId).FirstOrDefault();
            kpi.OrganizationEn = org != null ? org.NameEn : "";

            var lv1Usr = _userOrgGRepo.SelectWhere(x => x.UserName == kpi.Level1ManagerUserName).FirstOrDefault();
            if (lv1Usr != null)
            {
                var lv1MngJobTtl = _jobTitleRepo.SelectWhere(x => x.Id == lv1Usr.JobTitleId).FirstOrDefault();
                kpi.Level1ManagerJobTitleEn = lv1MngJobTtl != null ? lv1MngJobTtl.TitleEn : "";

                var org1 = _orgGRepo.SelectWhere(x => x.Id == lv1Usr.OrgId).FirstOrDefault();
                kpi.Level1ManagerOrgNameEn = org1 != null ? org1.NameEn : "";
            }

            var lv2Usr = _userOrgGRepo.SelectWhere(x => x.UserName == kpi.Level2ManagerUserName).FirstOrDefault();
            if (lv2Usr != null)
            {
                var lv2MngJobTtl = _jobTitleRepo.SelectWhere(x => x.Id == lv2Usr.JobTitleId).FirstOrDefault();
                kpi.Level2ManagerJobTitleEn = lv2MngJobTtl != null ? lv2MngJobTtl.TitleEn : "";

                var org2 = _orgGRepo.SelectWhere(x => x.Id == lv2Usr.OrgId).FirstOrDefault();
                kpi.Level2ManagerOrgNameEn = org2 != null ? org2.NameEn : "";
            }

            var empJobTtl = _jobTitleRepo.SelectWhere(x => x.Id == kpi.EmployeeJobId).FirstOrDefault();
            kpi.EmployeeJobTitleEn = empJobTtl != null ? empJobTtl.TitleEn : "";

            // add kpi evaluate
            var kpiEntity = this._kpiEvaluationGRepo.Add(kpi);
            this._unitOfWork.SaveChanges();

            // add kpi criterion detail
            var criterionCatalogs = this.GetCriterionCatalogByCriterionType(criterionType.Id);
            var kpiCriterionDetails = criterionCatalogs.Select(x => new Kpi_KpiCriterionDetail
            {
                CriterionTitle = x.CriterionTitle,
                MinimumPoint = x.MinimumPoint,
                MaximumPoint = x.MaximumPoint,
                EmployeeEvaluatePoint = x.MaximumPoint,
                ManagerEvaluatePoint = x.MaximumPoint,
                Month = config.MonthNumber,
                Year = config.FromDate.Year,
                UserId = u.UserId,
                OrgId = u.OrgId,
                KpiEvaluateId = kpiEntity.Id,
                KpiCatalogId = x.Id,
                CreatedDate = DateTime.Now,
            });
            this._kpiCriterionDetailGRepo.AddRange(kpiCriterionDetails);
            this._unitOfWork.SaveChanges();
        }

        private IEnumerable<CriterionCatalogDTO> GetCriterionCatalogByCriterionType(int type)
        {
            var qr = (from c in this._kpiCriterionCatalogGRepo.SelectWhere(x => x.IsFolder)
                      join ct in this._kpiCriterionTypeCatalogGRepo.SelectWhere(x => x.CriterionTypeId == type) on c.Id equals ct.CriterionCatalogId
                      select new CriterionCatalogDTO
                      {
                          Id = c.Id,
                          Code = c.Code,
                          CriterionTitle = c.CriterionTitle,
                          CriterionTitleEn = c.CriterionTitleEn,
                          MinimumPoint = 0,
                          MaximumPoint = ct.StartPoint.Value,
                          ParentId = c.ParentId,
                          IsFolder = c.IsFolder,
                          CategoryId = c.CategoryId,
                          CategoryName = c.CategoryName,
                          CriterionLevel = c.CriterionLevel
                      });
            return qr.ToList();
        }

        public bool GetIsKpiValidForDivManager(int kpiId, string userName)
        {
            var kpi = this._kpiEvaluationGRepo.SelectWhere(x => x.Id == kpiId).FirstOrDefault();
            if (kpi != null)
            {
                var usr = this._userOrgGRepo.SelectWhere(x => x.UserName == userName).FirstOrDefault();
                if (usr != null)
                {
                    if (this._divMngRepo.SelectWhere(x => x.UserId == usr.Id && kpi.OrganizationId == x.OrgId).FirstOrDefault() != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgsByDivManager(string userName)
        {
            var usr = this._userOrgGRepo.SelectWhere(x => x.UserName == userName).FirstOrDefault();
            if (usr != null)
            {
                // danh sach phan quyen ql khoi
                var divMngPers = this._divMngRepo.SelectWhere(x => x.UserId == usr.Id);

                // danh sach to chuc
                var query = this._orgGRepo.SelectAll()
                        .Join(divMngPers, o => o.Id, p => p.OrgId, (o, p) => o);

                return query.OrderBy(x => x.Id).Select(x => new OrgInfoModelDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    NameEn = x.NameEn,
                    Description = x.Description,
                    OrganizationTypeID = x.OrganizationTypeID,
                    NodeID = x.NodeID,
                    DirectoryPath = x.DirectoryPath
                }).ToList();
            }
            return new List<OrgInfoModelDTO>();
        }
        #endregion
    }
}
