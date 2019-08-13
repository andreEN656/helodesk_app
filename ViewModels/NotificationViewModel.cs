using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class NotificationViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }
    }
}
