using System.Collections.Generic;
using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Kpi;

namespace Kpi.Service.DataServices.Kpi
{
    public interface IUnLockDiaryCriterionService
    {
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        IEnumerable<UnLockDiaryCriterionDTO> Search(UnLockDiaryCriterionFilterModelDTO filter);
    }
    public class UnLockDiaryCriterionService : IUnLockDiaryCriterionService
    {
        private IUnLockDiaryCriterionRepository _repository;
        public UnLockDiaryCriterionService(IUnLockDiaryCriterionRepository gridRepository)
        {
            _repository = gridRepository;
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgs()
        {
            return this._repository.GetOrgs();
        }

        public IEnumerable<UnLockDiaryCriterionDTO> Search(UnLockDiaryCriterionFilterModelDTO filter)
        {
            return this._repository.Search(filter);
        }
    }
}
