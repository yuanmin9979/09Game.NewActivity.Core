using _09Game.NewActivity.Core.Authentication;
using _09Game.NewActivity.Core.Enity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rabbit;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace _09Game.NewActivity.Core.Controllers
{
    public class ActivityController : Controller
    {
        private Token _token;
        private readonly IConfiguration _configuration;
        private UserDb _userDb;
        private readonly ILogger<ActivityController> _logger;

        public ActivityController(IConfiguration configuration, ILogger<ActivityController> logger, Token token, UserDb userDb)
        {
            _token = token;
            _configuration = configuration;
            _userDb = userDb;
            _logger = logger;
        }

        public async Task<IActionResult> Testv2()
        {
            var game_log = await _userDb.Exchange2(1);
            var rooftop_log = await _userDb.Exchange2(3);
            var mogul_log = await _userDb.Exchange2(2);
            dynamic temp = new ExpandoObject();
            temp = game_log.Select(pz =>
            {
                if (pz == null)
                    return new object();
                dynamic o = new ExpandoObject();
                o.game_id = pz.game_id;
                o.hero_type = pz.hero_type;
                o.g_id = pz.g_id + "";
                o.heros = pz.heros;
                return o;
            });

            return Json(new { code = 0, game_log = temp, rooftop_log = rooftop_log, mogul_log = mogul_log });
        }
    }
}
