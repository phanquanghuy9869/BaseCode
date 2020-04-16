using Kpi.Api.Controllers.Base;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Kpi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Kpi.Api.Controllers.Kpi
{
    public class EventDiaryFilterRS
    {
        public int YearMonth { get; set; }
        public int? OrgId { get; set; }
        public int KpiId { get; set; }
        public string MngUserName { get; set; }
        public string EmployeeName { get; set; }
    }


    //[Authorize]
    public class EventDiaryController : BaseApiController<IEventDiaryService>
    {
        public EventDiaryController(IEventDiaryService service) : base(service)
        {
        }

        [HttpPost]
        public RespondData GetEventDiaryByManager(EventDiaryFilterRS filter)
        {
            return Success(this._service.GetEventDiaryByManager(filter.YearMonth, filter.OrgId, this.AppUser.Name, filter.EmployeeName));
        }

        /// <summary>
        /// Lấy event diary của manager level 2 . nhưng dữ liệu truyền lên username là của các manager level 1 thuộc manager lv 2 đó
        /// </summary>
        [HttpPost]
        public RespondData GetEventDiaryByManagerLv2(EventDiaryFilterRS filter)
        {
            return Success(this._service.GetEventDiaryByManagerLv2(filter.YearMonth, filter.OrgId, filter.MngUserName, this.UserName, filter.EmployeeName));
        }

        [HttpPost]
        public RespondData GetEventDiaryByKpiManager(EventDiaryFilterRS filter)
        {
            return Success(this._service.GetEventDiaryByKpiManager(filter.YearMonth, filter.OrgId, filter.MngUserName, filter.EmployeeName));
        }

        [HttpPost]
        public RespondData CompleteNotify(EventDiaryFilterRS filter)
        {
            this._service.CompleteNotify(this.UserName, filter.KpiId);
            return Success(null);
        }

        [HttpPost]
        public RespondData GetMonthlyEventDiaryByHRManager(EventDiaryFilterRS filter)
        {
            return Success(this._service.GetMonthlyEventDiaryByHRManager(filter.YearMonth, filter.OrgId, filter.MngUserName, filter.EmployeeName));
        }

        [HttpPost]
        public RespondData GetMonthlyEventDiaryByEmp(EventDiaryFilterRS filter)
        {
            return Success(this._service.GetMonthlyEventDiaryByEmp(filter.YearMonth, filter.OrgId, this.UserName));
        }

        [HttpPost]
        public RespondData GetMonthlyEventDiaryByKpiId(EventDiaryFilterRS filter)
        {
            return Success(this._service.GetMonthlyEventDiaryByKpiId(filter.KpiId));
        }

        [HttpPost]
        public RespondData GetPaging(EventDiaryFilterModelDTO filter)
        {
            return Success(this._service.GetPaging(filter));
        }

        [HttpPost]
        public RespondData GetLevel1UserByLevel2Manager(EventDiaryFilterRS filter)
        {
            return Success(this._service.GetLevel1UserByLevel2Manager(this.UserName, filter.OrgId));
        }

        [HttpPost]
        public RespondData GetPagingExistedEventDiaryByYearMonth(EventDiaryFilterModelDTO filter)
        {
            return Success(this._service.GetPagingExistedEventDiaryByYearMonth(filter));
        }

        [HttpPost]
        public RespondData GetPagingMissingEventDiaryByYearMonth(EventDiaryFilterModelDTO filter)
        {
            return Success(this._service.GetPagingMissingEventDiaryByYearMonth(filter));
        }

        [HttpPost]
        public RespondData GetUsers()
        {
            return Success(this._service.GetUsers());
        }

        [HttpPost]
        public RespondData GetEventByDivManager(EventDiaryFilterRS filter)
        {
            return Success(this._service.GetEventByDivManager(filter.YearMonth, filter.OrgId, filter.MngUserName, filter.EmployeeName, this.UserName));
        }

        [HttpPost]
        public RespondData AddRangeDiaryKpi(List<EventDiarySyncModelDTO> configs)
        {
            //var config1 = new EventDiarySyncModelDTO()
            //{
            //    OrgId = 2,
            //    OrgName = "PHÒNG PHÁT TRIỂN PHẦN MỀM (TRUNG TÂM CÔNG NGHỆ THÔNG TIN )",
            //    Level1ManagerUserName = "anh.nt2",
            //    Level1ManagerFullName = "Nguyễn Thế Anh",
            //    Level2ManagerUserName = "viet.nh",
            //    Level2ManagerFullName = "Nguyễn Hồng Việt",
            //    FromDate = new DateTime(2020, 1, 25),
            //    ToDate = new DateTime(2020, 2, 25),
            //    YearMonth = 202002,
            //    EventDiaryConfigId = 1
            //};

            //var config2 = new EventDiarySyncModelDTO()
            //{
            //    OrgId = 3,
            //    OrgName = "TRUNG TÂM CÔNG NGHỆ THÔNG TIN  (BRG GROUP)",
            //    Level1ManagerUserName = "viet.nh",
            //    Level1ManagerFullName = "Nguyễn Hồng Việt",
            //    Level2ManagerUserName = "viet.nh",
            //    Level2ManagerFullName = "Nguyễn Hồng Việt",
            //    FromDate = new DateTime(2020, 1, 25),
            //    ToDate = new DateTime(2020, 2, 25),
            //    YearMonth = 202002,
            //    EventDiaryConfigId = 15
            //};
            //this._service.AddRangeDiaryKpi(new List<EventDiarySyncModelDTO>() { config1 , config2 }, this.UserName);
            var ym = configs.FirstOrDefault().YearMonth;
            var kpiDate = DateTime.ParseExact(ym.ToString(), "yyyyMM", CultureInfo.InvariantCulture);
            var fromDate = kpiDate.AddMonths(-1);
            fromDate = new DateTime(fromDate.Year, fromDate.Month, 25);
            var toDate = new DateTime(kpiDate.Year, kpiDate.Month, 25);
            for (int i = 0; i < configs.Count; i++)
            {
                configs[i].YearMonth = ym;
                configs[i].FromDate = fromDate;
                configs[i].ToDate = toDate;
            }
            this._service.AddRangeDiaryKpi(configs, this.UserName);
            return DefaultSuccess(string.Empty);
        }

        [HttpPost]
        public RespondData UpdateRangeDiaryKpiConfigChanged(IEnumerable<EventDiarySyncModelDTO> configs)
        {
            //var config = new EventDiarySyncModelDTO()
            //{
            //    Id = 1,
            //    OrgId = 2,
            //    OrgName = "PHÒNG PHÁT TRIỂN PHẦN MỀM (TRUNG TÂM CÔNG NGHỆ THÔNG TIN )",
            //    Level1ManagerUserName = "anh.nt2",
            //    Level1ManagerFullName = "Nguyễn Thế Anh",
            //    Level2ManagerUserName = "viet.nh",
            //    Level2ManagerFullName = "Nguyễn Hồng Việt",
            //    FromDate = new DateTime(2019, 12, 25),
            //    ToDate = new DateTime(2020, 1, 25),
            //    YearMonth = 202001,
            //    EventDiaryConfigId = 1
            //};

            //var config = new EventDiarySyncModelDTO()
            //{
            //    Id = 4,
            //    OrgId = 3,
            //    OrgName = "TRUNG TÂM CÔNG NGHỆ THÔNG TIN (BRG GROUP)",
            //    Level1ManagerUserName = "viet.nh",
            //    Level1ManagerFullName = "Nguyễn Hồng Việt",
            //    Level2ManagerUserName = "viet.nh",
            //    Level2ManagerFullName = "Nguyễn Hồng Việt",
            //    FromDate = new DateTime(2019, 12, 25),
            //    ToDate = new DateTime(2020, 1, 25),
            //    YearMonth = 202002,
            //    EventDiaryConfigId = 15
            //};

            //var config = new EventDiarySyncModelDTO()
            //{
            //    Id = 4,
            //    OrgId = 3,
            //    OrgName = "TRUNG TÂM CÔNG NGHỆ THÔNG TIN (BRG GROUP)",
            //    Level1ManagerUserName = "viet.nh",
            //    Level1ManagerFullName = "Nguyễn Hồng Việt",
            //    Level2ManagerUserName = "viet.nh",
            //    Level2ManagerFullName = "Nguyễn Hồng Việt",
            //    FromDate = new DateTime(2019, 12, 25),
            //    ToDate = new DateTime(2020, 1, 25),
            //    YearMonth = 202002,
            //    EventDiaryConfigId = 15
            //};
            //this._service.UpdateRangeDiaryKpiConfigChanged(new List<EventDiarySyncModelDTO>() { config }, this.UserName);
            this._service.UpdateRangeDiaryKpiConfigChanged(configs, this.UserName);
            return DefaultSuccess(string.Empty);
        }
    }
}