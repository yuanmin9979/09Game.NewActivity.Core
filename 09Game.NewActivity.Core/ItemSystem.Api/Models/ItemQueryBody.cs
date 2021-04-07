//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                    4.0.30319.42000
//  机器名称:                   LAPTOP-7NAHVT84
//  命名空间名称/文件名:        ItemSystem.Api.Models/ItemQueryBody 
//  创建人:                             gogo_     
//  创建时间:                       2017/6/23 16:08:12
//  公司：                          09game.com
//==============================================================

using Newtonsoft.Json;
using System.Collections.Generic;

namespace ItemSystem.Api.Models
{
    public class ItemQueryRequestBody:IBody
    {
        [JsonProperty("client_request_id")]
        public int ClientRequestId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("item_id")]
        public int ItemId { get; set; }
    }

    public class ItemTypeQueryBody:IBody{
        [JsonProperty("client_request_id")]
        public int ClientRequestId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("item_type")]
        public int ItemType { get; set; }


    }

    public class ItemQueryResponseBody
    {
        [JsonProperty("client_request_id")]
        public int ClientRequestId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("item_type")]
        public int ItemType { get; set; }

        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("item")]
        public List<ItemCount> Item { get; set; }
    }

    public class ItemCount
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("item_count")]
        public int Amount { get; set; }
    }
}
