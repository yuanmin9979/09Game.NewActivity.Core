//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                    4.0.30319.42000
//  机器名称:                   LAPTOP-7NAHVT84
//  命名空间名称/文件名:        ItemSystem.Api.Configuration/ItemApiConfig 
//  创建人:                             gogo_     
//  创建时间:                       2017/6/15 20:14:15
//  公司：                          09game.com
//==============================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ItemSystem.Api.Configuration
{
    public class ItemApiConfig
    {
        public string Url { get; set; }

        public int AppId { get; set; }

        public int KeyVersion { get; set; }

        public string AppKey { get; set; }

        //private static IConfigurationRoot _conf;
        //public static ItemApiConfig Default
        //{
        //    get
        //    {
        //        if (_conf == null)
        //        {
        //            var builder = new ConfigurationBuilder()
        //                .SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json", optional: true, reloadOnChange:true);
        //            _conf = builder.Build();
        //        }
        //        var s = _conf.GetSection("ItemApiConfig").Value;
        //        //s.Bind(obj);
        //        return obj;


        //    }
        //}
    }

    public static class ItemApiConfigService
    {
        public static IServiceCollection AddItemApiConfig(this IServiceCollection services, IConfiguration section)
        {
            return services.Configure<ItemApiConfig>(section);
        }
    }
}
