using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Reports;
using Kpi.Service.BaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Service.DataServices.Reports
{
    public interface IView_KpiEvaluation_OrganizationService
    {
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        IEnumerable<View_KpiEvaluation_OrganizationDTO> Search(View_KpiEvaluation_OrganizationFilterModelDTO filter);
    }
    public class View_KpiEvaluation_OrganizationService: IView_KpiEvaluation_OrganizationService
    {
        private IView_KpiEvaluation_OrganizationRepository _repository;
        public View_KpiEvaluation_OrganizationService(IView_KpiEvaluation_OrganizationRepository gridRepository)
        {
            _repository = gridRepository;
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgs()
        {
            return this._repository.GetOrgs();
        }

        public IEnumerable<View_KpiEvaluation_OrganizationDTO> Search(View_KpiEvaluation_OrganizationFilterModelDTO filter)
        {
            return this._repository.Search(filter);
        }
    }
}
