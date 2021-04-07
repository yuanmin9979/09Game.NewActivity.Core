using System;
using System.Collections.Generic;
using System.Text;

namespace _09Game.NewActivity.Core.Authentication
{
    public class Identity
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public long UserFlag { get; set; }

        public int ArchieveLevel { get; set; }

        public DateTime ExpireTime { get; set; }
    }
}
