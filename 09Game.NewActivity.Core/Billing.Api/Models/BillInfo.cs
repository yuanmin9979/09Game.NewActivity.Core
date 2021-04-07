using Newtonsoft.Json;

namespace Billing.Api.Models
{
    internal class BillInfo
    {
        [JsonProperty("bill_client_id")]
        public int AppId { get; set; }

        [JsonProperty("md5_version")]
        public int KeyVersion { get; set; }

        [JsonProperty("bill_type")]
        public int Type { get; set; }

        [JsonProperty("json")]
        public string Body { get; set; }

        [JsonProperty("md5_info")]
        public string Sign { get; set; }
    }
}
