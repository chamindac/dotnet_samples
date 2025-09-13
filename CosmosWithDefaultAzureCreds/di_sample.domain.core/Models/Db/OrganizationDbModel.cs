using Newtonsoft.Json;

namespace di_sample.domain.infrastrcture.Models.Db
{
    public class OrganizationDbModel: BaseDbModel
    {
        [JsonProperty("name")]
        public required string Name { get; set; }
    }
}
