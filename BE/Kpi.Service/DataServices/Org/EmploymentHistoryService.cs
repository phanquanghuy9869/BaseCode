using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.Service.BaseServices;
using System.Collections.Generic;
using System;
using Kpi.DataAccess.DataContext;

namespace Kpi.Service.DataServices.Org
{
    public interface IEmploymentHistoryService : IGridService<EmploymentHistoryDTO, EmploymentHistoryFilterModelDTO>
    {
    }

    public class EmploymentHistoryService : BaseGridService<IEmploymentHistoryRepository, EmploymentHistoryDTO, EmploymentHistoryFilterModelDTO>, IEmploymentHistoryService
    {
        public EmploymentHistoryService(IEmploymentHistoryRepository gridRepository) : base(gridRepository)
        {
        }
    }
}
