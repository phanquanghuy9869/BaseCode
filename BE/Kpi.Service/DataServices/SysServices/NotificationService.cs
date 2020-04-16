using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Kpi;
using System;
using System.Collections.Generic;

namespace Kpi.Service.DataServices.Kpi
{
    public interface INotificationService : BaseServices.IGridService<NotificationDTO, NotificationPaginFilterModelDTO>
    {
        IEnumerable<Notification_TypeDTO> GetNotificationType();
        void SetNotificationStatus(NotificationPaginFilterModelDTO status);
        int CountNew(string userName);
    }

    public class NotificationService : BaseServices.BaseGridService<INotificationRepository, NotificationDTO, NotificationPaginFilterModelDTO>, INotificationService
    {

        public NotificationService(INotificationRepository repo) : base(repo)
        {
        }

        public IEnumerable<Notification_TypeDTO> GetNotificationType()
        {
            return this._gridRepository.GetNotificationType();
        }

        public void SetNotificationStatus(NotificationPaginFilterModelDTO status)
        {
            this._gridRepository.SetNotificationStatus(status);
        }
        public int CountNew(string userName)
        {
            return this._gridRepository.CountNew(userName);
        }
    }
}
