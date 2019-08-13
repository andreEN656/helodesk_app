using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Common
{
    public class PagedTableRequestParameters
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("skip")]
        public int Skip { get; set; }

        [JsonProperty("take")]
        public int Take { get; set; }

        [JsonProperty("filter")]
        public string Filter { get; set; }

        /*[JsonProperty("filter")]
        public Filter Filter { get; set; }

        PagedTableRequestParameters()
        {
            Filter = new Filter();
        }*/
    }
}
