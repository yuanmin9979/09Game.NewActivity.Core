using ItemSystem.Api.Configuration;
using ItemSystem.Api.Enums;
using ItemSystem.Api.Models;
using ItemSystem.Api.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ItemSystem.Api
{
    public class ItemApp
    {
        private ItemHttpClient _cl;

        public ItemApp(string apiUrl, int appId, int keyVersion, string appKey)
        {
            _cl = new ItemHttpClient(apiUrl, appId, keyVersion, appKey);
        }

        public ItemApp(IOptions<ItemApiConfig> config):this(config.Value.Url, config.Value.AppId, config.Value.KeyVersion, config.Value.AppKey) { }

        public ItemApp(ItemApiConfig config) : this(config.Url, config.AppId, config.KeyVersion, config.AppKey) { }

        public async Task<ItemQueryResponseBody> Query(int userId, int itemId)
        {
            var body = new ItemQueryRequestBody
            {
                UserId = userId,
                ItemId = itemId
            };
            var rslt = await _cl.GetAsync(body, ItemActionType.Query);

            if (rslt == null) return null;
            return rslt.Body.Deserialize<ItemQueryResponseBody>();
        }

        public string QueryUrl(int userId, int itemId)
        {
            var body = new ItemQueryRequestBody
            {
                UserId = userId,
                ItemId = itemId
            };
            var rslt = _cl.GetUrl(body, ItemActionType.Query);

            return rslt;
        }


        public async Task<ItemQueryResponseBody> QueryByItemType(int userId, int itemType)
        {
            var body = new ItemTypeQueryBody
            {
                UserId = userId,
                ItemType = itemType
            };
            var rslt = await _cl.GetAsync(body, ItemActionType.Query);

            if (rslt == null) return null;
            return rslt.Body.Deserialize<ItemQueryResponseBody>();
        }

        public async Task<ItemSubtractResponseBody> Subtract(int userId, int appType, string appOrder, int itemId, int amount)
        {
            var body = new ItemSubtractRequestBody
            {
                AppType = appType,
                AppOrder = appOrder,
                ItemId = itemId,
                UserId = userId,
                Amount = amount
            };
            
            var rslt =  await _cl.GetAsync(body, ItemActionType.Sub);

            if (rslt == null) return null;
            return rslt.Body.Deserialize<ItemSubtractResponseBody>();
        }

        public string SubtractUrl(int userId, int appType, string appOrder, int itemId, int amount)
        {
            var body = new ItemSubtractRequestBody
            {
                AppType = appType,
                AppOrder = appOrder,
                ItemId = itemId,
                UserId = userId,
                Amount = amount
            };
            return _cl.GetUrl(body, ItemActionType.Sub);
        }


        public async Task<ItemAddResponseBody> Add(int userId, int appType, string appOrder, int timeOut, string reason, params ItemActionInfo[] infos)
        {
            var body = new ItemAddRequestBody
            {
                AppType = appType,
                AppOrder = appOrder,
                Items = infos,
                UserId = userId,
                Reason = reason,
                TimeOut = timeOut
            };
           var rslt = await _cl.GetAsync(body, ItemActionType.Add);
            if (rslt == null) return null;
            return rslt.Body.Deserialize<ItemAddResponseBody>();
            

        }

        public  string AddUrl(int userId, int appType, string appOrder, int timeOut, string reason, params ItemActionInfo[] infos)
        {
            var body = new ItemAddRequestBody
            {
                AppType = appType,
                AppOrder = appOrder,
                Items = infos,
                UserId = userId,
                Reason = reason,
                TimeOut = timeOut
            };
            var rslt = _cl.GetUrl(body, ItemActionType.Add);
            return rslt;
        }

        public async Task<ItemRollbackResult> Rollback(int userId, int appType, string appOrder, string bill_order, string reason, int rollbackType = 23)
        {
            var body = new ItemRollbackRequestBody
            {
                AppType = appType,
                AppOrder = appOrder,
                UserId = userId,
                Reason = reason,
                RollbackType = rollbackType,
                BillOrder = bill_order
            };
            var rslt = await _cl.GetAsync(body, ItemActionType.SubRollback);
            if (rslt == null) return null;
            return rslt.Body.Deserialize<ItemRollbackResult>();
        }

        public async Task<ItemAddResponseBody> AddEx(int userId, int appType, string appOrder, int timeOut, string reason, params ItemActionInfo[] infos)
        {
            //infos = infos.Select(x => new ItemActionInfo
            //{
            //    Amount = x.Amount == -1 ? (x.Expire == -1 ? (int)(new DateTime(2099, 1, 1).Subtract(DateTime.Now).TotalDays) : (int)x.Expire) : x.Amount,
            //    Expire = x.Expire > 0 ? DateTime.Now.AddDays(x.Expire).UniversalTimestamp() : new DateTime(2099, 1, 1).UniversalTimestamp(),
            //    ItemId = x.ItemId,
            //    Money = x.Money
            //}).ToArray();
            //var body = new ItemAddRequestBody
            //{
            //    AppType = appType,
            //    AppOrder = appOrder,
            //    Items = infos,
            //    UserId = userId,
            //    Reason = reason,
            //    TimeOut = timeOut
            //};
            var body = new ItemAddRequestBody
            {
                AppType = 3000144,
                Reason= "商城购买[奇异盆 1]",
                TimeOut = 0,
                AppOrder = "mall-190118487440000000223",
                UserId = 45042,
                Items = new ItemActionInfo[]
                {
                    new ItemActionInfo
                    {
                        ItemId = 1169002,
                        Amount = 30,
                        Money = 297,
                         Expire = 1550381544, 
                         
                    }
                }
            };
            var rslt = await _cl.GetAsync(body, ItemActionType.Add);
            if (rslt == null) return null;
            return rslt.Body.Deserialize<ItemAddResponseBody>();


        }



    }
}
