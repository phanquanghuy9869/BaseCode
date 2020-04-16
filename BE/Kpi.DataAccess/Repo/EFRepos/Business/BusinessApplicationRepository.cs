using Kpi.Core.DTO;
using Kpi.Core.Enums;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.DataAccess.Repo.EFRepos.Business
{
    public interface IBusinessApplicationRepository
    {
        Process_BussinessApplication AddBussinessApplication(KpiDTO kpiDTo, Kpi_KpiEvaluation kpiEntity);
        ProcessStatusDTO GetFirstKpiProcess();
    }

    public class BusinessApplicationRepository : IBusinessApplicationRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IGenericRepository<Kpi_KpiEvaluation> _kpiEvaluationGRepo;
        private readonly IGenericRepository<Process_ProcessTransition> _processTransitionGRepo;
        private readonly IGenericRepository<Process_ProcessStatus> _processStatusGRepo;
        private readonly IGenericRepository<Process_BussinessApplication> _bussinessApplicationGRepo;
        private readonly IGenericRepository<Process_BussinessProcess> _bussinessProcessGRepo;

        public BusinessApplicationRepository(IUnitOfWork unitOfWork)
        {
            this._uow = unitOfWork;
            _kpiEvaluationGRepo = this._uow.GetDataRepository<Kpi_KpiEvaluation>();
            _processTransitionGRepo = _uow.GetDataRepository<Process_ProcessTransition>();
            _processStatusGRepo = _uow.GetDataRepository<Process_ProcessStatus>();
            _bussinessApplicationGRepo = this._uow.GetDataRepository<Process_BussinessApplication>();
            _bussinessProcessGRepo = this._uow.GetDataRepository<Process_BussinessProcess>();
        }

        public ProcessStatusDTO GetFirstProcess(BusinessProcessTypeCode processType)
        {
            var type = processType.ToString();
            var query = (from ps in _processStatusGRepo.SelectWhere(x => x.IsFirstProccess != null && x.IsFirstProccess.Value)
                         join bs in _bussinessProcessGRepo.SelectWhere(x => x.ObjectTypeCode == type) on ps.BussinessProcessID equals bs.Id
                         select new ProcessStatusDTO
                         {
                             Id = ps.Id,
                             Title = ps.Title,
                             Description = ps.Description,
                             BussinessProcessID = ps.BussinessProcessID
                         });
            return query.FirstOrDefault();
        }

        public ProcessStatusDTO GetFirstKpiProcess()
        {
            return this.GetFirstProcess(BusinessProcessTypeCode.KpiEvaluation);
        }

        //public Process_BussinessApplication AddBussinessApplication(KpiDTO kpiDto, Kpi_KpiEvaluation kpiEntity)
        //{
        //    //get kpi evaluation
        //   var kpiEvaluation = new Kpi_KpiEvaluation
        //   {
        //       Id = kpiDto.Id,
        //       StatusId = processTransitionDTO.TargetProcessID,
        //       StatusName = _processStatusGRepo.SelectWhere(x => x.Id == processTransitionDTO.TargetProcessID).Select(x => x.Title).FirstOrDefault(),
        //   };
        //}

        public Process_BussinessApplication AddBussinessApplication(KpiDTO kpiDto, Kpi_KpiEvaluation kpiEntity)
        {
            // get process transition
            Process_ProcessTransition processTransitionDTO = _processTransitionGRepo.Get(x => x.SourceProcessID == kpiDto.StatusId && x.ActionID == kpiDto.ActionId);
            if (processTransitionDTO == null) return null;
                      
            kpiEntity.StatusId = processTransitionDTO.TargetProcessID;
            kpiEntity.StatusName = _processStatusGRepo.SelectWhere(x => x.Id == processTransitionDTO.TargetProcessID).Select(x => x.Title).FirstOrDefault();

            var properties = new List<Expression<Func<Kpi_KpiEvaluation, object>>>
            {
                (x => x.StatusId),
                (x => x.StatusName),
            };
            this._kpiEvaluationGRepo.UpdateByProperties(kpiEntity, properties);
            this._uow.SaveChanges();

            Process_BussinessApplication business_BussinessApplication = new Process_BussinessApplication
            {
                AfterStatus = processTransitionDTO.TargetProcessID,
                BeforeStatus = processTransitionDTO.SourceProcessID,
                BusinessTransitionID = kpiEntity.Id,
                HandleByUserTitleName = "",
                HandledByOrganization = kpiDto.OrganizationId,
                HandledByUser = kpiDto.UpdatedByUser,
                ObjectTypeCode = "Kpi_KpiEvaluation",
                HandledByUserTitle = 0,
                HandledDate = DateTime.Now,
                RegencyTitle = processTransitionDTO.ActionName,
                ActionID = kpiDto.ActionId + "",
                Note = kpiEntity.CriterionTypeName,
                ObjectID = kpiEntity.Id,
                OrganizationName = kpiDto.Organization
            };

            business_BussinessApplication = _bussinessApplicationGRepo.Add(business_BussinessApplication);
            this._uow.SaveChanges();
            return business_BussinessApplication;
        }
    }
}
