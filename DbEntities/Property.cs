using DbMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbEntities
{
    public class Property: BaseEntity
    {
        [DataNames("name")]
        public string Name { get; set; }
    }
}
