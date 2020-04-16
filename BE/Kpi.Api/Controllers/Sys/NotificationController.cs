using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Kpi;
using System.Web.Http;

namespace Kpi.Api.Controllers.Kpi
{
    public class NotificationController : BaseGridApiController<INotificationService, NotificationViewModel, NotificationDTO, NotificationPaginFilterModel, NotificationPaginFilterModelDTO>
    {
        public NotificationController(INotificationService service) : base(service)
        {
        }

        [HttpPost]
        public RespondData GetNotificationType()
        {
            return Success(this._service.GetNotificationType());
        }

        [HttpPost]
        public RespondData SetNotificationStatus(NotificationPaginFilterModelDTO status)
        {
            this._service.SetNotificationStatus(status);
            return Success(null);
        }

        [HttpPost]
        public RespondData CountNew()
        {
            return Success(this._service.CountNew(this.UserName));
        }
    }
}