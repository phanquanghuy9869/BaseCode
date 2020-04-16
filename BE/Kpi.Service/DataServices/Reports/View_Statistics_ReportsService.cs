using System;
using System.Collections.Generic;
using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Reports;

namespace Kpi.Service.DataServices.Reports
{
    public interface IView_Statistics_ReportsService
    {
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        IEnumerable<View_Statistics_ReportsDTO> Search(View_Statistics_ReportsFilterModelDTO filter);
        IEnumerable<View_Statistics_ReportsDTO> Search1(View_Statistics_ReportsFilterModelDTO filter);
    }
    public class View_Statistics_ReportsService : IView_Statistics_ReportsService
    {
        private IView_Statistics_ReportsRepository _repository;
        public View_Statistics_ReportsService(IView_Statistics_ReportsRepository gridRepository)
        {
            _repository = gridRepository;
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgs()
        {
            return this._repository.GetOrgs();
        }

        public IEnumerable<View_Statistics_ReportsDTO> Search(View_Statistics_ReportsFilterModelDTO filter)
        {
            return this._repository.Search(filter);
        }
        public IEnumerable<View_Statistics_ReportsDTO> Search1(View_Statistics_ReportsFilterModelDTO filter)
        {
            return this._repository.Search1(filter);
        }
    }
}
