using _09Game.NewActivity.Core.Authentication.DbIWork;
using Billing.Api;
using Billing.Api.Utility;
using Dapper;
using ItemSystem.Api;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09Game.NewActivity.Core.Authentication.DbWork
{
    public class UserWork
    {
        private readonly UserDbWork _userDbWork;
        private IMemoryCache _cache;
        private IdGenerator _id;
        private ItemApp cbProjects;
        private Bill _bill;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserWork> _logger;

        public UserWork(ILogger<UserWork> logger, IConfiguration configuration, UserDbWork userDbWork, IMemoryCache cache)
        {
            _userDbWork = userDbWork;
            _cache = cache;
            _bill = new Bill(configuration.GetConnectionString("ip"), int.Parse(configuration.GetConnectionString("clientId")), int.Parse(configuration.GetConnectionString("md5Code")), configuration.GetConnectionString("md5Key"));
            cbProjects = new ItemApp(configuration.GetConnectionString("ip"), int.Parse(configuration.GetConnectionString("clientId")), int.Parse(configuration.GetConnectionString("md5Code")), configuration.GetConnectionString("md5Key"));
            _cache = cache;
            _logger = logger;
            _id = new IdGenerator(3);
        }

        public async Task<(int, string)> UserExchangeFuWenShi(int user_id)
        {
            int[] Days = new int[] { 6, 0, 1, 2, 3, 4, 5 };
            int day = Days[Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))];
            var request = await cbProjects.Query(user_id, 20086);
            var ucount = request.Item.Count == 0 ? 0 : request.Item[0].Amount;
            if (ucount < 10)
            {
                throw new ArgumentException("兑换所需积分不足!");
            }

            int user_amount = 0;
            _userDbWork.BeginTransaction();
            try
            {
                string sql = $"INSERT into user_exchange_log(app_id,user_id,item_id,item_name,amount,create_at) VALUES(90410,{user_id},19804,'重转符文石*10',10,NOW());SELECT SUM(amount) from user_exchange_log where user_id = {user_id} and item_id = 19804 and app_id=90410 and DATE_SUB(CURDATE(), INTERVAL {day} DAY) <= create_at";
                user_amount = await _userDbWork.Connection.ExecuteScalarAsync<int>(sql);
            }
            catch
            {
                _userDbWork.Dispose();
                throw new ArgumentException("系统异常!");
            }
            var need_amount = ((user_amount - 10) / 70 + 1) * 10;

            if (ucount < need_amount)
            {
                _userDbWork.Rollback();
                _userDbWork.Dispose();
                throw new ArgumentException("兑换所需积分不足!");
            }

            var requestv2 = await cbProjects.Subtract(user_id, 90410, _id.UUID(), 20086, need_amount);

            if (requestv2.Result != 0)
            {
                _logger.LogError($"积分扣道具:user_id ={user_id},Result={requestv2.Result}");
                _userDbWork.Rollback();
                _userDbWork.Dispose();
                throw new ArgumentException("道具扣除失败!");
            }

            var requestv3 = await cbProjects.Add(user_id, 90410, _id.UUID(), 0, "高级贵宾积分兑换", new ItemSystem.Api.Models.ItemActionInfo
            {
                ItemId = 19804,
                Amount = 10,
                Money = 0,
                Expire = 7,
            });

            if (requestv3.Result != 0)
            {
                _logger.LogError($"添加道具:user_id ={user_id},Result={requestv3.Result}");
                _userDbWork.Rollback();
                _userDbWork.Dispose();
                throw new ArgumentException("道具添加失败!");
            }

            _userDbWork.Commit();
            _userDbWork.Dispose();

            return (0, "重转符文石*10");
        }

        public async Task<(int, string)> UserExchangeCJDR(int user_id)
        {
            var request = await cbProjects.Query(user_id, 20086);
            var ucount = request.Item.Count == 0 ? 0 : request.Item[0].Amount;
            if (ucount < 10)
                throw new ArgumentException("兑换所需积分不足!");

            _userDbWork.BeginTransaction();
            try
            {
                string sql = $"INSERT into user_exchange_log(app_id,user_id,item_id,item_name,amount,create_at) VALUES(90410,{user_id},26,'成就达人卡*1天',1,NOW())";
                await _userDbWork.Connection.ExecuteScalarAsync<int>(sql);
            }
            catch
            {
                _userDbWork.Dispose();
                throw new ArgumentException("系统异常!");
            }

            var requestv2 = await cbProjects.Subtract(user_id, 90410, _id.UUID(), 20086, 10);

            if (requestv2.Result != 0)
            {
                _logger.LogError($"积分扣道具:user_id ={user_id},Result={requestv2.Result}");
                _userDbWork.Rollback();
                _userDbWork.Dispose();
                throw new ArgumentException("道具扣除失败!");
            }

            var requestv3 = await cbProjects.Add(user_id, 90410, _id.UUID(), 0, "高级贵宾积分兑换", new ItemSystem.Api.Models.ItemActionInfo
            {
                ItemId = 26,
                Amount = 1,
                Money = 0,
                Expire = 1,
            });

            if (requestv3.Result != 0)
            {
                _logger.LogError($"添加道具:user_id ={user_id},Result={requestv3.Result}");
                _userDbWork.Rollback();
                _userDbWork.Dispose();
                throw new ArgumentException("道具添加失败!");
            }

            _userDbWork.Commit();
            _userDbWork.Dispose();

            return (0, "成就达人卡*1天");
        }

        public async Task<(int, string)> UserExchangeSP2(int user_id, int item_id, string item_name)
        {
            var request = await cbProjects.Query(user_id, 20086);
            var ucount = request.Item.Count == 0 ? 0 : request.Item[0].Amount;
            if (ucount < 160)
                throw new ArgumentException("兑换所需积分不足!");

            _userDbWork.BeginTransaction();
            try
            {
                string sql = $"INSERT into user_exchange_log(app_id,user_id,item_id,item_name,amount,create_at) VALUES(90410,{user_id},{item_id},'{item_name}',1,NOW())";
                await _userDbWork.Connection.ExecuteScalarAsync<int>(sql);
            }
            catch
            {
                _userDbWork.Dispose();
                throw new ArgumentException("系统异常!");
            }

            var requestv2 = await cbProjects.Subtract(user_id, 90410, _id.UUID(), 20086, 160);

            if (requestv2.Result != 0)
            {
                _logger.LogError($"积分扣道具:user_id ={user_id},Result={requestv2.Result}");
                _userDbWork.Rollback();
                _userDbWork.Dispose();
                throw new ArgumentException("道具扣除失败!");
            }

            var requestv3 = await cbProjects.Add(user_id, 90410, _id.UUID(), 0, "高级贵宾积分兑换", new ItemSystem.Api.Models.ItemActionInfo
            {
                ItemId = item_id,
                Amount = 90,
                Money = 0,
                Expire = 90,
            });

            if (requestv3.Result != 0)
            {
                _logger.LogError($"添加道具:user_id ={user_id},Result={requestv3.Result}");
                _userDbWork.Rollback();
                _userDbWork.Dispose();
                throw new ArgumentException("道具添加失败!");
            }

            _userDbWork.Commit();
            _userDbWork.Dispose();

            return (0, item_name);
        }

        public async Task<(int, string)> UserExchangeFragment(int user_id)
        {
            var request = await cbProjects.Query(user_id, 20086);
            var ucount = request.Item.Count == 0 ? 0 : request.Item[0].Amount;
            if (ucount < 300)
                throw new ArgumentException("兑换所需积分不足!");

            int user_amount = 0;
            _userDbWork.BeginTransaction();
            try
            {
                string sql = $"INSERT into user_exchange_log(app_id,user_id,item_id,item_name,amount,create_at) VALUES(90410,{user_id},20039,'终极碎片*1',1,NOW());SELECT SUM(amount) from user_exchange_log where user_id = {user_id} and item_id = 20039 and app_id=90410 and  year(create_at)=year(date_sub(now(),interval 0 year))";
                user_amount = await _userDbWork.Connection.ExecuteScalarAsync<int>(sql);
            }
            catch
            {
                _userDbWork.Dispose();
                throw new ArgumentException("系统异常!");
            }

            if (user_amount > 3)
            {
                _userDbWork.Rollback();
                _userDbWork.Dispose();
                throw new ArgumentException("全年仅限兑换3个!");
            }

            var requestv2 = await cbProjects.Subtract(user_id, 90410, _id.UUID(), 20086, 300);

            if (requestv2.Result != 0)
            {
                _logger.LogError($"积分扣道具:user_id ={user_id},Result={requestv2.Result}");
                _userDbWork.Rollback();
                _userDbWork.Dispose();
                throw new ArgumentException("道具扣除失败!");
            }

            var requestv3 = await cbProjects.Add(user_id, 90410, _id.UUID(), 0, "高级贵宾积分兑换", new ItemSystem.Api.Models.ItemActionInfo
            {
                ItemId = 20039,
                Amount = 1,
                Money = 0,
                Expire = -1,
            });

            if (requestv3.Result != 0)
            {
                _logger.LogError($"添加道具:user_id ={user_id},Result={requestv3.Result}");
                _userDbWork.Rollback();
                _userDbWork.Dispose();
                throw new ArgumentException("道具添加失败!");
            }

            _userDbWork.Commit();
            _userDbWork.Dispose();

            return (0, "终极碎片*1");
        }

        public async Task<List<dynamic>> GetUserExchangeLog(int user_id)
        {
            string sql = $"SELECT * from user_exchange_log where user_id={user_id} and app_id=90410";
            var tep = await _userDbWork.Connection.QueryAsync<dynamic>(sql);
            return tep.ToList();
        }

        public async Task<int> GetUserFragmentLog(int user_id)
        {
            string sql = $"SELECT SUM(amount) from user_exchange_log where user_id = {user_id} and item_id = 20039 and app_id=90410 and  year(create_at)=year(date_sub(now(),interval 0 year))";
            return await _userDbWork.Connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> GetUserFuWenShi(int user_id)
        {
            int[] Days = new int[] { 6, 0, 1, 2, 3, 4, 5 };
            int day = Days[Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))];
            string sql = $"SELECT SUM(amount) from user_exchange_log where user_id = {user_id} and item_id = 19804 and app_id=90410 and DATE_SUB(CURDATE(), INTERVAL {day} DAY) <= create_at";
            return await _userDbWork.Connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
