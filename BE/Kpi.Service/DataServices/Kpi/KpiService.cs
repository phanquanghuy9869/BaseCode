using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Kpi;
using Kpi.Service.BaseServices;
using System.Collections.Generic;
using System;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using System.Linq;
using System.Text;
using System.IO;

namespace Kpi.Service.DataServices.Kpi
{
    public interface IKpiService : IGridService<KpiDTO, KpiFilterModelDTO>
    {
        IEnumerable<KpiDTO> GetPagingMng(ManagerKpiFilterModelDTO filter);
        void CreateKpiGlobal(KpiPeriodConfigDTO config);
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        void ProcessKpiLevel2Mng(KpiDTO processKpi);
        void UpdateStatusUncompletedKpi(UncompeletedKpiFilterModelDTO filter);
        void UpdateKpiHRSendManager(KpiDTO kpiDTO);
        void SaveKpiSendCorLeader(KpiDTO kpiDTO);
        void UpdateKpiSendCorLeader(KpiDTO kpiDTO);
        void UpdateKpiComplete(KpiDTO kpiDTO, string userName);
        void UpdateKpiCompleteRange(List<KpiDTO> kpiDTOList, string userName);
        int CountMng(ManagerKpiFilterModelDTO filter);
        IEnumerable<KpiDTO> GetPagingMngLv2(ManagerKpiFilterModelDTO filter);
        int CountMngLv2(ManagerKpiFilterModelDTO filter);
        void UpdateHRUnlockKpiRange(List<KpiDTO> kpiDTOList, string userName);
        void UpdateKpiVIP(KpiDTO kpiDTO);
        int CountDivisionManager(DivisionManagerKpiFilter filter);
        IEnumerable<KpiDTO> GetDivisionManagerKpiPaging(DivisionManagerKpiFilter filter);
        bool GetIsKpiValidForDivManager(int kpiId, string userName);
        IEnumerable<OrgInfoModelDTO> GetOrgsByDivManager(string userName);
        void ProcessRangeKpiLevel2Mng(IEnumerable<KpiDTO> kpiDtos);
        void UpdateRangeKpiBusinessApplication(IEnumerable<KpiDTO> kpiDTOs);
        void UpdateRangeKpiHRManagerPropose(IEnumerable<KpiDTO> kpiDTOs);
        IEnumerable<KpiDTO> GetHrManagerKpiPaging(DivisionManagerKpiFilter filter);
        int CountHrManagerKpi(DivisionManagerKpiFilter filter);
    }

    public class KpiService : BaseGridService<IKpiEvaluationRepository, KpiDTO, KpiFilterModelDTO>, IKpiService
    {
        private readonly IOrgUserRepository _orgRepo;

        public KpiService(IKpiEvaluationRepository gridRepository, IOrgUserRepository orgRepo) : base(gridRepository)
        {
            this._orgRepo = orgRepo;
        }

        public void CreateKpiGlobal(KpiPeriodConfigDTO config)
        {
            config.CreatedDate = DateTime.Now;
            this._gridRepository.CreateKpiGlobal(config);
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgs()
        {
            return this._gridRepository.GetOrgs();
        }

        public void ProcessKpiLevel2Mng(KpiDTO processKpi)
        {
            processKpi.ActionId = 4;
            this._gridRepository.ProcessKpiLevel2Mng(processKpi);
        }

        public IEnumerable<KpiDTO> GetPagingMng(ManagerKpiFilterModelDTO filter)
        {
            return this._gridRepository.GetPagingMng(filter);
        }

        public void UpdateStatusUncompletedKpi(UncompeletedKpiFilterModelDTO filter)
        {
            // Lấy dữ liệu
            var actionId = 8;
            filter.ActionId = actionId;
            this._gridRepository.UpdateStatusUncompletedKpi(filter);
        }

        public void UpdateKpiHRSendManager(KpiDTO kpiDTO)
        {
            kpiDTO.ActionId = 5;
            this._gridRepository.UpdateKpiBussinessApplication(kpiDTO);
        }

        public void UpdateKpiSendCorLeader(KpiDTO kpiDTO)
        {
            kpiDTO.ActionId = 6;
            this._gridRepository.UpdateKpiHRManagerPropose(kpiDTO);
        }

        public void SaveKpiSendCorLeader(KpiDTO kpiDTO)
        {
            kpiDTO.ActionId = 0;
            this._gridRepository.UpdateKpiHRManagerPropose(kpiDTO);
        }

        public void UpdateKpiComplete(KpiDTO kpiDTO, string userName)
        {
            kpiDTO.ActionId = 7;
            this._gridRepository.UpdateKpiLeaderPropose(kpiDTO, userName);
        }

        public void UpdateKpiCompleteRange(List<KpiDTO> kpiDTOList, string userName)
        {
            foreach (var k in kpiDTOList)
            {
                k.ActionId = 7;
            }
            this._gridRepository.UpdateKpiProcessRange(kpiDTOList, userName);
        }

        public int CountMng(ManagerKpiFilterModelDTO filter)
        {
            return this._gridRepository.CountMng(filter);
        }

        public void UpdateHRUnlockKpiRange(List<KpiDTO> kpiDTOList, string userName)
        {
            foreach (var k in kpiDTOList)
            {
                k.ActionId = 9;
            }
            this._gridRepository.UpdateKpiProcessRange(kpiDTOList, userName,true);
        }

        public IEnumerable<KpiDTO> GetPagingMngLv2(ManagerKpiFilterModelDTO filter)
        {
            return this._gridRepository.GetPagingMngLv2(filter);
        }

        public int CountMngLv2(ManagerKpiFilterModelDTO filter)
        {
            return this._gridRepository.CountMngLv2(filter);
        }

        public void UpdateKpiVIP(KpiDTO kpiDTO)
        {
            if (!ValidateUserVIP(kpiDTO.UpdatedByUser))
            {
                throw new UnauthorizedAccessException("Tài khoản không có quyền");
            }
            kpiDTO.ActionId = 10;
            this._gridRepository.Update(kpiDTO);
        }

        private bool ValidateUserVIP(string username)
        {
            return this._orgRepo.IsVIP(username);
        }

        public int CountDivisionManager(DivisionManagerKpiFilter filter)
        {
            return this._gridRepository.CountDivisionManager(filter);
        }

        public IEnumerable<KpiDTO> GetDivisionManagerKpiPaging(DivisionManagerKpiFilter filter)
        {
            return this._gridRepository.GetDivisionManagerKpiPaging(filter);
        }

        public bool GetIsKpiValidForDivManager(int kpiId, string userName)
        {
            return this._gridRepository.GetIsKpiValidForDivManager(kpiId, userName);
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgsByDivManager(string userName)
        {
            return this._gridRepository.GetOrgsByDivManager(userName);
        }

        public void ProcessRangeKpiLevel2Mng(IEnumerable<KpiDTO> kpiDtos)
        {
            var ts = this._gridRepository.FilterNoTasksKpi(kpiDtos);
            var taskKpi = ts.TaskKpiList;
            this._gridRepository.ProcessRangeKpiLevel2Mng(taskKpi);

            var noTasksKpi = ts.NoTaskKpiList;
            if (noTasksKpi != null && noTasksKpi.Any())
            {
                var error = new StringBuilder();
                error.Append("Không gửi được nhân sự bản ghi ");
                var kpiNames = noTasksKpi.Select(x => x.EmployeeName);
                var names = string.Join(", ", kpiNames);
                error.Append(names);
                error.Append(". Yêu cầu kiểm tra lại thông tin kế hoạch, nhiệm vụ đầu kỳ");
                throw new InvalidDataException(error.ToString());
            }
        }

        public void UpdateRangeKpiBusinessApplication(IEnumerable<KpiDTO> kpiDTOs)
        {
            foreach (var k in kpiDTOs)
            {
                k.ActionId = 5;
            }
            this._gridRepository.UpdateRangeKpiBusinessApplication(kpiDTOs);
        }

        public void UpdateRangeKpiHRManagerPropose(IEnumerable<KpiDTO> kpiDTOs)
        {
            foreach (var kpi in kpiDTOs)
            {
                kpi.ActionId = 6;
            }
            this._gridRepository.UpdateRangeKpiHRManagerPropose(kpiDTOs);
        }

        public IEnumerable<KpiDTO> GetHrManagerKpiPaging(DivisionManagerKpiFilter filter)
        {
            return this._gridRepository.GetHrManagerKpiPaging(filter);
        }

        public int CountHrManagerKpi(DivisionManagerKpiFilter filter)
        {
            return this._gridRepository.CountHrManagerKpi(filter);
        }
    }
}
