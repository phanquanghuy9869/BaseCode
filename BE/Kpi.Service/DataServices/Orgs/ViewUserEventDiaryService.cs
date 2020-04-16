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
    public interface IViewUserEventDiaryService : IGridService<ViewUserEventDiaryDTO, ViewUserEventDiaryFilterModelDTO>
    {

    }
    public class ViewUserEventDiaryService : BaseGridService<IViewUserEventDiaryRepository, ViewUserEventDiaryDTO, ViewUserEventDiaryFilterModelDTO>, IViewUserEventDiaryService
    {
        public ViewUserEventDiaryService(IViewUserEventDiaryRepository gridRepository) : base(gridRepository)
        {
        }
    }
}
