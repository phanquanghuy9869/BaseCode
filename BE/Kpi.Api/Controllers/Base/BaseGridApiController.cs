using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Core.Helper;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.Service.BaseServices;
using Kpi.Service.DataServices.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Kpi.Api.Controllers.Base
{
    public class BaseGridApiController<TGridService, TEntityViewModel, TEntityDTo, TPagingFilterModel, TPagingFilterModelDTO> : BaseApiController<TGridService>
       where TEntityViewModel : class, IResourceModel
       where TPagingFilterModelDTO : class, IGridFilterModelDTO
       where TPagingFilterModel : class, IGridFilterModel
       where TEntityDTo : class
       where TGridService : class, IGridService<TEntityDTo, TPagingFilterModelDTO>
    {

        public BaseGridApiController(TGridService service, IUserOrgService userOrgService) : base(service)
        {
            this._userOrgService = userOrgService;
        }

        public BaseGridApiController(TGridService service) : base(service)
        {
        }

        [HttpPost]
        public virtual RespondData Get(int id)
        {
            return Success(_service.Get(id));
        }

        [HttpPost]
        public virtual RespondData GetAll()
        {
            var result = _service.GetAll();
            return Success(result);
        }

        [HttpPost]
        public virtual RespondData GetPaging(TPagingFilterModel filter)
        {
            var filterDTO = AutoMapperHelper.Map<TPagingFilterModel, TPagingFilterModelDTO>(filter);
            filterDTO = FetchFilterPaging(filterDTO);
            var result = _service.GetPaging(filterDTO);
            return Success(result);
        }

        protected virtual TPagingFilterModelDTO FetchFilterPaging(TPagingFilterModelDTO filter)
        {
            return filter;
        }

        [HttpPost]
        public virtual RespondData AddOrEdit(TEntityViewModel model)
        {
            //// add
            if (model.Id <= 0)
            {
                this.Add(model);
            }
            // delete
            else
            {
                this.Update(model);
            }
            return Success(null);
        }   

        [NonAction]
        protected virtual TEntityDTo FetchDataCreate(TEntityViewModel model)
        {
            return AutoMapperHelper.Map<TEntityViewModel, TEntityDTo>(model);
        }

        [NonAction]
        protected virtual TEntityDTo FetchDataUpdate(TEntityViewModel model)
        {
            return AutoMapperHelper.Map<TEntityViewModel, TEntityDTo>(model);
        }

        [NonAction]
        protected virtual void Add(TEntityViewModel model)
        {
            _service.Add(this.FetchDataCreate(model));
        }

        [NonAction]
        protected virtual void Update(TEntityViewModel model)
        {
            _service.Update(this.FetchDataUpdate(model));
        }

        [HttpPost]
        public RespondData Delete(TEntityViewModel model)
        {
            _service.Delete(AutoMapperHelper.Map<TEntityViewModel, TEntityDTo>(model));
            return Success(null);
        }

        [HttpPost]
        public virtual RespondData Count(TPagingFilterModel filter)
        {
            var filterDTO = AutoMapperHelper.Map<TPagingFilterModel, TPagingFilterModelDTO>(filter);
            filterDTO = FetchFilterPaging(filterDTO);
            var result = this._service.Count(filterDTO);
            return Success(result);
        }

        [NonAction]
        protected virtual TPagingFilterModelDTO Map(TPagingFilterModel filter)
        {
            return AutoMapperHelper.Map<TPagingFilterModel, TPagingFilterModelDTO>(filter);
        }
    }
}