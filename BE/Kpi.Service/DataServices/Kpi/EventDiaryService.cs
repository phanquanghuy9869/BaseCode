using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Service.DataServices.Kpi
{
    public interface IEventDiaryService
    {
        EventDiaryDTO GetEventDiaryByManager(int yearMonth, int? orgId, string level1ManagerUserName, string empName);
        EventDiaryDTO GetEventDiaryByKpiManager(int yearMonth, int? orgId, string level1ManagerUserName, string empName);

        EventDiaryDTO GetMonthlyEventDiaryByHRManager(int yearMonth, int? orgId, string level1ManagerUserName, string empUserName);
        EventDiaryDTO GetEventDiaryByManagerLv2(int yearMonth, int? orgId, string level1ManagerUserName, string level2ManagerUserName, string empName);
        EventDiaryDTO GetMonthlyEventDiaryByEmp(int yearMonth, int? orgId, string empUserName);
        EventDiaryDTO GetMonthlyEventDiaryByKpiId(int kpiId);
        EventDiaryDTO GetEventDiaryByManagerLv2(int yearMonth, int? orgId, string userName);
        EventDiaryDTO GetEventDiaryByKpiManager(int yearMonth, int? orgId);
        EventDiaryPagingModelDTO GetPaging(EventDiaryFilterModelDTO filter);
        IEnumerable<UserOrgDTO> GetLevel1UserByLevel2Manager(string userName, int? orgId);
        PagingModelDTO GetPagingExistedEventDiaryByYearMonth(EventDiaryFilterModelDTO filter);
        PagingModelDTO GetPagingMissingEventDiaryByYearMonth(EventDiaryFilterModelDTO filter);
        IEnumerable<UserOrgDTO> GetUsers();
        EventDiaryDTO GetEventByDivManager(int yearMonth, int? orgId, string level1ManagerUserName, string empName, string userName);
        void AddRangeDiaryKpi(IEnumerable<EventDiarySyncModelDTO> configs, string currentUser);
        void UpdateRangeDiaryKpiConfigChanged(IEnumerable<EventDiarySyncModelDTO> configs, string currentUser);
        void CompleteNotify(string userName, int evntId);
    }

    public class EventDiaryService : IEventDiaryService, IDisposable
    {
        private readonly IEventDiaryRepository _repo;

        public EventDiaryService(IEventDiaryRepository repo)
        {
            this._repo = repo;
        }

        public EventDiaryDTO GetEventDiaryByManager(int yearMonth, int? orgId, string level1ManagerUserName, string empName)
        {
            return _repo.GetMonthlyEventDiaryByManager(yearMonth, orgId, level1ManagerUserName, empName);
        }
        public EventDiaryDTO GetEventDiaryByKpiManager(int yearMonth, int? orgId, string level1ManagerUserName, string empName)
        {
            return _repo.GetEventDiaryByKpiManager(yearMonth, orgId, level1ManagerUserName, empName);
        }

        public EventDiaryDTO GetMonthlyEventDiaryByHRManager(int yearMonth, int? orgId, string level1ManagerUserName, string empUserName)
        {
            return _repo.GetMonthlyEventDiaryByHRManager(yearMonth, orgId, level1ManagerUserName, empUserName);
        }

        public EventDiaryDTO GetEventDiaryByManagerLv2(int yearMonth, int? orgId, string level1ManagerUserName, string level2ManagerUserName, string empName)
        {
            return _repo.GetEventDiaryByManagerLv2(yearMonth, orgId, level1ManagerUserName, level2ManagerUserName , empName);
        }

        public void Dispose()
        {
            if (_repo != null && _repo is IDisposable)
            {
                (this._repo as IDisposable).Dispose();
            }
        }

        public EventDiaryDTO GetMonthlyEventDiaryByEmp(int yearMonth, int? orgId, string empUserName)
        {
            return _repo.GetMonthlyEventDiaryByEmp(yearMonth, orgId, empUserName);
        }

        public EventDiaryDTO GetMonthlyEventDiaryByKpiId(int kpiId)
        {
            return _repo.GetMonthlyEventDiaryByKpiId(kpiId);
        }

        public EventDiaryDTO GetEventDiaryByManagerLv2(int yearMonth, int? orgId, string userName)
        {
            return _repo.GetMonthlyEventDiaryByManagerLv2(yearMonth, orgId, userName);
        }

        public EventDiaryDTO GetEventDiaryByKpiManager(int yearMonth, int? orgId)
        {
            return _repo.GetEventDiaryByKpiManager(yearMonth, orgId);
        }

        public EventDiaryPagingModelDTO GetPaging(EventDiaryFilterModelDTO filter)
        {
            return _repo.GetPaging(filter);
        }

        public IEnumerable<UserOrgDTO> GetLevel1UserByLevel2Manager(string userName, int? orgId)
        {
            return _repo.GetLevel1UserByLevel2Manager(userName, orgId);
        }

        public PagingModelDTO GetPagingExistedEventDiaryByYearMonth(EventDiaryFilterModelDTO filter)
        {
            return this._repo.GetPagingExistedEventDiaryByYearMonth(filter);
        }

        public PagingModelDTO GetPagingMissingEventDiaryByYearMonth(EventDiaryFilterModelDTO filter)
        {
            return this._repo.GetPagingMissingEventDiaryByYearMonth(filter);
        }

        public IEnumerable<UserOrgDTO> GetUsers()
        {
            return _repo.GetUsers();
        }

        public EventDiaryDTO GetEventByDivManager(int yearMonth, int? orgId, string level1ManagerUserName, string empName, string userName)
        {
            return this._repo.GetEventByDivManager(yearMonth, orgId, level1ManagerUserName, empName, userName);
        }

        public void AddRangeDiaryKpi(IEnumerable<EventDiarySyncModelDTO> configs, string currentUser)
        {
            this._repo.AddRangeDiaryKpi(configs, currentUser);
        }

        public void UpdateRangeDiaryKpiConfigChanged(IEnumerable<EventDiarySyncModelDTO> configs, string currentUser)
        {
            this._repo.UpdateRangeDiaryKpiConfigChanged(configs, currentUser);
        }

        public void CompleteNotify(string userName, int evntId)
        {
            this._repo.CompleteNotify(userName, evntId);
        }
    }
}
