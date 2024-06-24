using Newtonsoft.Json;

namespace AdsReportingPortal.Model.DTO
{
    public class MetaGenRootApi
    {
        [JsonProperty("data")]
        public List<MetaGenApiResponse> Data { get; set; }
    }
}
