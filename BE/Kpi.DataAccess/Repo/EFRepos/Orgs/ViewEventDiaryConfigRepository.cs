using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;

namespace Kpi.DataAccess.Repo.EFRepos.Orgs
{

    public interface IViewEventDiaryConfigRepository : IGridRepository<ViewEventDiaryConfigDTO, ViewEventDiaryConfigFilterModelDTO>
    {
    }
    public class  ViewEventDiaryConfigRepository : BaseGridRepository<View_EventDiaryConfig, ViewEventDiaryConfigDTO, ViewEventDiaryConfigFilterModelDTO>, IViewEventDiaryConfigRepository
    {
        public ViewEventDiaryConfigRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override ViewEventDiaryConfigDTO Get(int id)
        {
            return base.Get(id);
        }

        public override IEnumerable<ViewEventDiaryConfigDTO> GetPaging(ViewEventDiaryConfigFilterModelDTO pagingModel)
        {
            return base.GetPaging(pagingModel);
        }
    }
}
