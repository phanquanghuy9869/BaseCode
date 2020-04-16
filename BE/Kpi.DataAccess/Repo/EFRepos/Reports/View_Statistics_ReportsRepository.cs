using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;

namespace Kpi.DataAccess.Repo.EFRepos.Reports
{
    public interface IView_Statistics_ReportsRepository
    {
        IEnumerable<OrgInfoModelDTO> GetOrgs();
        IEnumerable<View_Statistics_ReportsDTO> Search(View_Statistics_ReportsFilterModelDTO filter);
        IEnumerable<View_Statistics_ReportsDTO> Search1(View_Statistics_ReportsFilterModelDTO filter);
    }

    public class View_Statistics_ReportsRepository : BaseDataRepository<View_Statistics_ReportsDTO>, IView_Statistics_ReportsRepository
    {
        private readonly IGenericRepository<Org_Organization> _orgRepo;
        private readonly IGenericRepository<View_Statistics_Reports> _repo;

        public View_Statistics_ReportsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _orgRepo = _unitOfWork.GetDataRepository<Org_Organization>();
            _repo = _unitOfWork.GetDataRepository<View_Statistics_Reports>();
        }

        public IEnumerable<OrgInfoModelDTO> GetOrgs()
        {
            // get kpi belong to this config  
            var query = this._orgRepo.SelectAll();
            return query.OrderBy(x => x.Id).Select(x => new OrgInfoModelDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OrganizationTypeID = x.OrganizationTypeID,
                NodeID = x.NodeID,
                DirectoryPath = x.DirectoryPath
            }).ToList();
        }
        public OrgInfoModelDTO FindOrgById(int? orgId)
        {
            // get kpi belong to this config  
            var query = this._orgRepo.SelectAll();
            return query.OrderBy(x => x.Id==orgId).Select(x => new OrgInfoModelDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OrganizationTypeID = x.OrganizationTypeID,
                NodeID = x.NodeID,
                DirectoryPath = x.DirectoryPath
            }).ToList().FirstOrDefault();
        }

       
        public IEnumerable<View_Statistics_ReportsDTO> Search2(View_Statistics_ReportsFilterModelDTO filter)
        {
            OrgInfoModelDTO orgInfoModelDTO = FindOrgById(filter.orgId);
            //   var query = this._repo.SelectWhere(x => x.YearMonth == filter.yearMonth);
            var query = this._repo.SelectWhere(x => x.YearMonth == filter.yearMonth && (x.OrganizationId == filter.orgId || x.DirectoryPath.StartsWith(filter.directoryPath + ".")));
            var lst = query.OrderBy(x => x.NumberOrder).ThenBy(x => x.DirectoryPath).Select(x => new View_Statistics_ReportsDTO
            {
                STT = x.STT,
                DirectoryPath = x.DirectoryPath,
                TongSoNhanVien = x.TongSoNhanVien,
                Tong = x.Tong,
                AP = x.AP,
                A = x.A,
                AM = x.AM,
                BP = x.BP,
                BM = x.BM,
                B = x.B,
                C = x.C,
                Organization = x.Organization,
                ParentId = x.ParentId
            }).ToList();
            List<View_Statistics_ReportsDTO> intList = new List<View_Statistics_ReportsDTO>();
            int size = lst.Count;
            int stt = 0;
            for (int i = 0; i < size; i++)
            {
                var item = lst[i];
                //item.STT = i + 1;
                int? ParentId = 1;
                if (orgInfoModelDTO.ParentId > 0)
                {
                    ParentId = orgInfoModelDTO.ParentId;
                }
                 if (item.ParentId == ParentId)
                 {
                    stt = stt + 1;
                     for (int j = 0; j < size; j++)
                     {
                         var item1 = lst[j];
                         if ( item1.DirectoryPath.Contains(item.DirectoryPath+".")){
                             item.TongSoNhanVien = item.TongSoNhanVien + item1.TongSoNhanVien;
                             item.Tong = item.Tong + item1.Tong;
                             item.AP = item.AP + item1.AP;
                             item.A = item.A + item1.A;
                             item.AM = item.AM + item1.AM;
                             item.BP = item.BP + item1.BP;
                             item.B = item.B + item1.B;
                             item.BM = item.BM + item1.BM;
                             item.C = item.C + item1.C;
                         }
                     }
                    item.STT = stt;
                    item.APPercent = 100 * (float)Math.Round((float)item.AP / (float)item.TongSoNhanVien, 2);
                    item.APercent = 100 * (float)Math.Round((float)item.A / (float)item.TongSoNhanVien, 2);
                    item.AMPercent = 100 * (float)Math.Round((float)item.AM / (float)item.TongSoNhanVien, 2);
                    item.BPPercent = 100 * (float)Math.Round((float)item.BP / (float)item.TongSoNhanVien, 2);
                    item.BPercent = 100 * (float)Math.Round((float)item.B / (float)item.TongSoNhanVien, 2);
                    item.BMPercent = 100 * (float)Math.Round((float)item.BM / (float)item.TongSoNhanVien, 2);
                    item.CPercent = 100 * (float)Math.Round((float)item.C / (float)item.TongSoNhanVien, 2);
                    intList.Add(item);
                }
                
                
            }

            return intList;

        }
        public IEnumerable<OrgInfoModelDTO> GetOrgsByParent(int? ParentId)
        {
            // get kpi belong to this config  
            var query = this._orgRepo.SelectWhere((x => x.ParentId == ParentId));
            return query.OrderBy(x => x.NumberOrder).Select(x => new OrgInfoModelDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OrganizationTypeID = x.OrganizationTypeID,
                NodeID = x.NodeID,
                DirectoryPath = x.DirectoryPath
            }).ToList();
        }
        public IEnumerable<View_Statistics_ReportsDTO> Search1(View_Statistics_ReportsFilterModelDTO filter)
        {
            OrgInfoModelDTO orgInfoModelDTO = FindOrgById(filter.orgId);
            IEnumerable<OrgInfoModelDTO> orgInfoModelDTOs = GetOrgsByParent(filter.orgId);
            List<View_Statistics_ReportsDTO> intList = new List<View_Statistics_ReportsDTO>();
            int size = orgInfoModelDTOs.Count();
            for (int i = 0; i < size; i++) {
                OrgInfoModelDTO item = orgInfoModelDTOs.ElementAt(i);
                //   var query = this._repo.SelectWhere(x => x.YearMonth == filter.yearMonth);
                var query = this._repo.SelectWhere(x => x.YearMonth == filter.yearMonth && (x.OrganizationId == item.Id || x.DirectoryPath.StartsWith(item.DirectoryPath + ".")));
                var lst = query.OrderBy(x => x.NumberOrder).ThenBy(x => x.DirectoryPath).Select(x => new View_Statistics_ReportsDTO
                {
                    STT = i+1,
                    DirectoryPath = x.DirectoryPath,
                    TongSoNhanVien = x.TongSoNhanVien,
                    Tong = x.Tong,
                    AP = x.AP,
                    A = x.A,
                    AM = x.AM,
                    BP = x.BP,
                    BM = x.BM,
                    B = x.B,
                    C = x.C,
                    Organization = item.Name,
                    ParentId = x.ParentId
                }).ToList();
                if (lst != null && lst.Count > 0)
                {
                    var data = lst.FirstOrDefault();

                    for (int j = 1; j < lst.Count; j++)
                    {
                        var item1 = lst[j];
                        //  if(data.ParentId != item1.ParentId) { 
                        data.TongSoNhanVien = data.TongSoNhanVien + item1.TongSoNhanVien;
                        data.Tong = data.Tong + item1.Tong;
                        data.AP = data.AP + item1.AP;
                        data.A = data.A + item1.A;
                        data.AM = data.AM + item1.AM;
                        data.BP = data.BP + item1.BP;
                        data.B = data.B + item1.B;
                        data.BM = data.BM + item1.BM;
                        data.C = data.C + item1.C;
                        //}

                    }

                    data.APPercent = 100 * (float)Math.Round((float)data.AP / (float)data.TongSoNhanVien, 2);
                    data.APercent = 100 * (float)Math.Round((float)data.A / (float)data.TongSoNhanVien, 2);
                    data.AMPercent = 100 * (float)Math.Round((float)data.AM / (float)data.TongSoNhanVien, 2);
                    data.BPPercent = 100 * (float)Math.Round((float)data.BP / (float)data.TongSoNhanVien, 2);
                    data.BPercent = 100 * (float)Math.Round((float)data.B / (float)data.TongSoNhanVien, 2);
                    data.BMPercent = 100 * (float)Math.Round((float)data.BM / (float)data.TongSoNhanVien, 2);
                    data.CPercent = 100 * (float)Math.Round((float)data.C / (float)data.TongSoNhanVien, 2);
                    intList.Add(data);
                }
            }


            return intList;

        }
        public IEnumerable<View_Statistics_ReportsDTO> Search(View_Statistics_ReportsFilterModelDTO filter)
        {
            //   var query = this._repo.SelectWhere(x => x.YearMonth == filter.yearMonth);
            var query = this._repo.SelectWhere(x => x.YearMonth == filter.yearMonth && (x.OrganizationId == filter.orgId || x.DirectoryPath.StartsWith(filter.directoryPath + ".")));
            var lst = query.OrderBy(x => x.NumberOrder).ThenBy(x => x.DirectoryPath).Select(x => new View_Statistics_ReportsDTO
            {
                STT = x.STT,
                DirectoryPath = x.DirectoryPath,
                TongSoNhanVien = x.TongSoNhanVien,
                Tong = x.Tong,
                AP = x.AP,
                A = x.A,
                AM = x.AM,
                BP = x.BP,
                BM = x.BM,
                B = x.B,
                C = x.C,
                Organization = x.Organization,
                ParentId = x.ParentId
            }).ToList();
            int size = lst.Count;
            for (int i = 0; i < size; i++)
            {
                var item = lst[i];
                item.STT = i + 1;               
                item.APPercent = 100 * (float)Math.Round((float)item.AP / (float)item.TongSoNhanVien, 2);
                item.APercent = 100 * (float)Math.Round((float)item.A / (float)item.TongSoNhanVien, 2);
                item.AMPercent = 100 * (float)Math.Round((float)item.AM / (float)item.TongSoNhanVien, 2);
                item.BPPercent = 100 * (float)Math.Round((float)item.BP / (float)item.TongSoNhanVien, 2);
                item.BPercent = 100 * (float)Math.Round((float)item.B / (float)item.TongSoNhanVien, 2);
                item.BMPercent = 100 * (float)Math.Round((float)item.BM / (float)item.TongSoNhanVien, 2);
                item.CPercent = 100 * (float)Math.Round((float)item.C / (float)item.TongSoNhanVien, 2);
            }

            return lst;

        }
    }
}
