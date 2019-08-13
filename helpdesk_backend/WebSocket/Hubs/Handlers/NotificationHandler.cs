using DataAccess.Repositories.Implementation.NotificationRepository;
using DataAccess.Uow;
using DbEntities;
using helpdesk_backend.WebSocket.Hubs.Handlers.EventsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace helpdesk_backend.WebSocket.Hubs.Handlers
{
    public class NotificationHandler
    {
        public event EventHandler<SingleValueEventArgs> ChangeTracker;

        private readonly IUowProvider _uowProvider;
        private NotificationRepository notificationRepository;

        List<Notification> Notifications;
        List<Notification> _Notifications;

        int Timeout;

        public NotificationHandler(int timeout, IUowProvider uowProvider)
        {
            this.Timeout = timeout;
            this._uowProvider = uowProvider;
        }

        public async Task InitNotifications()
        {
            if (Notifications == null)
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    notificationRepository = (NotificationRepository)uow.GetCustomRepository<INotificationRepository>();
                    Notifications = (await notificationRepository.GetAll()).ToList();
                }
            }
        }

        public void InitStart()
        {
            Start();
        }

        private void Start()
        {

            while (true)
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    var notificationRepository = (NotificationRepository)uow.GetCustomRepository<INotificationRepository>();

                    var _Notifications = notificationRepository.GetAllNotAsync().ToList();
                    if (_Notifications.Count != Notifications.Count)
                    {
                        var args = new SingleValueEventArgs() { Value = _Notifications };
                        ChangeTracker(this, args);
                        Notifications = _Notifications;
                    }
                }
                System.Threading.Thread.Sleep(Timeout);
            }
        }

        private bool CompareNotifications(List<Notification> first, List<Notification> second)
        {
            int count = 0;
            for (int i = 0; i < first.Count; i++)
            {
                if (second.Contains(first[i])) count++;
            }
            return false;
        }



    }
}
