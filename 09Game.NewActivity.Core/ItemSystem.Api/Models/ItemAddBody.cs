//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                    4.0.30319.42000
//  机器名称:                   LAPTOP-7NAHVT84
//  命名空间名称/文件名:        ItemSystem.Api.Models/ItemAddRequestBody 
//  创建人:                             gogo_     
//  创建时间:                       2017/6/15 17:48:58
//  公司：                          09game.com
//==============================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemSystem.Api.Models
{
    internal class ItemAddRequestBody : IBody
    {
        [JsonProperty("client_request_id")]
        public int ClientRequestId { get; set; }

        [JsonProperty("app_type")]
        public int AppType { get; set; }

        [JsonProperty("app_order")]
        public string AppOrder { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("add_info")]
        public ItemActionInfo[] Items { get; set; }

        [JsonProperty("add_time_out")]
        public int TimeOut { get; set; }

        [JsonProperty("add_reason")]
        public string Reason { get; set; }
    }

    public class ItemAddResponseBody
    {
        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("bill_order")]
        public string Order { get; set; }

    }
}
