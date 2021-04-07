//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                    4.0.30319.42000
//  机器名称:                   LAPTOP-7NAHVT84
//  命名空间名称/文件名:        ItemSystem.Api.Models/ItemRequest 
//  创建人:                             gogo_     
//  创建时间:                       2017/6/15 15:53:17
//  公司：                          09game.com
//==============================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemSystem.Api.Models
{
    internal class ItemReqAndResp
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
