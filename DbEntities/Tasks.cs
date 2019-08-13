using DbMapper.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbEntities
{
    public class Tasks: BaseEntity
    {
        [JsonProperty("statusId")]
        [DataNames("status_id")]
        public int StatusId { get; set; }

        [JsonProperty("statusName")]
        [DataNames("status_name")]
        public string StatusName { get; set; }

        [JsonProperty("priorityId")]
        [DataNames("priority_id")]
        public int PriorityId { get; set; }

        [JsonProperty("projectId")]
        [DataNames("project_id")]
        public int ProjectId { get; set; }

        [JsonProperty("fromUserId")]
        [DataNames("from_user_id")]
        public string FromUserId { get; set; }

        [JsonProperty("fromUserName")]
        [DataNames("from_user_name")]
        public string FromUserName { get; set; }

        [JsonProperty("name")]
        [DataNames("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        [DataNames("description")]
        public string Description { get; set; }

        [JsonProperty("finishDatetime")]
        [DataNames("finish_datetime")]
        public DateTime FinishDatetime { get; set; }

        [JsonProperty("users")]
        public List<TaskUser> Users { get; set; }

        [JsonProperty("comments")]
        public List<Comment> Comments { get; set; }

        public List<Files> Files { get; set; }

        public Tasks()
        {
            Files = new List<Files>();
        }
    }
}
