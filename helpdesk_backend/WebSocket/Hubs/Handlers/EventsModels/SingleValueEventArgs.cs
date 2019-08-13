using DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace helpdesk_backend.WebSocket.Hubs.Handlers.EventsModels
{
    public class SingleValueEventArgs : EventArgs
    {
        public List<Notification> Value { get; set; }
    }
}
