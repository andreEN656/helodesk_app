using DbMapper.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DbEntities
{
    public class BaseEntity
    {
        [Key]
        [JsonProperty("id")]
        [DataNames("id")]
        public int? Id { get; set; }
    }
}
