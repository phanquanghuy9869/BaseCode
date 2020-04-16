using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.Service.BaseServices;
using System.Collections.Generic;
using System;
using Kpi.DataAccess.DataContext;

namespace Kpi.Service.DataServices.Org
{
    public interface IOrgService : IGridService<OrganizationDTO, OrgPagingFilterModelDTO>
    {
        IEnumerable<OrganizationTypeDTO> GetOrgTypes();
    }

    public class OrgService : BaseGridService<IOrgRepository, OrganizationDTO, OrgPagingFilterModelDTO>, IOrgService
    {
        public OrgService(IOrgRepository gridRepository) : base(gridRepository)
        {
        }

        public IEnumerable<OrganizationTypeDTO> GetOrgTypes()
        {
            return this._gridRepository.GetOrgTypes();
        }
    }
}
