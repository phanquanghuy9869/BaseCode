using System;
using System.Collections.Generic;
using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.Base;
using Kpi.DataAccess.Repo.EFRepos.Kpi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kpi.Api.Tests.Api.DAO.Repository
{
    [TestClass]
    public class KpiEvaluationRepoIntergrationTest : IDisposable
    {
        private KpiEvaluationRepository _repo;

        public KpiEvaluationRepoIntergrationTest()
        {
            var uow = new UnitOfWork();
            _repo = new KpiEvaluationRepository(uow);
        }

        [TestMethod]
        public void Should_Create_Global_Kpi()
        {
            var config = new KpiPeriodConfigDTO
            {
                //Id = 1,
                Code = "HuyPQ Test",
                MonthNumber = 2,
                YearMonth = 202002, 
                FromDate = new DateTime(2020, 1, 25),
                ToDate = new DateTime(2020, 2, 25),
                Note = "Kpi tháng 2/2020",
                CreatedDate = new DateTime(2020, 6, 9),
                IsCreateEventDiary = true,
                CriterionTypes = new List<int> { 1, 2 },
                CreatedByUser = "anh.nt2",
            };

            this._repo.CreateKpiGlobal(config);
        }

        public void Dispose()
        {
            if (_repo != null)
            {
                _repo.Dispose();
            }
        }
    }
}
