using Billing.Api.Interfaces;
using Newtonsoft.Json;


namespace Billing.Api.Models
{
    public class SubstactBody : IBillBodyInfo
    {
        [JsonProperty("app_type")]
        public int AppType { get; set; }

        [JsonProperty("app_order")]
        public string OrderNo { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("money_type")]
        public int MoneyType { get; set; }

        [JsonProperty("sub_count")]
        public int Amount { get; set; }

        [JsonProperty("sub_type")]
        public int SubType { get; set; }

        [JsonProperty("freeze_timeout")]
        public int FreezeTime { get; set; }


    }

    public class SubstractResult : SubstactBody
    {
        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("bill_order")]
        public string BillOrder { get; set; }

        [JsonProperty("sum_money1_count")]
        public int SubMoney1Amount { get; set; }

        [JsonProperty("sum_money2_count")]
        public int SubMoney2Amount { get; set; }

        [JsonProperty("money1_balance")]
        public int Money1Balance { get; set; }

        [JsonProperty("money2_balance")]
        public int Money2Balance { get; set; }

        [JsonProperty("money1_freeze")]
        public int Money1Freeze { get; set; }

        [JsonProperty("money2_freeze")]
        public int Money2Freeze { get; set; }

    }
}
