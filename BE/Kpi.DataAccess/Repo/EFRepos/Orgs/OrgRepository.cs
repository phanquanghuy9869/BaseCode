using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using Kpi.Core.Helper;
using System.Linq.Expressions;

namespace Kpi.DataAccess.Repo.EFRepos.Orgs
{
    public interface IOrgRepository : IGridRepository<OrganizationDTO, OrgPagingFilterModelDTO>
    {
        IEnumerable<OrganizationTypeDTO> GetOrgTypes();
    }

    public class OrgRepository : BaseGridRepository<Org_Organization, OrganizationDTO, OrgPagingFilterModelDTO>, IOrgRepository
    {
        private readonly IGenericRepository<View_UserOrg_GetPaging> _viewUsrRepo;
        private readonly IGenericRepository<Org_OrganizationType> _orgTypeRepo;

        public OrgRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _viewUsrRepo = _unitOfWork.GetDataRepository<View_UserOrg_GetPaging>();
            _orgTypeRepo = _unitOfWork.GetDataRepository<Org_OrganizationType>();
        }

        public override OrganizationDTO Add(OrganizationDTO entityDTo)
        {
            using (var trans = this._unitOfWork.BeginTransaction())
            {
                if (string.IsNullOrWhiteSpace(entityDTo.Code) || string.IsNullOrWhiteSpace(entityDTo.Name))
                {
                    throw new Exception("Chưa nhập thông tin mã/tên khối/phòng ban");
                }
                var entity = AutoMapperHelper.Map<OrganizationDTO, Org_Organization>(entityDTo);
                var result = _repo.Add(entity);
                _unitOfWork.SaveChanges();

                // update duong dan thu muc
                var org = _repo.SelectWhere(x => x.Id == result.Id).FirstOrDefault();
                var dirPath = result.Id.ToString();
                if (entityDTo.ParentId >= 0)
                {

                    var parent = this._repo.SelectWhere(x => x.Id == entityDTo.ParentId).FirstOrDefault();
                    if (parent != null)
                    {
                        dirPath = parent.DirectoryPath + "." + dirPath;
                        org.NodeID = parent.NodeID;
                        // GetDirectoryPath(parent.ParentId, ref ret);
                    }
                    else
                    {
                        String nodeId = this._repo.SelectAll().Max(x => x.NodeID);
                        int value = int.Parse(nodeId) + 1;
                        dirPath = value.ToString();
                        org.NodeID = dirPath;
                    }
                    //GetDirectoryPath(entityDTo.ParentId, ref dirPath);
                }
                else
                {

                    String nodeId = this._repo.SelectAll().Max(x => x.NodeID);
                    int value = int.Parse(nodeId) + 1;
                    dirPath = value.ToString();
                    org.NodeID = dirPath;
                }
                org.DirectoryPath = dirPath;
                this._repo.UpdateByProperties(org,
                    new List<Expression<Func<Org_Organization, object>>> { x => x.DirectoryPath, x => x.NodeID });

                //_repo.Update(user);
                _unitOfWork.SaveChanges();

                trans.Commit();
                return AutoMapperHelper.Map<Org_Organization, OrganizationDTO>(result);
            }
        }

        private void GetDirectoryPath(int? parentId, ref string ret)
        {
            var parent = this._repo.SelectWhere(x => x.Id == parentId).FirstOrDefault();
            if (parent != null)
            {
                ret = parent.DirectoryPath + "." + ret;
                // GetDirectoryPath(parent.ParentId, ref ret);
            }
            else
            {
                String nodeId = this._repo.SelectAll().Max(x => x.NodeID);
                int value = int.Parse(nodeId) + 1;
                ret = value.ToString();
            }
        }

        public override void Update(OrganizationDTO entityDto)
        {
            using (var trans = this._unitOfWork.BeginTransaction())
            {
                var org = _repo.SelectWhere(x => x.Id == entityDto.Id).FirstOrDefault();
                if (org != null)
                {
                    if (_repo.SelectWhere(x => x.Code == entityDto.Code && x.Id != org.Id).FirstOrDefault() != null)
                    {
                        throw new Exception("Mã khối/phòng ban đã tồn tại");
                    }
                    org.Code = entityDto.Code;
                    org.Name = entityDto.Name;
                    org.NameEn = entityDto.NameEn;
                    org.Description = entityDto.Description;
                    org.OrganizationTypeID = entityDto.OrganizationTypeID;
                    org.ParentId = entityDto.ParentId;
                    org.IsActive = entityDto.IsActive;
                    org.NumberOrder = entityDto.NumberOrder;
                    var dirPath = entityDto.Id.ToString();
                    // khi thay doi phong ban cha
                    if (entityDto.ParentId >= 0)
                    {
                        GetDirectoryPath(entityDto.ParentId, ref dirPath);
                    }
                    org.DirectoryPath = dirPath;

                    this._repo.UpdateByProperties(org,
                        new List<Expression<Func<Org_Organization, object>>> { x => x.Code, x => x.Name, x => x.Description,
                            x => x.OrganizationTypeID, x => x.DirectoryPath, x => x.ParentId, x => x.NameEn, x => x.IsActive, x => x.NumberOrder });
                    _unitOfWork.SaveChanges();

                    // cap nhat duong dan thu muc cac phong ban lien quan
                    var children = GetAllChild(entityDto.Id);
                    if (children.Count > 0)
                    {
                        foreach (var item in children)
                        {
                            var child = this._repo.SelectWhere(x => x.Id == item.Item1).FirstOrDefault();
                            if (child != null)
                            {
                                child.DirectoryPath = item.Item2;
                                this._repo.UpdateByProperties(child,
                                    new List<Expression<Func<Org_Organization, object>>> { x => x.DirectoryPath });
                                _unitOfWork.SaveChanges();
                            }
                        }
                    }
                    trans.Commit();
                }
                else
                {
                    throw new Exception("Không tìm thấy khối/phòng ban [id=" + entityDto.Id + "]");
                }
            }
        }

        private List<Tuple<int, string>> GetAllChild(int parentId)
        {
            var res = new List<Tuple<int, string>>();
            var allOrgs = _repo.SelectWhere(x => x.Id != parentId).ToList();
            if (allOrgs.Count > 0)
            {
                foreach (var item in allOrgs)
                {
                    var tmpDirPath = item.Id.ToString();
                    GetDirectoryPath(item.ParentId, ref tmpDirPath);
                    var arr = tmpDirPath.Split('.');
                    var found = false;
                    if (arr.Length > 0)
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (Convert.ToInt32(arr[i]) == Convert.ToInt32(parentId))
                            {
                                found = true;
                                break;
                            }
                        }

                        // duong dan co chua id cua org
                        if (found)
                        {
                            res.Add(new Tuple<int, string>(item.Id, tmpDirPath));
                        }
                    }
                }
            }
            return res;
        }

        public override IEnumerable<OrganizationDTO> GetPaging(OrgPagingFilterModelDTO pagingModel)
        {
            var allOrg = this._repo.SelectAll();
            var query = this.PagingFilter(_repo.SelectAll(), pagingModel);
            var qry = from org in query
                      join porg in allOrg on org.ParentId equals porg.Id into gj
                      from subOrg in gj.DefaultIfEmpty()
                      select new { org = org, parentName = subOrg.Name };
            return qry.OrderBy(x => x.org.Id).Skip(pagingModel.Start - 1).Take(pagingModel.Length).Select(x =>
                    new OrganizationDTO()
                    {
                        Id = x.org.Id,
                        Code = x.org.Code,
                        Name = x.org.Name,
                        Description = x.org.Description,
                        DirectoryPath = x.org.DirectoryPath,
                        ParentId = x.org.ParentId,
                        ParentName = x.parentName,
                    }
                ).ToList();
        }

        protected override IQueryable<Org_Organization> PagingFilter(IQueryable<Org_Organization> query, OrgPagingFilterModelDTO pagingModel)
        {
            if (!string.IsNullOrWhiteSpace(pagingModel.Code))
            {
                query = query.Where(x => x.Code.Contains(pagingModel.Code));
            }
            if (!string.IsNullOrWhiteSpace(pagingModel.Name))
            {
                query = query.Where(x => x.Name.Contains(pagingModel.Name));
            }
            return query;
        }

        public IEnumerable<OrganizationTypeDTO> GetOrgTypes()
        {
            var query = _orgTypeRepo.SelectAll().ProjectTo<OrganizationTypeDTO>();
            return query.ToList();
        }
    }
}
