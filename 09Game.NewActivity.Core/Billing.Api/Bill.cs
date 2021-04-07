using Billing.Api.Configuration;
using Billing.Api.Enums;
using Billing.Api.Models;
using Billing.Api.Utility;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Billing.Api
{
    public class Bill
    {
        private BillClient _cl;

        public Bill(string apiUrl, int appId, int keyVersion, string appKey)
        {
            _cl = new BillClient(apiUrl, appId, keyVersion, appKey);
        }

        public Bill(IOptions<BillConfig> config):this(config.Value.Url, config.Value.AppId, config.Value.KeyVersion, config.Value.AppKey) { }

        public Bill(BillConfig config) : this(config.Url, config.AppId, config.KeyVersion, config.AppKey) { }

        public async Task<SubstractResult> Substract(int appType, string orderNo, int userId, int moneyType, int amount, int subType = 0, int freezTime = 0)
        {
            var subBody = new SubstactBody
            {
                Amount = amount,
                AppType = appType,
                FreezeTime = freezTime,
                MoneyType = moneyType,
                OrderNo = orderNo,
                SubType = subType,
                UserId = userId
            };
            var rslt = await _cl.GetAsync<SubstractResult>(subBody, BillActionType.Sub);
            return rslt;
        }

        public string SubstractUrl(int appType, string orderNo, int userId, int moneyType, int amount, int subType = 0, int freezTime = 0)
        {
            var subBody = new SubstactBody
            {
                Amount = amount,
                AppType = appType,
                FreezeTime = freezTime,
                MoneyType = moneyType,
                OrderNo = orderNo,
                SubType = subType,
                UserId = userId
            };
            return _cl.GetUrl(subBody, BillActionType.Sub);
        }

        public async Task<AddResult> Add(int appType, int userId, string orderNo, int channelId, string channelOrder, MoneyType type, int amount, int expireDays, int rmbCount=0, string reason="")
        {
            var addBody = new AddMoneyBody
            {
                AppType = appType,
                UserId = userId,
                OrderNo = orderNo,
                ChannelId = channelId,
                ChannelOrder = channelOrder,
                MoneyType = (int)type,
                Amount = amount,
                MoneyTimeout = expireDays,
                RmbCount = 0,
                Reason = reason
            };
            var rslt = await _cl.GetAsync<AddResult>(addBody, BillActionType.Pay);
            return rslt;
        }

        public string AddUrl(int appType, int userId, string orderNo, int channelId, string channelOrder, MoneyType type, int amount, int expireDays, int rmbCount = 0, string reason = "")
        {
            var addBody = new AddMoneyBody
            {
                AppType = appType,
                UserId = userId,
                OrderNo = orderNo,
                ChannelId = channelId,
                ChannelOrder = channelOrder,
                MoneyType = (int)type,
                Amount = amount,
                MoneyTimeout = expireDays,
                RmbCount = 0,
                Reason = reason
            };
            return _cl.GetUrl(addBody, BillActionType.Pay);
        }

        public async Task<QueryResult> Query(int userId, int moneyType = 1)
        {
            var qBdy = new QueryBody
            {
                UserId = userId,
                MoneyType = moneyType
            };

            var rslt = await _cl.GetAsync<QueryResult>(qBdy, BillActionType.Query);

            return rslt;

        }

        public string QueryUrl(int userId, int moneyType = 1)
        {
            var qBdy = new QueryBody
            {
                UserId = userId,
                MoneyType = moneyType
            };
            return _cl.GetUrl(qBdy, BillActionType.Query);
        }


        public async Task<RollbackResult> Rollback(int userId, int appType, string billOrder, string appOrder, int rollbackType = 13)
        {
            var rBody = new RollbackBody
            {
                UserId = userId,
                AppOrder = appOrder,
                BillOrder = billOrder,
                AppType = appType,
                RollbackType = rollbackType
            };
            var rslt = await _cl.GetAsync<RollbackResult>(rBody, BillActionType.RollBack);
            return rslt;
        }

        public string RollbackUrl(int userId, int appType, string billOrder, string appOrder, int rollbackType = 13)
        {
            var rBody = new RollbackBody
            {
                UserId = userId,
                AppOrder = appOrder,
                BillOrder = billOrder,
                AppType = appType,
                RollbackType = rollbackType
            };
            return _cl.GetUrl(rBody, BillActionType.RollBack);
        }
    }
}
