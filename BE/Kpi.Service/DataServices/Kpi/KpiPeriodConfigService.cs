using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Kpi;

namespace Kpi.Service.DataServices.Kpi
{
    public interface IKpiPeriodConfigService : BaseServices.IGridService<KpiPeriodConfigDTO, KpiPeriodConfigFilterModelDTO>
    {
        void UpdateKPIByYearMonthEvent(UpdateKPIByYearMonthEventFilter model);
    }
    public class KpiPeriodConfigService : BaseServices.BaseGridService<IKpiPeriodConfigRepository, KpiPeriodConfigDTO, KpiPeriodConfigFilterModelDTO>, IKpiPeriodConfigService
    {
        public KpiPeriodConfigService(IKpiPeriodConfigRepository gridRepository) : base(gridRepository)
        {
        }

        public void UpdateKPIByYearMonthEvent(UpdateKPIByYearMonthEventFilter model)
        {
            this._gridRepository.UpdateKPIByYearMonthEvent(model);
        }
    }
}
