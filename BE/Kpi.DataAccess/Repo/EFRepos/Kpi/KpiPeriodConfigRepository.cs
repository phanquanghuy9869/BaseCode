using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using Kpi.Core.Helper;
using System.Linq.Expressions;

namespace Kpi.DataAccess.Repo.EFRepos.Kpi
{
    public interface IKpiPeriodConfigRepository : IGridRepository<KpiPeriodConfigDTO, KpiPeriodConfigFilterModelDTO>
    {
        //    IEnumerable<EventDiaryConfigDTO> GetConfigOrgByLevel1Manager(string username);
        void UpdateKPIByYearMonthEvent(UpdateKPIByYearMonthEventFilter model);
    }
    public class KpiPeriodConfigRepository : BaseGridRepository<Sys_KpiPeriodConfig, KpiPeriodConfigDTO, KpiPeriodConfigFilterModelDTO>, IKpiPeriodConfigRepository
    {
        protected IGenericRepository<Sys_EventDiaryConfig> _edcRepo;

        public KpiPeriodConfigRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _edcRepo = unitOfWork.GetDataRepository<Sys_EventDiaryConfig>();
        }

        public override IEnumerable<KpiPeriodConfigDTO> GetPaging(KpiPeriodConfigFilterModelDTO filter)
        {
            // get config 
            var query = this._repo.SelectWhere(x => (x.FromDate >= filter.FromDate || filter.FromDate == null) && (x.ToDate <= filter.ToDate || filter.FromDate == null)
            && (x.PeriodConfig == filter.PeriodConfig || filter.PeriodConfig == null || filter.PeriodConfig.Trim() == ""));

            var test = query.ToList();
            return query.OrderByDescending(x => x.YearMonth).Skip(filter.Start - 1).Take(filter.Length).Select(x =>
            new KpiPeriodConfigDTO
            {
                Id = x.Id,
                Code = x.Code,
                DayStart = x.DayStart,
                DayEnd = x.DayEnd,
                DaySendEvalation = x.DaySendEvalation,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                Note = x.Note,
                PeriodConfig = x.PeriodConfig,
                YearMonth = x.YearMonth.HasValue ? x.YearMonth.Value : 0
            }).ToList();
        }

        public KpiPeriodConfigDTO Add(KpiPeriodConfigDTO dto)
        {
            var entity = AutoMapperHelper.Map<KpiPeriodConfigDTO, Sys_KpiPeriodConfig>(dto);
            var kpi = _repo.SelectWhere(k => k.YearMonth == dto.YearMonth).FirstOrDefault();
            if (kpi != null)
            {
                var yearMonthstr = "";
                try
                {
                    yearMonthstr = dto.YearMonth.ToString();
                    yearMonthstr = dto.YearMonth.ToString().Substring(4) + "/" + dto.YearMonth.ToString().Substring(0, 4);
                }
                catch (Exception)
                {
                }
                throw new Exception("Kỳ đánh giá đã tồn tại");
            }


            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                throw new Exception("Chưa nhập mã");
            }

            if (string.IsNullOrWhiteSpace(dto.PeriodConfig))
            {
                throw new Exception("Chưa nhập kỳ đánh giá");
            }

            // khong cho ngay bat dau truoc ngay ket thuc
            if (dto.FromDate.Date >= dto.ToDate.Date)
            {
                throw new Exception("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
            }

            // khong cho ngay bat dau truoc ngay ket thuc
            if (dto.FromDate.Date >= dto.ToDate.Date)
            {
                throw new Exception("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
            }
            
            dto.MonthNumber = dto.ToDate.Month;
            IKpiEvaluationRepository kpiRepo = new KpiEvaluationRepository(_unitOfWork);

            kpiRepo.CreateKpiGlobal(dto);

            //using (var transaction = _unitOfWork.BeginTransaction())
            //{
            //    // add kpi period config
            //    entity = _repo.Add(entity);
            //    _unitOfWork.SaveChanges();
            //    transaction.Commit();
            //}

            return AutoMapperHelper.Map<Sys_KpiPeriodConfig, KpiPeriodConfigDTO>(entity);

        }

        public override void Update(KpiPeriodConfigDTO entityDto)
        {
            var kpi = _repo.SelectWhere(k => k.YearMonth == entityDto.YearMonth && k.Id != entityDto.Id).FirstOrDefault();
            if (kpi != null)
            {
                var yearMonthstr = "";
                try
                {
                    yearMonthstr = entityDto.YearMonth.ToString();
                    yearMonthstr = entityDto.YearMonth.ToString().Substring(4) + "/" + entityDto.YearMonth.ToString().Substring(0, 4);
                }
                catch (Exception)
                {
                }
                throw new Exception("Kỳ đánh giá đã tồn tại");
            }


            var obj = _repo.SelectWhere(k => k.Id == entityDto.Id).FirstOrDefault();
            obj.Code = entityDto.Code;
            obj.PeriodConfig = entityDto.PeriodConfig;
            obj.DaySendEvalation = entityDto.DaySendEvalation;
            obj.YearMonth = entityDto.YearMonth;
            obj.FromDate = entityDto.FromDate;
            obj.ToDate = entityDto.ToDate;
            obj.Note = entityDto.Note;
            this._repo.UpdateByProperties(obj,
                new List<Expression<Func<Sys_KpiPeriodConfig, object>>> { x => x.Code, x => x.PeriodConfig, x => x.DaySendEvalation, x => x.YearMonth, x => x.FromDate, x => x.ToDate
                , x => x.Note});

            _unitOfWork.SaveChanges();
        }

        public override int Count(KpiPeriodConfigFilterModelDTO filter)
        {
            return this._repo.SelectWhere(x => (x.FromDate >= filter.FromDate || filter.FromDate == null) && (x.ToDate <= filter.ToDate || filter.FromDate == null)
            && (x.PeriodConfig == filter.PeriodConfig || filter.PeriodConfig == null || filter.PeriodConfig.Trim() == "")).Count();
        }

        public void UpdateKPIByYearMonthEvent(UpdateKPIByYearMonthEventFilter filter)
        {
            // lay cau hinh nksk
            var eDiary = _edcRepo.SelectWhere(c => c.Id == filter.EventDiaryConfigId).FirstOrDefault();
            if (eDiary == null)
            {
                throw new Exception("Không tìm thấy cấu hình NKSK.");
            }

            // lay danh sach ky >= thang-nam da chon
            var pConfigs = this._repo.SelectWhere(p => p.YearMonth >= filter.YearMonth).ToList();

            if (pConfigs.Count > 0)
            {
                foreach (var config in pConfigs)
                {
                    // lay danh sach kpi

                }
            }
        }
    }
}
