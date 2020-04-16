using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.Service.BaseServices;
using System.Collections.Generic;
using System;
using Kpi.DataAccess.DataContext;

namespace Kpi.Service.DataServices.Org
{
    public interface IUserOrgService : IGridService<UserInfoModel, OrgInfoModelFilterModelDTO>
    {
        UserInfoModel Get(string username);
        IEnumerable<UserOrgDTO> GetEDUsersByOrg(int orgId);
        IEnumerable<UserOrgDTO> GetEDUsersByEvent(int eventDiaryId);
        IEnumerable<UserOrgDTO> GetEDUsersByEvents(List<int> eventDiaryIds);
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        IEnumerable<OrgInfoModelDTO> GetAllOrgs();
        IEnumerable<UserInfoModel> GetUsers();
        IEnumerable<UserInfoModel> GetAllUsers();
        IEnumerable<Org_JobTitleDTO> GetJobTitles();
        bool AddUser(UserOrgDTO model);
        bool AddUserNoTrans(UserOrgDTO model);
        bool UpdateUser(UserOrgDTO model);
        IEnumerable<AspNetRole> GetRoles();
        OrganizationDTO GetOrgByUser(string username);
        bool UpdateUserNoTrans(UserOrgDTO entity);
        IEnumerable<OrgInfoModelDTO> GetOrgsByLevel2Manager(string username);
        IEnumerable<OrgInfoModelDTO> GetOrgsByLevel1Manager(string username);
        IEnumerable<UserOrgDTO> GetLevel1ManagerUserOrg();
        IEnumerable<UserOrgDTO> GetLevel2ManagerUserOrg();
        IEnumerable<Org_JobTitleDTO> GetAllJobTitles();
        bool DeleteUser(UserOrgDTO entity);
        IEnumerable<UserInfoModel> SearchPagingDivisionManager(UserOrgFilterModelDTO model, List<string> users);
        void SaveDivisionManagerPermission(DivMngPerUser model);
        DivMngPerUser GetDivisionManagerPermission(int userId);
        bool IsVIP(string username);
        UserOrgDTO GetLevel1ManagerByUserName(string userName, int yearMonth);
    }

    public class UserOrgService : BaseGridService<IOrgUserRepository, UserInfoModel, OrgInfoModelFilterModelDTO>, IUserOrgService
    {
        public UserOrgService(IOrgUserRepository gridRepository) : base(gridRepository)
        {
        }

        public UserInfoModel Get(string username)
        {
            return this._gridRepository.Get(username);
        }

        public IEnumerable<UserOrgDTO> GetEDUsersByOrg(int orgId)
        {
            return this._gridRepository.GetEDUsersByOrg(orgId);
        }

        public IEnumerable<Org_JobTitleDTO> GetJobTitles()
        {
            return _gridRepository.GetJobTitles();
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgs()
        {
            return this._gridRepository.GetOrgs();
        }

        public IEnumerable<UserInfoModel> GetUsers()
        {
            return _gridRepository.GetUsers();
        }

        public bool AddUser(UserOrgDTO model)
        {
            return _gridRepository.AddUser(model);
        }
        public bool AddUserNoTrans(UserOrgDTO model)
        {
            return _gridRepository.AddUserNoTrans(model);
        }

        public bool UpdateUser(UserOrgDTO model)
        {
            return _gridRepository.UpdateUser(model);
        }

        public IEnumerable<AspNetRole> GetRoles()
        {
            return _gridRepository.GetRoles();
        }

        public OrganizationDTO GetOrgByUser(string username)
        {
            return this._gridRepository.GetOrgByUser(username);
        }

        public IEnumerable<UserOrgDTO> GetEDUsersByEvent(int eventDiaryId)
        {
            return this._gridRepository.GetEDUsersByEvent(eventDiaryId);
        }

        public IEnumerable<UserOrgDTO> GetEDUsersByEvents(List<int> eventDiaryIds)
        {
            return this._gridRepository.GetEDUsersByEvents(eventDiaryIds);
        }

        public bool UpdateUserNoTrans(UserOrgDTO entity)
        {
            if (entity.FirstKpiDateYM != null && entity.FirstKpiDateYM > 0)
            {
                entity.FirstKpiDate = DateTime.ParseExact(entity.FirstKpiDateYM.ToString(), "yyyyMM",
                                       System.Globalization.CultureInfo.InvariantCulture);
                entity.EDCUpdateDate = DateTime.Now;
            }
            return _gridRepository.UpdateUserNoTrans(entity);
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgsByLevel2Manager(string username)
        {
            return this._gridRepository.GetOrgsByLevel2Manager(username);
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgsByLevel1Manager(string username)
        {
            return this._gridRepository.GetOrgsByLevel1Manager(username);
        }

        public IEnumerable<UserOrgDTO> GetLevel1ManagerUserOrg()
        {
            return this._gridRepository.GetLevel1ManagerUserOrg();
        }

        public IEnumerable<UserOrgDTO> GetLevel2ManagerUserOrg()
        {
            return this._gridRepository.GetLevel2ManagerUserOrg();
        }

        public IEnumerable<OrgInfoModelDTO> GetAllOrgs()
        {
            return this._gridRepository.GetAllOrgs();
        }

        public IEnumerable<UserInfoModel> GetAllUsers()
        {
            return this._gridRepository.GetAllUsers();
        }

        public IEnumerable<Org_JobTitleDTO> GetAllJobTitles()
        {
            return this._gridRepository.GetAllJobTitles();
        }

        public bool DeleteUser(UserOrgDTO entity)
        {
            return this._gridRepository.DeleteUser(entity);
        }

        public IEnumerable<UserInfoModel> SearchPagingDivisionManager(UserOrgFilterModelDTO model, List<string> users)
        {
            return this._gridRepository.SearchPagingDivisionManager(model, users);
        }

        public void SaveDivisionManagerPermission(DivMngPerUser model)
        {
            this._gridRepository.saveDivisionManagerPermission(model);
        }

        public DivMngPerUser GetDivisionManagerPermission(int userId)
        {
            return this._gridRepository.GetDivisionManagerPermission(userId);
        }

        public bool IsVIP(string username)
        {
            return this._gridRepository.IsVIP(username);
        }

        public UserOrgDTO GetLevel1ManagerByUserName(string userName, int yearMonth)
        {
            return this._gridRepository.GetLevel1ManagerByUserName(userName, yearMonth);
        }
    }
}
