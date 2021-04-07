using _09Game.NewActivity.Core.Authentication;
using _09Game.NewActivity.Core.Authentication.DbWork;
using _09Game.NewActivity.Core.Enity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _09Game.NewActivity.Core.Controllers
{
    [Route("/")]
    public class VipMissionController : Controller
    {
        private Token _token;
        private readonly IConfiguration _configuration;
        private UserDb _userDb;
        private UserWork _userWork;
        private readonly ILogger<VipMissionController> _logger;

        public VipMissionController(IConfiguration configuration, Token token, UserDb userDb, UserWork userWork, ILogger<VipMissionController> logger)
        {
            _configuration = configuration;
            _token = token;
            _logger = logger;
            _userDb = userDb;
            _userWork = userWork;
        }
        /// <summary>
        /// 积分兑换
        /// </summary>
        /// <param name="token"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet("UserIntegralExchange")]
        public async Task<IActionResult> UserIntegralExchange(string token, int count, int item_id, string item_name)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    return Json(new { code = 4, error = "Token不能为空" });

                var version = int.Parse(token.Split('_')[1].Substring(0, 1));
                var key = _configuration.GetConnectionString("version-" + version);

                var u = _token.Parse(token, version, key);
                if ((u?.UserId ?? 0) == 0)
                    return Json(new { code = 4, error = "Token有误" });
                var userId = u.UserId;
                var code = 0;
                var name = "";
                switch (count)
                {
                    case 1:
                        (code, name) = await _userWork.UserExchangeFuWenShi(userId);
                        return Json(new { code = 0, msg = name });
                    case 2:
                        (code, name) = await _userWork.UserExchangeCJDR(userId);
                        return Json(new { code = 0, msg = name });
                    case 3:
                        (code, name) = await _userWork.UserExchangeSP2(userId, item_id, item_name + "*90天");
                        return Json(new { code = 0, msg = name });
                    case 4:
                        (code, name) = await _userWork.UserExchangeFragment(userId);
                        return Json(new { code = 0, msg = name });
                    default:
                        code = 2;
                        break;
                }
                return Json(new { code = 1, error = "兑换失败！" });

            }
            catch (Exception ex)
            {
                return Json(new { code = -1, error = ex.Message });
            }
        }

        /// <summary>
        /// 积分兑换记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserIntegralExchangeLog")]
        public async Task<IActionResult> GetUserIntegralExchangeLog(string token)
        {
            if (string.IsNullOrEmpty(token))
                return Json(new { code = 4, error = "Token不能为空" });

            var version = int.Parse(token.Split('_')[1].Substring(0, 1));
            var key = _configuration.GetConnectionString("version-" + version);

            var u = _token.Parse(token, version, key);
            if ((u?.UserId ?? 0) == 0)
                return Json(new { code = 4, error = "Token有误" });
            var userId = u.UserId;

            var user = await _userWork.GetUserExchangeLog(userId);
            var number = await _userWork.GetUserFragmentLog(userId);
            var Fws_number = await _userWork.GetUserFuWenShi(userId);
            return Json(new { code = 0, msg = user, fragment = number, Fws_number = Fws_number });
        }
    }
}
