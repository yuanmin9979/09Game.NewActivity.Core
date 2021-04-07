using _09Game.NewActivity.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace _09Game.NewActivity.Core.Authentication
{
    public class Token
    {
        public Identity Parse(string token, int versionCode, string versionKey)
        {
            //int versionCode = 1;
            //string versionKey = "6326f4b9ae66bf535612dd40e61fd36da69fa21d83628a438de6f959f5d893884ce07761c0e3b5e125e6bdd4d8252497142a3ed20d8ff31632914a3f2b6c4555";

            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            var sections = token.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            if (sections.Length != 2)
            {
                return null;
            }
            var primarySection = sections[0];
            var signSection = sections[1];

            var signSections = signSection.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (signSections.Length != 2)
            {
                return null;
            }
            int signVersionCode = int.TryParse(signSections[0], out int code) ? code : 0;
            if (signVersionCode != versionCode)
            {
                return null;
            }
            string sign = signSections[1];
            if (sign.IsNullOrWhiteSpace())
            {
                return null;
            }
            var key = $"{primarySection}-{versionKey}".ToMD5();
            if (!key.Equals(sign))
            {
                return null;
            }
            try
            {
                int userId = 0;
                long userFlag = 0;
                var infos = primarySection.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                long unixTimeStamp = 1478162177;
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

                try
                {
                    var expTime = startTime.AddSeconds(long.TryParse(infos[3], out long exp1) ? exp1 : 0L);
                    if (DateTime.Compare(Convert.ToDateTime(expTime), DateTime.Now) < 0)
                        return null;
                }
                catch
                {
                    return null;
                }

                return new Identity()
                {
                    UserId = int.TryParse(infos[0], out userId) ? userId : 0,
                    UserName = Encoding.UTF8.GetString(infos[1].HexToBytes()),
                    UserFlag = long.TryParse(infos[2], out userFlag) ? userFlag : 0,
                    ExpireTime = startTime.AddSeconds(long.TryParse(infos[3], out long exp) ? exp : 0L),
                    ArchieveLevel = infos.Length == 5 ? (int.TryParse(infos[4], out int rid) ? rid : 0) : 0
                };
            }
            catch { }

            return null;
        }


        public int UserId { get; set; }

        public string UserName { get; set; }

        public long Flag { get; set; }

        public DateTime ExpireTime { get; set; }

        public int ArchieveLevel { get; set; }


        public string Generate(long timeStamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime newTime = dtStart.AddSeconds(timeStamp);

            int versionCode = 1;
            string versionKey = "6326f4b9ae66bf535612dd40e61fd36da69fa21d83628a438de6f959f5d893884ce07761c0e3b5e125e6bdd4d8252497142a3ed20d8ff31632914a3f2b6c4555";

            var info = $"{UserId}-{Encoding.UTF8.GetBytes(UserName).ToHex()}-{Flag}-{timeStamp}-{ArchieveLevel}";
            var sign = $"{info}-{versionKey}".ToMD5();
            return $"{info}_{versionCode}-{sign}";
        }

        public string Getsign()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long timeStamp = (long)(ExpireTime.Date - startTime).TotalSeconds;

            int versionCode = 1;
            string versionKey = "6326f4b9ae66bf535612dd40e61fd36da69fa21d83628a438de6f959f5d893884ce07761c0e3b5e125e6bdd4d8252497142a3ed20d8ff31632914a3f2b6c4555";

            var info = $"{UserId}-{Encoding.UTF8.GetBytes(UserName).ToHex()}-{Flag}-{timeStamp}-{ArchieveLevel}";
            var sign = $"{info}-{versionKey}".ToMD5();
            return $"{sign}";
        }
    }
}
