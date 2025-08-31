using Newtonsoft.Json;

namespace di_sample.domain.infrastrcture.Models.Db
{
    public class OrganizationCosmosDbModel: BaseCosmosDbModel
    {
        [JsonProperty("name")]
        public required string Name { get; set; }
    }
}
