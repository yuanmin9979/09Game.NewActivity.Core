using _09Game.NewActivity.Core.Authentication.DbIWork;
using _09Game.NewActivity.Core.Log;
using _09Game.NewActivity.Core.Util;
using Billing.Api;
using Billing.Api.Utility;
using Dapper;
using ItemSystem.Api;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace _09Game.NewActivity.Core.Authentication.DbWork
{
    public class PifuWork
    {
        private readonly PifuDbWork _pifuDbWork;
        private IMemoryCache _cache;
        private IdGenerator _id;
        private ItemApp cbProjects;
        private Bill _bill;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PifuWork> _logger;

        public PifuWork(ILogger<PifuWork> logger, IConfiguration configuration, PifuDbWork pifuDbWork, IMemoryCache cache)
        {
            _pifuDbWork = pifuDbWork;
            _cache = cache;
            _bill = new Bill(configuration.GetConnectionString("ip"), int.Parse(configuration.GetConnectionString("clientId")), int.Parse(configuration.GetConnectionString("md5Code")), configuration.GetConnectionString("md5Key"));
            cbProjects = new ItemApp(configuration.GetConnectionString("ip"), int.Parse(configuration.GetConnectionString("clientId")), int.Parse(configuration.GetConnectionString("md5Code")), configuration.GetConnectionString("md5Key"));
            _cache = cache;
            _logger = logger;
            _id = new IdGenerator(3);
        }

        private static Dictionary<int, string> _promisePifuName = new Dictionary<int, string>()
        {
            {10182, "骷髅王：凛冬君王" }, {10107, "风暴之灵：金龙雷影"}
        };

        private static Dictionary<int, int> _promisePifuCount = new Dictionary<int, int>()
        {
            {10182, 39 }, {10107, 68 }
        };

        /// <summary>
        /// 许愿
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="item_id"></param>
        /// <returns></returns>
        public async Task<int> UserPromise(int user_id, int item_id)
        {
            if (!_promisePifuName.ContainsKey(item_id))
                throw new ArgumentException("道具id不合法!");

            double amount = 10;
            var moneyResult = await _bill.Query(user_id);
            var money1Balance = moneyResult != null ? moneyResult.Money1Balance : 0;
            if (money1Balance < amount)
                throw new NeedMoneyException { code = 2, NeedMoney = ((int)amount - money1Balance) };

            string pifuName = _promisePifuName[item_id];
            int count = _promisePifuCount[item_id];

            var code = 0;
            _pifuDbWork.BeginTransaction();
            try
            {
                string sql = "INSERT into user_promise_log(app_id,user_id,item_id,item_name,count,update_at,create_at)" +
                "VALUES(90460,@user_id, @item_id, @item_name, @count, NOW(), NOW())";

                code = await _pifuDbWork.Connection.ExecuteAsync(sql, new
                {
                    user_id = user_id,
                    item_id = item_id,
                    item_name = pifuName,
                    count = count
                });
            }
            catch (Exception ex)
            {
                _pifuDbWork.Dispose();
                throw new ArgumentException("无法重复许愿!");
            }

            if (code > 0)
            {
                var orderNo = Guid.NewGuid().ToString();
                try
                {
                    var requestv2 = await _bill.Substract(90460, orderNo, user_id, 1, (int)amount);

                    if (requestv2.Result != 0)
                    {
                        _pifuDbWork.Rollback();
                        _pifuDbWork.Dispose();
                        Logger.Error($"宝箱许愿扣钱:user_id ={user_id},Result={requestv2.Result},RequestURL={_bill.SubstractUrl(90460, orderNo, user_id, 1, (int)amount)}");
                        throw new ArgumentException("扣钱失败!");
                    }
                }
                catch (Exception ex)
                {
                    _pifuDbWork.Rollback();
                    _pifuDbWork.Dispose();
                    _logger.LogWarning(ex, _bill.SubstractUrl(90460, orderNo, user_id, 1, (int)amount));
                    throw new ArgumentException(ex.Message.ToString());
                }

                string url = "http://shop.09game.com:1288/shop";
                dynamic temp = new ExpandoObject();
                temp.packet_id = 904609;
                temp.user_id = user_id;
                temp.expire = 15;

                var bodyJson = JsonConvert.SerializeObject(temp);

                dynamic json = new ExpandoObject();
                json.type = "add_packet";
                json.json = bodyJson;

                var reposJson = JsonConvert.SerializeObject(json);

                var reposurl = $"{url}?{reposJson}";

                var response = "";

                try
                {
                    _logger.LogInformation($"user_id ={user_id},reposurl={reposurl}");
                    response = await HttpRequest.HttpGetAsync(reposurl);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"许愿礼包领取:url ={reposurl}");
                    throw new ArgumentException(ex.Message);
                }

                var jObject = JObject.Parse(response);
                code = jObject.TryGetValue("code", out JToken c) ? c.Value<int>() : -1;
                if (code != 0)
                {
                    _logger.LogError($"user_id ={user_id},response={response}");
                }
            }
            _pifuDbWork.Commit();
            _pifuDbWork.Dispose();
            return code;
        }

        /// <summary>
        /// 许愿详情
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task<dynamic> GetUserPromiseLog(int user_id)
        {
            var sql = "SELECT * from user_promise_log where app_id = 90460 and user_id=@user_id";
            return await _pifuDbWork.Connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { user_id });
        }

        /// <summary>
        /// 能量查询和升级
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="type">1:查询，2:升级</param>
        /// <returns></returns>
        public async Task<dynamic> GetUserEnergyLog(int user_id, int type)
        {
            var sql = " CALL pifu_db.task_will_of_fire(@user_id,@type)";
            return await _pifuDbWork.Connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { user_id, type });
        }

        public async Task UserEnergyUpgrade(int user_id)
        {
            var user = await GetUserEnergyLog(user_id, 1);
            if ((int)user.amount < (int)user.balance)
                throw new ArgumentException($"当前能量不足{(int)user.balance}");
            await GetUserEnergyLog(user_id, 2);
        }
    }
}
