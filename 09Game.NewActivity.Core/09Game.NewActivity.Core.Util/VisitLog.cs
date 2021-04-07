using _09Game.NewActivity.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _09Game.NewActivity.Core.Util
{
    public class VisitLog
    {
        public class Requestcount { 
            public string request { get; set; }
            public int count { get; set; }
        }

        static List<Requestcount> list = new List<Requestcount>();

        public static void Visit(string request)
        {
            list.Add(new Requestcount { request = request, count = 1 });
        }

        public static void saveLog()
        {
            var ListNew = list.GroupBy(x => x.request).Select(g => new { request = g.Key, count = g.Count() }).ToList();

            foreach (var list in ListNew)
            {
                Logger.Info($"Request_list:Request = {list.request}:::count = {list.count}");
            }

            list.Clear();
        }
    }
}
