using DbMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbEntities
{
    public class SimpleUser
    {
        [DataNames("user_id")]
        public string UserId { get; set; }

        [DataNames("UserName")]
        public string UserName { get; set; }

        [DataNames("task_id")]
        public string TaskId { get; set; }
    }
}
