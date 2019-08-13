using System.Collections.Generic;
using Newtonsoft.Json;

namespace ViewModels.Common
{
    public class Filter
    {
        [JsonProperty("logic")]
        public string Logic { get; set; }

        [JsonProperty("filters")]
        public IEnumerable<Filter> Filters { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }

        public Filter()
        {
            Filters=new List<Filter>();
        }
    }
}
