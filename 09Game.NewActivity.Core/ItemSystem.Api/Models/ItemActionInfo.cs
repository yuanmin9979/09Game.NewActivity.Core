//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                    4.0.30319.42000
//  机器名称:                   LAPTOP-7NAHVT84
//  命名空间名称/文件名:        ItemSystem.Api.Models/ItemActionInfo 
//  创建人:                             gogo_     
//  创建时间:                       2017/6/15 17:40:20
//  公司：                          09game.com
//==============================================================

using ItemSystem.Api.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemSystem.Api.Models
{
    public class ItemActionInfo
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("count")]
        public int Amount { get; set; }

        [JsonProperty("money_count")]
        public int Money { get; set; } = 0;

        [JsonProperty("expire")]
        public long Expire { get; set; } = new DateTime(2099,12,1).UniversalTimestamp();
    }
}
