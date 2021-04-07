//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                    4.0.30319.42000
//  机器名称:                   LAPTOP-7NAHVT84
//  命名空间名称/文件名:        ItemSystem.Api.Enums/ItemSubtractRequestBody 
//  创建人:                             gogo_     
//  创建时间:                       2017/6/15 16:57:13
//  公司：                          09game.com
//==============================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemSystem.Api.Models
{
    internal class ItemSubtractRequestBody : IBody
    {
        [JsonProperty("client_request_id")]
        public int ClientRequestId { get; set; }

        [JsonProperty("app_type")]
        public int AppType { get; set; }

        [JsonProperty("app_order")]
        public string AppOrder { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("sub_count")]
        public int Amount { get; set; }
    }

    public class ItemSubtractResponseBody
    {
        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("bill_order")]
        public string Order { get; set; }

        [JsonProperty("item_balance")]
        public int Balance { get; set; }
    }
}
