using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.Service.BaseServices;

namespace Kpi.Service.DataServices.Orgs
{
    public interface IViewEventDiaryConfigService : IGridService<ViewEventDiaryConfigDTO, ViewEventDiaryConfigFilterModelDTO>
    {

    }
    public class ViewEventDiaryConfigService : BaseGridService<IViewEventDiaryConfigRepository, ViewEventDiaryConfigDTO, ViewEventDiaryConfigFilterModelDTO>, IViewEventDiaryConfigService
    {
        public ViewEventDiaryConfigService(IViewEventDiaryConfigRepository gridRepository) : base(gridRepository)
        {
        }
    }
}
