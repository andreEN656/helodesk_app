using DbMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbEntities
{
    public class Notification: BaseEntity
    {
        [DataNames("user_id")]
        public string UserId { get; set; }

        [DataNames("text")]
        public string Text { get; set; }

        [DataNames("visible")]
        public bool Visible { get; set; }
    }
}
