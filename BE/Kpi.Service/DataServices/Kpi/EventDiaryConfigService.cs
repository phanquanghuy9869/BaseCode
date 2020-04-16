using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Service.DataServices.Kpi
{
    public interface IEventDiaryConfigService : BaseServices.IGridService<EventDiaryConfigDTO, ViewEventDiaryConfigFilterModelDTO>
    {
        IEnumerable<EventDiaryConfigDTO> GetConfigOrgByLevel1Manager(string username);
        IEnumerable<EventDiaryConfigDTO> GetConfigOrgByLevel2Manager(string username);
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        IEnumerable<UserInfoModel> GetUsers();
        IEnumerable<EventDiaryConfigDTO> GetConfigOrgByKpiManager();
        IEnumerable<EventDiaryConfigDTO> GetConfigOrgByDivManager(string userName);
        IEnumerable<UserDTO> GetConfigLevel1Manager();
    }

    public class EventDiaryConfigService : BaseServices.BaseGridService<IEvenDiaryConfigRepository, EventDiaryConfigDTO, ViewEventDiaryConfigFilterModelDTO>, IEventDiaryConfigService
    {
        private readonly IEvenDiaryConfigRepository _repo;

        public EventDiaryConfigService(IEvenDiaryConfigRepository repo) : base(repo)
        {
            this._repo = repo;
        }

        public IEnumerable<EventDiaryConfigDTO> GetConfigOrgByLevel1Manager(string username)
        {
            return this._repo.GetConfigOrgByLevel1Manager(username);
        }

        public IEnumerable<EventDiaryConfigDTO> GetConfigOrgByDivManager(string username)
        {
            return this._repo.GetConfigOrgByDivManager(username);
        }

        public void Dispose()
        {
            if (this._repo != null && this._repo is IDisposable)
            {
                (this._repo as IDisposable).Dispose();
            }
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgs()
        {
            return this._gridRepository.GetOrgs();
        }

        public IEnumerable<UserInfoModel> GetUsers()
        {
            return this._gridRepository.GetUsers();
        }

        public IEnumerable<EventDiaryConfigDTO> GetConfigOrgByLevel2Manager(string username)
        {
            return this._repo.GetConfigOrgByLevel2Manager(username);
        }

        public IEnumerable<EventDiaryConfigDTO> GetConfigOrgByKpiManager()
        {
            return this._repo.GetConfigOrgByKpiManager();
        }

        public IEnumerable<UserDTO> GetConfigLevel1Manager()
        {
            return this._repo.GetConfigLevel1Manager();
        }
    }
}
