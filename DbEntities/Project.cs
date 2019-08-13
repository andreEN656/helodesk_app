using DbMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbEntities
{
    public class Project: BaseEntity
    {
        [DataNames("name")]
        public string Name { get; set; }

        [DataNames("attribute01")]
        public string Attribute01 { get; set; }

    }
}
