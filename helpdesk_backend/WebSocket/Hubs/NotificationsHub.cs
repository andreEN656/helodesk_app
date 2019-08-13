using DataAccess.Repositories.Implementation.NotificationRepository;
using DataAccess.Uow;
using DbEntities;
using helpdesk_backend.WebSocket.Hubs.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using ViewModels;

namespace helpdesk_backend.WebSocket.Hubs
{
    [Authorize]
    public class NotificationsHub: Hub
    {
        private readonly IUowProvider _uowProvider;
        private NotificationHandler NotificationHandler;

        static List<UsersHub> UsersHub = new List<UsersHub>();
        static List<Notification> Notifications = new List<Notification>();

        public NotificationsHub(IUowProvider uowProvider)
        {
            //CreateNotificationHandler();
            _uowProvider = uowProvider;
        }

        private async Task CreateNotificationHandler()
        {
            NotificationHandler = new NotificationHandler(1000, _uowProvider);
            await NotificationHandler.InitNotifications();
            NotificationHandler.ChangeTracker += NotificationHandler_ChangeTracker;
            NotificationHandler.InitStart();
        }

        private void NotificationHandler_ChangeTracker(object sender, Handlers.EventsModels.SingleValueEventArgs e)
        {
            var Notifications = e.Value;
            foreach (var Notification in Notifications)
            {
                var Users = UsersHub.Where(c => c.UserID == Notification.UserId);
                foreach(var User in Users)
                {
                    Clients.Client(User.ConnectionId).SendAsync("sendNotification", User.UserID, Notification);
                }
            }
        }

        public void Connect()
        {
            var id = Context.ConnectionId;
            var userId = Context.User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

            if (UsersHub.Count(x => x.ConnectionId == id) == 0)
            {
                UsersHub.Add(new UsersHub { ConnectionId = id, UserID = userId });
            }
        }

        public async Task RecievedNotification()
        {
            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var notificationRepository = (NotificationRepository)uow.GetCustomRepository<INotificationRepository>();
                var _Notifications = (await notificationRepository.GetAll()).ToList();
                if (Notifications.Count < _Notifications.Count)
                {
                    var listDouble = _Notifications;
                    foreach (var Notification in Notifications)
                        listDouble.Remove(Notification);

                    Notifications = _Notifications;

                    foreach (var Notification in listDouble)
                    {
                        var Users = UsersHub.Where(c => c.UserID == Notification.UserId);
                        foreach (var User in Users)
                        {
                            await Clients.Client(User.ConnectionId).SendAsync("sendNotification", User.UserID, Notification);
                        }
                    }
                }
            }

        }

        public void SendNotification()
        {
            var output = new NotificationViewModel();
            Clients.Client(Context.ConnectionId).SendAsync("sendNotification", "anonymous", output);
        }
    }
}
