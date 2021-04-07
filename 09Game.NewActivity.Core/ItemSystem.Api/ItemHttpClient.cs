//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                    4.0.30319.42000
//  机器名称:                   LAPTOP-7NAHVT84
//  命名空间名称/文件名:        ItemSystem.Api/ItemHttpClient 
//  创建人:                             gogo_     
//  创建时间:                       2017/6/15 16:00:46
//  公司：                          09game.com
//==============================================================

using ItemSystem.Api.Enums;
using ItemSystem.Api.Models;
using ItemSystem.Api.Utility;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ItemSystem.Api
{
    internal class ItemHttpClient
    {
        private string _appKey;
        private int _appId;
        private int _keyVersion;

        private readonly ILogger<ItemHttpClient> _logger;
        private readonly object _locker = new object();
        //private static HttpClient Client = new HttpClient();

        public ItemHttpClient(ILogger<ItemHttpClient> logger)
        {
            _logger = logger;
        }

        private string _baseUrl;

        public ItemHttpClient(string apiUrl, int appId, int keyVersion, string appKey)
        {
            _appId = appId;
            _keyVersion = keyVersion;
            _appKey = appKey;

            _baseUrl = apiUrl;

        }

        private static HttpClient _httpClient = null;
        public HttpClient CreateHttpClient()
        {
            if (_httpClient == null) _httpClient = new HttpClient();
            return _httpClient;
        }
        public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<ItemReqAndResp> GetAsync(IBody body, ItemActionType actType)
        {

            var bodyJson = JsonConvert.SerializeObject(body);

            var request = new ItemReqAndResp
            {
                AppId = _appId,
                KeyVersion = _keyVersion,
                Type = (int)actType,
                Body = bodyJson,
                Sign = bodyJson.GenerateSign(_appKey)
            };

            var json = JsonConvert.SerializeObject(request);
            var url = $"{_baseUrl}?{json}";

            using (var cl = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
            {
                try
                {
                    var sw = new Stopwatch();
                    sw.Start();

                    var s = await cl.GetStringAsync($"{_baseUrl}?{json}");
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > 3000)
                        Logger.Info("Exceed3seconds:Url:" + url);
                    return s.Deserialize<ItemReqAndResp>();
                }
                catch (Exception ex)
                {
                    Logger.Error($"Exception:{ex.Message.ToString()},Url; {url}");
                    return null;
                }
                
            }
            //_httpClient = CreateHttpClient();
            //var s = await _httpClient.GetStringAsync($"{_baseUrl}?{json}");
            //_logger.LogError(s);
            //return s.Deserialize<ItemReqAndResp>();

        }

        public string GetUrl(IBody body, ItemActionType actType)
        {

            var bodyJson = JsonConvert.SerializeObject(body);

            var request = new ItemReqAndResp
            {
                AppId = _appId,
                KeyVersion = _keyVersion,
                Type = (int)actType,
                Body = bodyJson,
                Sign = bodyJson.GenerateSign(_appKey)
            };

            var json = JsonConvert.SerializeObject(request);
            var url = $"{_baseUrl}?{json}";
            return url;
        }
    }
}
