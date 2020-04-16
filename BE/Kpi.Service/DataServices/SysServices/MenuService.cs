using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Sys;
using Kpi.Service.BaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Service.DataServices.SysServices
{
    public interface IMenuService : IGridService<MenuDTO, MenuFilterModelDTO>
    {
        IEnumerable<MenuDTO> GetMenuByRole(string userName);
    }

    public class MenuService : BaseGridService<IMenuRepository, MenuDTO, MenuFilterModelDTO>, IMenuService
    {
        public MenuService(IMenuRepository gridRepository) : base(gridRepository)
        {
        }

        public IEnumerable<MenuDTO> GetMenuByRole(string userName)
        {
            return _gridRepository.GetMenuByRole(userName);
        }
    }
}
