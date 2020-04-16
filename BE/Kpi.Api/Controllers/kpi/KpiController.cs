using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Core.Helper;
using Kpi.Service.DataServices.Kpi;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web.Http;

namespace Kpi.Api.Controllers.kpi
{
    public class UpdateStatusUncompletedKpiModel
    {
        public int YearMonth { get; set; }
        public List<int> OrgIds { get; set; }
        //public string OrgPath { get; set; }
    }

    [Authorize]
    public class KpiController : BaseGridApiController<IKpiService, KpiRS, KpiDTO, KpiGridFilterModel, KpiFilterModelDTO>
    {
        public KpiController(IKpiService service) : base(service)
        {
        }

        [HttpPost]
        public RespondData GetPagingMng(ManagerKpiFilterModelDTO filter)
        {
            filter.Level1ManagerUserName = this.UserName;
            return Success(this._service.GetPagingMng(filter));
        }

        [HttpPost]
        public RespondData GetPagingMngLv2(ManagerKpiFilterModelDTO filter)
        {
            filter.Level2ManagerUserName = this.UserName;
            return Success(this._service.GetPagingMngLv2(filter));
        }

        [HttpPost]
        public RespondData GetOrgs()
        {
            var res = this._service.GetOrgs();
            return Success(res);
        }

        [HttpPost]
        public RespondData CreateKpiGlobal(KpiPeriodConfigDTO config)
        {
            config.CreatedByUser = this.UserName;
            config.CreatedDate = DateTime.Now;
            this._service.CreateKpiGlobal(config);
            return Success(null);
        }

        /// <summary>
        /// Update with specific action
        /// </summary>
        [HttpPost]
        public RespondData UpdateKpiEmp(KpiRS kpiRs)
        {
            var kpiDTO = this.FetchDataUpdate(kpiRs);
            kpiDTO.ActionId = 2;
            this._service.Update(kpiDTO);
            return Success(null);
        }

        [HttpPost]
        public RespondData UpdateKpiVIP(KpiRS kpiRs)
        {
            var kpiDTO = this.FetchDataUpdate(kpiRs);
            this._service.UpdateKpiVIP(kpiDTO);
            return Success(null);
        }


        /// <summary>
        /// Chuyển trạng thái cho quản lý cấp 2
        /// </summary>
        [HttpPost]
        public RespondData ProcessKpiLevel2Mng(KpiDTO kpiDTO)
        {
            kpiDTO.UpdatedDate = DateTime.Now;
            kpiDTO.UpdatedByUser = this.UserName;
            kpiDTO.SentHRDate = DateTime.Now;
            this._service.ProcessKpiLevel2Mng(kpiDTO);
            return Success(null);
        }

        [HttpPost]
        public RespondData UpdateStatusUncompletedKpi(UpdateStatusUncompletedKpiModel filter)
        {
            try
            {
                var filterDTO = AutoMapperHelper.Map<UpdateStatusUncompletedKpiModel, UncompeletedKpiFilterModelDTO>(filter);
                filterDTO.Username = this.UserName;
                this._service.UpdateStatusUncompletedKpi(filterDTO);
                return Success(null);
            }
            catch (ObjectNotFoundException ex)
            {
                return DefaultSuccess(ex.Message);
            }
        }

        [HttpPost]
        public RespondData UpdateKpiHRSendManager(KpiRS kpiRs)
        {
            var kpiDTO = this.FetchDataUpdate(kpiRs);
            this._service.UpdateKpiHRSendManager(kpiDTO);
            return Success(null);
        }

        [HttpPost]
        public RespondData UpdateKpiSendCorLeader(KpiRS kpiRs)
        {
            var kpiDTO = this.FetchDataUpdate(kpiRs);
            this._service.UpdateKpiSendCorLeader(kpiDTO);
            return Success(null);
        }

        [HttpPost]
        public RespondData SaveKpiSendCorLeader(KpiRS kpiRs)
        {
            var kpiDTO = this.FetchDataUpdate(kpiRs);
            this._service.SaveKpiSendCorLeader(kpiDTO);
            return Success(null);
        }
               
        [HttpPost]
        public RespondData UpdateKpiComplete(KpiRS kpiRs)
        {
            var kpiDTO = this.FetchDataUpdate(kpiRs);
            this._service.UpdateKpiComplete(kpiDTO, this.UserName);
            return Success(null);
        }

        [HttpPost]
        public RespondData UpdateHRUnlockKpiRange(List<KpiRS> kpiRsList)
        {
            var kpiDTOList = new List<KpiDTO>();
            foreach (var kpiRs in kpiRsList)
            {
                var kpiDTO = this.FetchDataUpdate(kpiRs);
                kpiDTOList.Add(kpiDTO);
            }
            this._service.UpdateHRUnlockKpiRange(kpiDTOList, this.UserName);
            return Success(null);
        }

        [HttpPost]
        public RespondData UpdateKpiCompleteRange(List<KpiRS> kpiRsList)
        {
            var kpiDTOList = new List<KpiDTO>();
            foreach (var kpiRs in kpiRsList)
            {
                var kpiDTO = this.FetchDataUpdate(kpiRs);
                kpiDTOList.Add(kpiDTO);
            }
            this._service.UpdateKpiCompleteRange(kpiDTOList, this.UserName);
            return Success(null);
        }

        [HttpPost]
        public RespondData CountMng(ManagerKpiFilterModelDTO filter)
        {
            filter.Level1ManagerUserName = this.UserName;
            return Success(this._service.CountMng(filter));
        }

        [HttpPost]
        public RespondData CountMngLv2(ManagerKpiFilterModelDTO filter)
        {
            filter.Level2ManagerUserName = this.UserName;
            return Success(this._service.CountMngLv2(filter));
        }

        protected override KpiFilterModelDTO FetchFilterPaging(KpiFilterModelDTO filter)
        {
            filter.UserName = this.UserName;
            return base.FetchFilterPaging(filter);
        }

        protected override KpiDTO FetchDataUpdate(KpiRS model)
        {
            var kpiDTO = base.FetchDataUpdate(model);
            kpiDTO.UpdatedDate = DateTime.Now;
            kpiDTO.UpdatedByUser = this.UserName;
            //kpiDTO.KpiCriterionDetailList.ForEach(x => x. = DateTime.Now);
            return kpiDTO;
        }

        [HttpPost]
        public RespondData GetDivisionManagerKpiPaging(DivisionManagerKpiFilter filter)
        {
            filter.UserName = this.UserName;
            return Success(this._service.GetDivisionManagerKpiPaging(filter));
        }

        [HttpPost]
        public RespondData CountDivisionManager(DivisionManagerKpiFilter filter)
        {
            filter.UserName = this.UserName;
            return Success(this._service.CountDivisionManager(filter));
        }

        [HttpPost]
        public RespondData GetIsKpiValidForDivManager(DivisionManagerKpiFilter filter)
        {
            return Success(this._service.GetIsKpiValidForDivManager(filter.OrgId, this.UserName));
        }

        [HttpPost]
        public RespondData GetOrgsByDivManager()
        {
            return Success(this._service.GetOrgsByDivManager(this.UserName));
        }

        [HttpPost]
        public RespondData ProcessRangeKpiLevel2Mng(IEnumerable<KpiDTO> kpiDTOs)
        {
            foreach (var kpiDTO in kpiDTOs)
            {
                kpiDTO.UpdatedDate = DateTime.Now;
                kpiDTO.UpdatedByUser = this.UserName;
                kpiDTO.SentHRDate = DateTime.Now;
                kpiDTO.ActionId = 4;
            }
            this._service.ProcessRangeKpiLevel2Mng(kpiDTOs);
            return DefaultSuccess(string.Empty);
        }

        [HttpPost]
        public RespondData UpdateRangeKpiBusinessApplication(IEnumerable<KpiRS> kpis)
        {
            var kpiDtos = kpis.Select(x => this.FetchDataUpdate(x)).ToList();
            this._service.UpdateRangeKpiBusinessApplication(kpiDtos);
            return DefaultSuccess(string.Empty);
        }

        [HttpPost]
        public RespondData UpdateRangeKpiHRManagerPropose(IEnumerable<KpiRS> kpis)
        {
            var kpiDTOs = kpis.Select(x => this.FetchDataUpdate(x)).ToList();
            this._service.UpdateRangeKpiHRManagerPropose(kpiDTOs);
            return DefaultSuccess(string.Empty);
        }

        [HttpPost]
        public RespondData GetHrManagerKpiPaging(DivisionManagerKpiFilter filter)
        {
            filter.UserName = this.UserName;
            return Success(this._service.GetHrManagerKpiPaging(filter));
        }

        [HttpPost]
        public RespondData CountHrManagerKpi(DivisionManagerKpiFilter filter)
        {
            filter.UserName = this.UserName;
            return Success(this._service.CountHrManagerKpi(filter));
        }
    }
}