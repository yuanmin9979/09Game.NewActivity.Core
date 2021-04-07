using Billing.Api.Enums;
using Billing.Api.Interfaces;
using Billing.Api.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Api.Utility
{
    internal class BillClient
    {
        private string _appKey;
        private int _appId;
        private int _keyVersion;
        private string _baseUrl;

        public BillClient(string apiUrl, int appId, int keyVersion, string appKey)
        {
            _appId = appId;
            _keyVersion = keyVersion;
            _appKey = appKey;
            _baseUrl = apiUrl;
        }

        public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<T> GetAsync<T>(IBillBodyInfo body, BillActionType actType)
        {
            var bodyJson = body.ToJson();

            var request = new BillInfo
            {
                AppId = _appId,
                KeyVersion = _keyVersion,
                Type = (int)actType,
                Body = bodyJson,
                Sign = bodyJson.GenerateSign(_appKey)
            };

            var json = request.ToJson();
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
                    var resp = s.ToObj<BillInfo>();
                    
                    if (resp == null) return default(T);

                    var jbody = resp.Body;

                    return jbody.ToObj<T>();
                }
                catch(Exception ex)
                {
                    Logger.Error($"Exception:{ex.Message.ToString()},Url; {url}");
                    return default(T);
                }
            }


        }

        public string GetUrl(IBillBodyInfo body, BillActionType actType)
        {
            var bodyJson = body.ToJson();

            var request = new BillInfo
            {
                AppId = _appId,
                KeyVersion = _keyVersion,
                Type = (int)actType,
                Body = bodyJson,
                Sign = bodyJson.GenerateSign(_appKey)
            };

            var json = request.ToJson();
            var url = $"{_baseUrl}?{json}";
            return url;
        }
    }
}
