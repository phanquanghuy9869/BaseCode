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
    public interface IViewUserEventDiaryRepository : IGridRepository<ViewUserEventDiaryDTO, ViewUserEventDiaryFilterModelDTO>
    {
    }
    public class ViewUserEventDiaryRepository : BaseGridRepository<View_User_EventDiary, ViewUserEventDiaryDTO, ViewUserEventDiaryFilterModelDTO>, IViewUserEventDiaryRepository
    {
        public ViewUserEventDiaryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override ViewUserEventDiaryDTO Get(int id)
        {
            return base.Get(id);
        }

        public override IEnumerable<ViewUserEventDiaryDTO> GetPaging(ViewUserEventDiaryFilterModelDTO pagingModel)
        {
            return base.GetPaging(pagingModel);
        }
    }
}
