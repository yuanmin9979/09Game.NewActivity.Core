using _09Game.NewActivity.Core.Authentication;
using _09Game.NewActivity.Core.Authentication.DbWork;
using _09Game.NewActivity.Core.Enity;
using _09Game.NewActivity.Core.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _09Game.NewActivity.Core.Controllers
{
    [Route("/")]
    public class PromiseController : Controller
    {
        private Token _token;
        private PifuDb _pifuDb;
        private PifuWork _pifuWork;
        private readonly IConfiguration _configuration;

        public PromiseController(IConfiguration configuration, Token token, PifuDb pifuDb , PifuWork pifuWork)
        {
            _pifuDb = pifuDb;
            _pifuWork = pifuWork;
            _configuration = configuration;
            _token = token;
        }

        /// <summary>
        /// 许愿
        /// </summary>
        /// <param name="token"></param>
        /// <param name="item_id"></param>
        /// <returns></returns>
        [HttpGet("UserPromise")]
        public async Task<IActionResult> UserPromise(string token,int item_id)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime("2021-04-22 00:00:00"), DateTime.Now) < 0)
                    return Json(new { code = -1, msg = "活动已过期！" });

                if (string.IsNullOrEmpty(token))
                    return Json(new { code = 4, msg = "Token不能为空" });

                var version = int.Parse(token.Split('_')[1].Substring(0, 1));
                var key = _configuration.GetConnectionString("version-" + version);

                var u = _token.Parse(token, version, key);
                if ((u?.UserId ?? 0) == 0)
                    return Json(new { code = 4, msg = "Token有误" });
                var userId = u.UserId;
                var userflag = u.UserFlag;
                if (DateTime.Compare(Convert.ToDateTime("2021-04-09 00:00:00"), DateTime.Now) > 0 && ((userflag & 256) != 256))
                    return Json(new { code = 4, msg = "非内部人员!" });

                var code = await _pifuWork.UserPromise(userId, item_id);

                if (code == 0)
                    return Json(new { code = 0, msg = "许愿成功!" });

                return Json(new { code = 3, msg = "许愿失败" });
            }
            catch (NeedMoneyException ex)
            {
                return Json(new { code = ex.code, msg = ex.NeedMoney });

            }
            catch (Exception ex)
            {
                return Json(new { code = -1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 许愿详情
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("GetUserpromise")]
        public async Task<IActionResult> GetUserpromise(string token)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime("2021-05-08 00:00:00"), DateTime.Now) < 0)
                    return Json(new { code = -1, msg = "活动已过期！" });

                if (string.IsNullOrEmpty(token))
                    return Json(new { code = 4, msg = "Token不能为空" });

                var version = int.Parse(token.Split('_')[1].Substring(0, 1));
                var key = _configuration.GetConnectionString("version-" + version);

                var u = _token.Parse(token, version, key);
                if ((u?.UserId ?? 0) == 0)
                    return Json(new { code = 4, msg = "Token有误" });
                var userId = u.UserId;
                var userflag = u.UserFlag;
                if (DateTime.Compare(Convert.ToDateTime("2021-04-09 00:00:00"), DateTime.Now) > 0 && ((userflag & 256) != 256))
                    return Json(new { code = 4, msg = "非内部人员!" });

                var user = await _pifuWork.GetUserPromiseLog(userId);

                if (user == null)
                    return Json(new { code = 3, msg = "未许愿" });
                return Json(new { code = 0, msg = user });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 获取能量详情
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("GetUserEnergyLog")]
        public async Task<IActionResult> GetUserEnergyLog(string token)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime("2021-05-08 00:00:00"), DateTime.Now) < 0)
                    return Json(new { code = -1, msg = "活动已过期！" });

                if (string.IsNullOrEmpty(token))
                    return Json(new { code = 4, msg = "Token不能为空" });

                var version = int.Parse(token.Split('_')[1].Substring(0, 1));
                var key = _configuration.GetConnectionString("version-" + version);

                var u = _token.Parse(token, version, key);
                if ((u?.UserId ?? 0) == 0)
                    return Json(new { code = 4, msg = "Token有误" });
                var userId = u.UserId;
                var userflag = u.UserFlag;
                if (DateTime.Compare(Convert.ToDateTime("2021-04-09 00:00:00"), DateTime.Now) > 0 && ((userflag & 256) != 256))
                    return Json(new { code = 4, msg = "非内部人员!" });

                var user = await _pifuWork.GetUserEnergyLog(userId,1);
                return Json(new { code = 0, msg = user });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 能量升级
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("UserEnergyUpgrade")]
        public async Task<IActionResult> UserEnergyUpgrade(string token)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime("2021-05-08 00:00:00"), DateTime.Now) < 0)
                    return Json(new { code = -1, msg = "活动已过期！" });

                if (string.IsNullOrEmpty(token))
                    return Json(new { code = 4, msg = "Token不能为空" });

                var version = int.Parse(token.Split('_')[1].Substring(0, 1));
                var key = _configuration.GetConnectionString("version-" + version);

                var u = _token.Parse(token, version, key);
                if ((u?.UserId ?? 0) == 0)
                    return Json(new { code = 4, msg = "Token有误" });
                var userId = u.UserId;
                var userflag = u.UserFlag;
                if (DateTime.Compare(Convert.ToDateTime("2021-04-09 00:00:00"), DateTime.Now) > 0 && ((userflag & 256) != 256))
                    return Json(new { code = 4, msg = "非内部人员!" });

                await _pifuWork.UserEnergyUpgrade(userId);
                return Json(new { code = 0, msg = "升级成功!" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, msg = ex.Message });
            }
        }


        [HttpGet("TestAddItem")]
        public async Task<IActionResult> TestAddItem(string token)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime("2021-05-08 00:00:00"), DateTime.Now) < 0)
                    return Json(new { code = -1, msg = "活动已过期！" });

                if (string.IsNullOrEmpty(token))
                    return Json(new { code = 4, msg = "Token不能为空" });

                var version = int.Parse(token.Split('_')[1].Substring(0, 1));
                var key = _configuration.GetConnectionString("version-" + version);

                var u = _token.Parse(token, version, key);
                if ((u?.UserId ?? 0) == 0)
                    return Json(new { code = 4, msg = "Token有误" });
                var userId = u.UserId;
                var userflag = u.UserFlag;
                if (DateTime.Compare(Convert.ToDateTime("2021-04-09 00:00:00"), DateTime.Now) > 0 && ((userflag & 256) != 256))
                    return Json(new { code = 4, msg = "非内部人员!" });

                var code = await _pifuDb.TestAdd(userId);
                return Json(new { code = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, msg = ex.Message });
            }
        }
    }
}
