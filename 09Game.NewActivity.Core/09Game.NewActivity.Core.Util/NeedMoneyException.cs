using System;
using System.Collections.Generic;
using System.Text;

namespace _09Game.NewActivity.Core.Util
{
    public class NeedMoneyException : Exception
    {
        public int code { get; set; }

        public int NeedMoney { get; set; }
    }
}
