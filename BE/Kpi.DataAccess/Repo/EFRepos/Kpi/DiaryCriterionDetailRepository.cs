using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using Kpi.Core.DTO;
using Kpi.Core.Helper;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;

namespace Kpi.DataAccess.Repo.EFRepos.Kpi
{
    public interface IDiaryCriterionDetailRepository : IGridRepository<DiaryCriterionDetailDTO, DiaryCriterionDetailFilterModelDTO>
    {
        void RemoveByManagerLv2(DiaryCriterionDetailDTO filter, string userName, bool isHrOrHrManager);
        void UpdateKpiCriterionPointByYearMonth(int yearMonth);
        void UpdateKpiCriterionPointByKpiId(int id);
    }

    public class DiaryCriterionDetailRepository : BaseGridRepository<Kpi_DiaryCriterionDetail, DiaryCriterionDetailDTO, DiaryCriterionDetailFilterModelDTO>, IDiaryCriterionDetailRepository
    {
        private readonly IGenericRepository<Kpi_DiaryCriterionDetail> _kpiDiaryCriterionDetailGRepo;
        private readonly IGenericRepository<Kpi_KpiCriterionDetail> _kpiCriterionDetailGRepo;
        private readonly IGenericRepository<Kpi_KpiEvaluation> _kpiEvaluationGRepo;
        private readonly IGenericRepository<Kpi_CriterionCatalog> _kpiCriCatalogRepo;

        public DiaryCriterionDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _kpiDiaryCriterionDetailGRepo = _unitOfWork.GetDataRepository<Kpi_DiaryCriterionDetail>();
            _kpiCriterionDetailGRepo = _unitOfWork.GetDataRepository<Kpi_KpiCriterionDetail>();
            _kpiEvaluationGRepo = _unitOfWork.GetDataRepository<Kpi_KpiEvaluation>();
            _kpiCriCatalogRepo = _unitOfWork.GetDataRepository<Kpi_CriterionCatalog>();
        }

        /// <summary>
        /// Thêm Detail của diary
        /// Tính toán lại điểm kpi của đối tượng
        /// </summary>
        public override DiaryCriterionDetailDTO Add(DiaryCriterionDetailDTO entityDTo)
        {
            Kpi_DiaryCriterionDetail diaryDetailEntity;
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                // lưu diary detail
                diaryDetailEntity = AutoMapperHelper.Map<DiaryCriterionDetailDTO, Kpi_DiaryCriterionDetail>(entityDTo);
                diaryDetailEntity = _repo.Add(diaryDetailEntity);
                _unitOfWork.SaveChanges();

                // tính toán lại điểm của kpi 
                this.UpdateKpiPoint(diaryDetailEntity);
                transaction.Commit();
            }

            return AutoMapperHelper.Map<Kpi_DiaryCriterionDetail, DiaryCriterionDetailDTO>(diaryDetailEntity);
        }

        /// <summary>
        /// Update điểm Kpi của Kpi và Kpi detail
        /// </summary>
        private void UpdateKpiPoint(Kpi_DiaryCriterionDetail diaryDetail)
        {
            // get kpi
            var kpiEvaluate = this._kpiEvaluationGRepo.SelectWhere(x => x.EventDiaryId == diaryDetail.EventDiaryId && x.UserName == diaryDetail.UserName).FirstOrDefault();
            if (kpiEvaluate == null)
            {
                throw new ObjectNotFoundException("Kpi của nhân viên này không tồn tại");
            }

            // get kpi detail có catalog là parrent của diaryDetail
            var modifiedDiaryDetails = this._kpiCriterionDetailGRepo.SelectWhere(x => x.KpiEvaluateId == kpiEvaluate.Id && x.KpiCatalogId == diaryDetail.CriterionCatalogFolderId).FirstOrDefault();
            if (modifiedDiaryDetails == null)
            {
                throw new ObjectNotFoundException("Kpi của nhân viên này không có tiêu chuẩn này");
            }

            // update điểm kpi của detail
            //modifiedDiaryDetails.ManagerEvaluatePoint += diaryDetail.KpiPoint;
            modifiedDiaryDetails.ManagerEvaluatePoint = GetLevel1PointOfCriterionDetail(modifiedDiaryDetails, kpiEvaluate);
            modifiedDiaryDetails.ManagerEvaluateComment = String.Format("{0}. {1}", modifiedDiaryDetails.ManagerEvaluateComment, diaryDetail.Comment);
            modifiedDiaryDetails.ManagerLastUpdatedDate = DateTime.Now;
            this._kpiCriterionDetailGRepo.UpdateByProperties(modifiedDiaryDetails,
                new List<Expression<Func<Kpi_KpiCriterionDetail, object>>> { x => x.ManagerEvaluatePoint, x => x.ManagerEvaluateComment, x => x.ManagerLastUpdatedDate });
            this._unitOfWork.SaveChanges();

            // sua diem neu sai
            RefreshCriterionPoint(kpiEvaluate.Id);

            // update điểm kpi của kpi evaluate
            if (kpiEvaluate.Level1ManagerKpiPoint == null)
            {
                kpiEvaluate.Level1ManagerKpiPoint = 100;
            }
            //kpiEvaluate.Level1ManagerKpiPoint += diaryDetail.KpiPoint;
            kpiEvaluate.Level1ManagerKpiPoint = GetLevel1PointFromCriterionDetail(kpiEvaluate.Id);
            kpiEvaluate.Level1ManagerKpiClassification = this.CalKpiClassification(kpiEvaluate.Level1ManagerKpiPoint.Value);
            this._kpiEvaluationGRepo.UpdateByProperties(kpiEvaluate,
                new List<Expression<Func<Kpi_KpiEvaluation, object>>> { x => x.Level1ManagerKpiPoint, x => x.Level1ManagerKpiClassification });
            this._unitOfWork.SaveChanges();
        }

        private string CalKpiClassification(int kpiPoint)
        {
            if (kpiPoint >= 105)
            {
                return "A+";
            }
            else if (kpiPoint >= 95)
            {
                return "A";
            }
            else if (kpiPoint >= 85)
            {
                return "A-";
            }
            else if (kpiPoint >= 75)
            {
                return "B+";
            }
            else if (kpiPoint >= 66)
            {
                return "B";
            }
            else if (kpiPoint >= 60)
            {
                return "B-";
            }
            else
            {
                return "C";
            }
        }

        private int GetLevel1PointOfCriterionDetail(Kpi_KpiCriterionDetail criter, Kpi_KpiEvaluation kpiEvaluate)
        {
            var maxpoint = criter.MaximumPoint;
            //var catalog = this._kpiCriCatalogRepo.SelectWhere(x => x.Id == criter.KpiCatalogId).FirstOrDefault();
            //if (catalog != null)
            //{
            //    maxpoint = catalog.MaximumPoint;
            //}

            var ret = this._repo.SelectWhere(x => x.EventDiaryId == kpiEvaluate.EventDiaryId && x.CriterionCatalogFolderId == criter.KpiCatalogId
            && x.UserName == kpiEvaluate.UserName
            && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)).ToList();
            return maxpoint + ret.Sum(x => x.KpiPoint);

        }

        public void UpdateKpiCriterionPointByKpiId(int kpiEvaluateId)
        {
            using (var trans = this._unitOfWork.BeginTransaction())
            {
                var kpiEvaluate = this._kpiEvaluationGRepo.SelectWhere(x => x.Id == kpiEvaluateId).FirstOrDefault();
                if (kpiEvaluate != null)
                {
                    // danh sach criterion
                    var kpiCriterions = this._kpiCriterionDetailGRepo.SelectWhere(x => x.KpiEvaluateId == kpiEvaluateId);

                    foreach (var kpiCriter in kpiCriterions)
                    {
                        kpiCriter.ManagerEvaluatePoint = GetLevel1PointOfCriterionDetail(kpiCriter, kpiEvaluate);
                        this._kpiCriterionDetailGRepo.UpdateByProperties(kpiCriter,
                           new List<Expression<Func<Kpi_KpiCriterionDetail, object>>> { x => x.ManagerEvaluatePoint, x => x.ManagerEvaluateComment, x => x.ManagerLastUpdatedDate });
                        this._unitOfWork.SaveChanges();
                    }

                    // kpi
                    kpiEvaluate.Level1ManagerKpiPoint = GetLevel1PointFromCriterionDetail(kpiEvaluate.Id);

                    this._kpiEvaluationGRepo.UpdateByProperties(kpiEvaluate,
                        new List<Expression<Func<Kpi_KpiEvaluation, object>>> { x => x.Level1ManagerKpiPoint, x => x.Level1ManagerKpiClassification });
                    this._unitOfWork.SaveChanges();
                    trans.Commit();
                }

            }
        }

        public void UpdateKpiCriterionPointByKpiIdNoTrans(int kpiEvaluateId)
        {
            var kpiEvaluate = this._kpiEvaluationGRepo.SelectWhere(x => x.Id == kpiEvaluateId).FirstOrDefault();
            if (kpiEvaluate != null)
            {

                // danh sach criterion
                var kpiCriterions = this._kpiCriterionDetailGRepo.SelectWhere(x => x.KpiEvaluateId == kpiEvaluateId);

                foreach (var kpiCriter in kpiCriterions)
                {
                    kpiCriter.ManagerEvaluatePoint = GetLevel1PointOfCriterionDetail(kpiCriter, kpiEvaluate);
                    this._kpiCriterionDetailGRepo.UpdateByProperties(kpiCriter,
                       new List<Expression<Func<Kpi_KpiCriterionDetail, object>>> { x => x.ManagerEvaluatePoint, x => x.ManagerEvaluateComment, x => x.ManagerLastUpdatedDate });
                    this._unitOfWork.SaveChanges();
                }

                // kpi
                kpiEvaluate.Level1ManagerKpiPoint = GetLevel1PointFromCriterionDetail(kpiEvaluate.Id);

                this._kpiEvaluationGRepo.UpdateByProperties(kpiEvaluate,
                    new List<Expression<Func<Kpi_KpiEvaluation, object>>> { x => x.Level1ManagerKpiPoint, x => x.Level1ManagerKpiClassification });
                this._unitOfWork.SaveChanges();
            }
        }

        public void UpdateKpiCriterionPointByYearMonth(int yearMonth)
        {
            using (var trans = this._unitOfWork.BeginTransaction())
            {
                var kpis = this._kpiEvaluationGRepo.SelectWhere(x => x.YearMonth == yearMonth).ToList();
                if (kpis.Count > 0)
                {
                    foreach (var kpi in kpis)
                    {
                        UpdateKpiCriterionPointByKpiIdNoTrans(kpi.Id);
                    }

                    trans.Commit();
                }
            }
        }


        private int GetLevel1PointFromCriterionDetail(int kpiEvaluateId)
        {
            var ret = this._kpiCriterionDetailGRepo.SelectWhere(x => x.KpiEvaluateId == kpiEvaluateId).ToList();
            return ret.Sum(x => x.ManagerEvaluatePoint);

        }

        public override void Update(DiaryCriterionDetailDTO entityDto)
        {
            var docEntity = AutoMapperHelper.Map<DiaryCriterionDetailDTO, Kpi_DiaryCriterionDetail>(entityDto);
            _repo.Update(docEntity);
            _unitOfWork.SaveChanges();
        }

        public override DiaryCriterionDetailDTO Get(int id)
        {
            Kpi_DiaryCriterionDetail kpi_DiaryCriterionDetail = _kpiDiaryCriterionDetailGRepo.Get(x => x.Id == id);
            return AutoMapperHelper.Map<Kpi_DiaryCriterionDetail, DiaryCriterionDetailDTO>(kpi_DiaryCriterionDetail);
        }

        public override IEnumerable<DiaryCriterionDetailDTO> GetPaging(DiaryCriterionDetailFilterModelDTO pagingModel)
        {
            return base.GetPaging(pagingModel);
        }

        public void RemoveByManagerLv2(DiaryCriterionDetailDTO filter, string userName, bool isHrOrHrManager)
        {
            var diaryCrtDetail = this._repo.SelectWhere(x => x.Id == filter.Id && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)).FirstOrDefault();
            if (diaryCrtDetail != null)
            {
                var kpiEvaluate = this._kpiEvaluationGRepo.SelectWhere(x => x.EventDiaryId == diaryCrtDetail.EventDiaryId && x.UserName == diaryCrtDetail.UserName).FirstOrDefault();

                // user dang nhap trung voi user quan ly truc tiep hoac cap tren
                if (kpiEvaluate != null && (kpiEvaluate.Level2ManagerUserName.ToLower() == userName.ToLower() || kpiEvaluate.Level1ManagerUserName.ToLower() == userName.ToLower())
                    || isHrOrHrManager)
                {
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        // lưu diary detail
                        diaryCrtDetail.IsDeleted = true;
                        this._repo.UpdateByProperties(diaryCrtDetail, new List<Expression<Func<Kpi_DiaryCriterionDetail, object>>> { x => x.IsDeleted });
                        this._unitOfWork.SaveChanges();

                        // sua diem
                        var tmpDiaryCrtDetail = new Kpi_DiaryCriterionDetail();
                        tmpDiaryCrtDetail.KpiPoint = -diaryCrtDetail.KpiPoint;
                        tmpDiaryCrtDetail.EventDiaryId = diaryCrtDetail.EventDiaryId;
                        tmpDiaryCrtDetail.UserName = diaryCrtDetail.UserName;
                        tmpDiaryCrtDetail.CriterionCatalogFolderId = diaryCrtDetail.CriterionCatalogFolderId;
                        tmpDiaryCrtDetail.Comment = diaryCrtDetail.Comment;
                        UpdateKpiPoint(tmpDiaryCrtDetail);

                        transaction.Commit();
                    }
                }
                else
                {
                    throw new Exception("Không tìm thấy nhật ký sự kiện (id-" + diaryCrtDetail.EventDiaryId + ") hoặc người dùng không phải quản lý/GĐ nhân sự");
                }
            }
        }

        private bool IsHrOrHrManager(string userName)
        {
            throw new NotImplementedException();
        }

        public void RefreshCriterionPoint(int kpiEvaluateId)
        {
            var kpi = this._kpiEvaluationGRepo.SelectWhere(x => x.Id == kpiEvaluateId).FirstOrDefault();
            if (kpi != null)
            {
                var eventDiary = this._kpiEvaluationGRepo.SelectWhere(x => x.Id == kpi.EventDiaryId).FirstOrDefault();
                if (eventDiary != null)
                {
                    // danh sach su kien nksk
                    var eventCrits = this._kpiDiaryCriterionDetailGRepo.SelectWhere(x => x.EventDiaryId == eventDiary.Id
                    && ((x.IsDeleted != null && x.IsDeleted.HasValue && !x.IsDeleted.Value) || x.IsDeleted == null || !x.IsDeleted.HasValue)
                    && x.UserName == kpi.UserName
                        ).ToList();
                    if (eventCrits.Count > 0)
                    {
                        // danh sach tieu chuan kpi
                        var kpiCrits = this._kpiCriterionDetailGRepo.SelectWhere(x => x.KpiEvaluateId == kpiEvaluateId).ToList();
                        if (kpiCrits.Count > 0)
                        {
                            foreach (var item in kpiCrits)
                            {
                                // danh sach tieu chuan con
                                var childCrits = this._kpiCriCatalogRepo.SelectWhere(x => x.ParentId == item.KpiCatalogId).ToList();
                                if (childCrits.Count > 0)
                                {
                                    // update
                                    var kpiCrit = this._kpiCriterionDetailGRepo.SelectWhere(x => x.Id == item.Id).FirstOrDefault();
                                    var sumPoint = eventCrits.Where(x => childCrits.Exists(y => y.Id == x.CriterionCatalogId)).Sum(x => x.KpiPoint);
                                    kpiCrit.ManagerEvaluatePoint = kpiCrit.MaximumPoint + sumPoint;
                                    this._kpiCriterionDetailGRepo.UpdateByProperties(kpiCrit, new List<Expression<Func<Kpi_KpiCriterionDetail, object>>> { x => x.ManagerEvaluatePoint });
                                    this._unitOfWork.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
