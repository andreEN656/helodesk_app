using DbMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbEntities
{
    public class Files
    {
        [DataNames("url")]
        public string Url { get; set; }

        [DataNames("name")]
        public string Name { get; set; }
    }
}
