using Newtonsoft.Json;

namespace RoaringAPI.ModelRoaring
{
    public class RoaringSearchResult
    {
        [JsonProperty("hitCount")]
        public int HitCount { get; set; }

        [JsonProperty("hits")]
        public List<RoaringSearchResponse> Hits { get; set; }
    }

    public class RoaringSearchResponse
    {
        [JsonProperty("companyId")]
        public string CompanyId { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("legalGroupCode")]
        public string LegalGroupCode { get; set; }

        [JsonProperty("town")]
        public string Town { get; set; }

        [JsonProperty("legalGroupText")]
        public string LegalGroupText { get; set; }
    }
}
