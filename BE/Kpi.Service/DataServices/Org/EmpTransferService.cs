using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.Service.BaseServices;
using System.Collections.Generic;
using System;
using Kpi.DataAccess.DataContext;

namespace Kpi.Service.DataServices.Org
{
    public interface IEmpTransferService : IGridService<EmpTransferDTO, EmpTransferFilterModelDTO>
    {
        IEnumerable<Org_Organization> GetAllOrgs();
    }

    public class EmpTransferService : BaseGridService<IEmpTransferRepository, EmpTransferDTO, EmpTransferFilterModelDTO>, IEmpTransferService
    {
        public EmpTransferService(IEmpTransferRepository gridRepository) : base(gridRepository)
        {
        }

        public IEnumerable<Org_Organization> GetAllOrgs()
        {
            return this._gridRepository.GetAllOrgs();
        }
    }
}
