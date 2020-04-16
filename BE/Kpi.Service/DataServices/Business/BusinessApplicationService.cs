using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Service.DataServices.Business
{
    public interface IBusinessApplicationService
    {
        ProcessStatusDTO GetFirstKpiProcess();
    }

    public class BusinessApplicationService : IBusinessApplicationService
    {
        private readonly IBusinessApplicationRepository _repo;

        public BusinessApplicationService(IBusinessApplicationRepository repo)
        {
            this._repo = repo;
        }

        public ProcessStatusDTO GetFirstKpiProcess()
        {
            return this._repo.GetFirstKpiProcess();
        }
    }
}
