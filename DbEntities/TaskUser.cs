using DbMapper.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbEntities
{
    public class TaskUser
    {
        [JsonProperty("userId")]
        [DataNames("user_id")]
        public string UserId { get; set; }

        [JsonProperty("userName")]
        [DataNames("UserName")]
        public string UserName { get; set; }

        //[DataNames("task_id")]
        //public string TaskId { get; set; }
    }
}
