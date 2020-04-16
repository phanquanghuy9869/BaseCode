using System.Collections.Generic;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;

namespace Kpi.DataAccess.Repo.EFRepos.Reports
{
    public interface IKpiPointEvaluationRepository : IGridRepository<KpiDTO, KpiPointEvaluationFilterModelDTO>
    {
    }

    public class KpiPointEvaluationRepository : BaseGridRepository<View_KpiPointEvaluation, KpiDTO, KpiPointEvaluationFilterModelDTO>, IKpiPointEvaluationRepository
    {

        public KpiPointEvaluationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        
        //public override KpiPointEvaluationDTO Get(int id)
        //{
        //    View_KpiPointEvaluation view_KpiPointEvaluation = _view_KpiPointEvaluationRepo.Get(x => x.Id == id);
        //    return AutoMapperHelper.Map<View_KpiPointEvaluation, KpiDTO>(view_KpiPointEvaluation);
        //}

        //public override IEnumerable<KpiPointEvaluationDTO> GetPaging(KpiPointEvaluationFilterModelDTO pagingModel)
        //{
        //    return base.GetPaging(pagingModel);
        //}
    }
}
