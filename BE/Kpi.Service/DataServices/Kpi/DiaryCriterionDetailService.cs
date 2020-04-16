using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Kpi;
using Kpi.Service.BaseServices;

namespace Kpi.Service.DataServices.Kpi
{
    public interface IDiaryCriterionDetailService : IGridService<DiaryCriterionDetailDTO, DiaryCriterionDetailFilterModelDTO>
    {
        void RemoveByManagerLv2(DiaryCriterionDetailDTO filter, string userName, bool isHrOrHrManager);
        void UpdateKpiCriterionPointByYearMonth(int yearMonth);
        void UpdateKpiCriterionPointByKpiId(int id);
    }
    public class DiaryCriterionDetailService : BaseGridService<IDiaryCriterionDetailRepository, DiaryCriterionDetailDTO, DiaryCriterionDetailFilterModelDTO>, IDiaryCriterionDetailService
    {
        public DiaryCriterionDetailService(IDiaryCriterionDetailRepository gridRepository) : base(gridRepository)
        {
        }

        public override DiaryCriterionDetailDTO Add(DiaryCriterionDetailDTO entityDTo)
        {
            return base.Add(entityDTo);
        }

        public override void Delete(DiaryCriterionDetailDTO entityDto)
        {
            base.Delete(entityDto);
        }

        public override DiaryCriterionDetailDTO Get(int id)
        {
            return base.Get(id);
        }

        public override IEnumerable<DiaryCriterionDetailDTO> GetPaging(DiaryCriterionDetailFilterModelDTO pagingModel)
        {
            return base.GetPaging(pagingModel);
        }

        public void RemoveByManagerLv2(DiaryCriterionDetailDTO filter, string userName, bool isHrOrHrManager)
        {
            this._gridRepository.RemoveByManagerLv2(filter, userName, isHrOrHrManager);
        }

        public override void Update(DiaryCriterionDetailDTO entityDto)
        {
            base.Update(entityDto);
        }

        public void UpdateKpiCriterionPointByYearMonth(int yearMonth)
        {
            this._gridRepository.UpdateKpiCriterionPointByYearMonth(yearMonth);
        }

        public void UpdateKpiCriterionPointByKpiId(int id)
        {
            this._gridRepository.UpdateKpiCriterionPointByKpiId(id);
        }
        
    }
}
