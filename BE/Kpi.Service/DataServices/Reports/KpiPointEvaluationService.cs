using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Reports;
using Kpi.Service.BaseServices;

namespace Kpi.Service.DataServices.Reports
{
    public interface IKpiPointEvaluationService : IGridService<KpiDTO, KpiPointEvaluationFilterModelDTO>
    {
    }

    public class KpiPointEvaluationService : BaseGridService<IKpiPointEvaluationRepository, KpiDTO, KpiPointEvaluationFilterModelDTO>, IKpiPointEvaluationService
    {
        public KpiPointEvaluationService(IKpiPointEvaluationRepository gridRepository) : base(gridRepository)
        {
        }

        public void CreateKpiGlobal(KpiPeriodConfigDTO config)
        {
        }
    }
}
