//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                    4.0.30319.42000
//  机器名称:                   LAPTOP-7NAHVT84
//  命名空间名称/文件名:        ItemSystem.Api.Utility/JsonUtility 
//  创建人:                             gogo_     
//  创建时间:                       2017/6/15 18:44:59
//  公司：                          09game.com
//==============================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemSystem.Api.Utility
{
    public static class JsonUtility
    {
        public static T Deserialize<T>(this string str)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
