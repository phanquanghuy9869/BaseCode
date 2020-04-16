using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using System.Collections.Generic;
using System.Linq;

namespace Kpi.DataAccess.Repo.EFRepos.Sys
{
    public interface IMenuRepository : IGridRepository<MenuDTO, MenuFilterModelDTO>
    {
        IEnumerable<MenuDTO> GetMenuByRole(string userName);
    }

    public class MenuRepository : BaseGridRepository<Sys_Menu, MenuDTO, MenuFilterModelDTO>, IMenuRepository
    //public class MenuRepository : BaseCategoryGridRepository<Sys_Menu, MenuDTO, MenuFilterModelDTO>, IMenuRepository
    {
        private readonly IGenericRepository<Sys_Menu> _menuRepo;
        private readonly IGenericRepository<RoleMenuPermission> _rolePermissRepo;
        private readonly IGenericRepository<View_AspNetUserRoles> _userRolesRepo;
        private readonly IGenericRepository<AspNetUser> _aspNetUserRepo;
        private readonly IGenericRepository<AspNetRole> _aspNetRoleRepo;

        public MenuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _menuRepo = _unitOfWork.GetDataRepository<Sys_Menu>();
            _rolePermissRepo = _unitOfWork.GetDataRepository<RoleMenuPermission>();
            _userRolesRepo = _unitOfWork.GetDataRepository<View_AspNetUserRoles>();
            _aspNetUserRepo = _unitOfWork.GetDataRepository<AspNetUser>();
            _aspNetRoleRepo = _unitOfWork.GetDataRepository<AspNetRole>();
        }

        /// <summary>
        /// get all menu that roles have permission
        /// </summary>
        public IEnumerable<MenuDTO> GetMenuByRole(string userName)
        {
            var user = this._aspNetUserRepo.SelectWhere(u => u.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
            if (user != null)
            {
                var menus = this._repo.SelectAll();
                var permiss = this._rolePermissRepo.SelectAll();
                var roles = this._aspNetRoleRepo.SelectAll();

                var userRoles = this._userRolesRepo.SelectWhere(x => x.UserId == user.Id);

                var res = menus.Join(permiss, menu => menu.Id, per => per.MenuID, (menu, per) => new { menu, per })
                    .Join(roles, menuPer => menuPer.per.RoleID, role => role.Id, (menuPer, role) => new { menu = menuPer.menu, per = menuPer.per, role })
                    .Join(userRoles, mpr => mpr.role.Id, ur => ur.RoleId, (mpr, ur) => new { menu = mpr.menu, per = mpr.per, role = mpr.role, ur });

                var ret = res.Where(x => x.ur.UserId == user.Id).Select(r => r.menu).ProjectTo<MenuDTO>().ToList().DistinctBy(x => new { x.Id });
                return ret;
            }

            return null;
        }
    }
}
