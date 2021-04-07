using Billing.Api.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Billing.Api.Models
{
    public class RollbackBody : IBillBodyInfo
    {
        [JsonProperty("bill_order")]
        public string BillOrder { get; set; }
        [JsonProperty("app_order")]
        public string AppOrder { get; set; }

        [JsonProperty("app_type")]
        public int AppType { get; set; }


        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("rollback_type")]
        public int RollbackType { get; set; }
    }

    public class RollbackResult : RollbackBody
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
