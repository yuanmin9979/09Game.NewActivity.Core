using Billing.Api.Interfaces;
using Newtonsoft.Json;

namespace Billing.Api.Models
{
    public class QueryBody:IBillBodyInfo
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("money_type")]
        public int MoneyType { get; set; }
    }

    public class QueryResult : QueryBody
    {
        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("money1_balance")]
        public int Money1Balance { get; set; }

        [JsonProperty("money1_freeze")]
        public int Money1Freeze { get; set; }

        [JsonProperty("money2_balance")]
        public int Money2Balance { get; set; }

        [JsonProperty("money2_freeze")]
        public int Money2Freeze { get; set; }
    }
}
