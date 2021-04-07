using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemSystem.Api.Models
{
    internal class ItemRollbackRequestBody : IBody
    {
        [JsonProperty("client_request_id")]
        public int ClientRequestId { get; set; }

        [JsonProperty("app_type")]
        public int AppType { get; set; }

        [JsonProperty("app_order")]
        public string AppOrder { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("bill_order")]
        public string BillOrder { get; set; }

        [JsonProperty("rollback_type")]
        public int RollbackType { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    public class ItemRollbackResult
    {
        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("item_id")]
        public int Item_id { get; set; }

        [JsonProperty("balance")]
        public int Balance { get; set; }

    }
}
