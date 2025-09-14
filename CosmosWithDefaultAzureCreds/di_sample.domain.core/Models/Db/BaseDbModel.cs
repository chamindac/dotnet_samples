using Newtonsoft.Json;
using System;

namespace di_sample.domain.core.Models.Db
{
    public abstract class BaseDbModel
    {
        [JsonProperty("id")]
        public required string Id { get; set; }

        [JsonProperty("createdTimeUtc")]
        public DateTime CreatedTimeUtc { get; set; }
    }
}
