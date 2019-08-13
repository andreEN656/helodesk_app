using DataAccess.Context;
using DbEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Implementation.NotificationRepository
{
    public class NotificationRepository : EntityRepositoryBase<HelpdeskAppContext, Notification>, INotificationRepository
    {
        public NotificationRepository(HelpdeskAppContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetAll()
        {
            var notifications = await GetResultMapping(CommandType.Text, null, "SELECT * FROM `notifications`");

            return notifications;
        }

        public IEnumerable<Notification> GetAllNotAsync()
        {
            var notifications = GetResultMappingNotAsync(CommandType.Text, null, "SELECT * FROM `notifications`");

            return notifications;
        }
    }
}
