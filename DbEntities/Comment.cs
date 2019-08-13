using DbMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbEntities
{
    public class Comment: BaseEntity
    {
        [DataNames("user_id")]
        public string UserId { get; set; }

        [DataNames("task_id")]
        public int TaskId { get; set; }

        [DataNames("user_name")]
        public string UserName { get; set; }

        [DataNames("text")]
        public string Text { get; set; }

        [DataNames("datetime")]
        public string Datetime { get; set; }

        public List<Files> Files { get; set; }

        public Comment()
        {
            Files = new List<Files>();
        }

    }
}
