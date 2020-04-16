using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.DataAccess.Repo.EFRepos.Kpi
{
    public interface INotificationRepository : IGridRepository<NotificationDTO, NotificationPaginFilterModelDTO>
    {
        IEnumerable<Notification_TypeDTO> GetNotificationType();
        void SetNotificationStatus(NotificationPaginFilterModelDTO status);
        int CountNew(string userName);
    }

    public class NotificationRepository : BaseGridRepository<Notification_Queue, NotificationDTO, NotificationPaginFilterModelDTO>, INotificationRepository
    {
        private readonly IGenericRepository<Notification_Type> _notiTypeRepo;

        public NotificationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._notiTypeRepo = this._unitOfWork.GetDataRepository<Notification_Type>();
        }

        public IEnumerable<Notification_TypeDTO> GetNotificationType()
        {
            return this._notiTypeRepo.SelectAll().ProjectTo<Notification_TypeDTO>().ToList();
        }

        public override IEnumerable<NotificationDTO> GetPaging(NotificationPaginFilterModelDTO pagingModel)
        {
            var query = this.PagingFilter(_repo.SelectAll(), pagingModel);
            var catl = this._repo.SelectAll();
            var res = query.ProjectTo<NotificationDTO>().OrderByDescending(x => x.Id).Skip(pagingModel.Start - 1).Take(pagingModel.Length);
            return res.ToList();
        }

        public void SetNotificationStatus(NotificationPaginFilterModelDTO status)
        {
            var noti = _repo.SelectWhere(x => x.Id == status.Id).FirstOrDefault();
            if (noti != null)
            {
                noti.Status = status.Status;
                this._repo.UpdateByProperties(noti, new List<Expression<Func<Notification_Queue, object>>> { x => x.Status });
                this._unitOfWork.SaveChanges();
            }
            else { throw new Exception("Không tìm thấy thông báo id='" + status.Id + "'"); }
        }

        protected override IQueryable<Notification_Queue> PagingFilter(IQueryable<Notification_Queue> query, NotificationPaginFilterModelDTO pagingModel)
        {
            if (!string.IsNullOrWhiteSpace(pagingModel.UserName))
            {
                query = query.Where(x => x.ToUserName == pagingModel.UserName);
            }
            if (pagingModel.Type >= 0)
            {
                query = query.Where(x => x.Type == pagingModel.Type);
            }
            if (pagingModel.Status >= 0)
            {
                query = query.Where(x => x.Status == pagingModel.Status);
            }
            return query;
        }

        public int CountNew(string userName)
        {
            var res = this._repo.SelectWhere(x=>x.ToUserName==userName&&x.Status!=1).Count();
            return res;
        }
    }
}
