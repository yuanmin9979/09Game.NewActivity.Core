using Billing.Api.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Billing.Api.Models
{
    public class AddMoneyBody : IBillBodyInfo
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("app_type")]
        public int AppType { get; set; }

        [JsonProperty("client_order")]
        public string OrderNo { get; set; }  

        [JsonProperty("channel_id")]
        public int ChannelId { get; set; }

        [JsonProperty("channel_order")]
        public string ChannelOrder { get; set; }
        
        [JsonProperty("money_type")]
        public int MoneyType { get; set; }

        [JsonProperty("add_reason")]
        public string Reason { get; set; } 

        [JsonProperty("rmb_count")]
        public int RmbCount { get; set; }

        [JsonProperty("add_money_count")]
        public int Amount { get; set; }

        [JsonProperty("money_timeout")]
        public int MoneyTimeout { get; set; }

    }
    public class AddResult : SubstactBody
    {
        [JsonProperty("result")]
        public int Result { get; set; }

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
